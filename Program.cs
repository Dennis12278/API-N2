using System.Text;
using Scalar.AspNetCore;
using DreamApi.Services;
using DreamApi.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirSite", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<DreamDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ConexaoDream")));

// ===== JWT =====
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException("Jwt:Key não configurada."))),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

// Tudo exige login, exceto o que tiver [AllowAnonymous] (o AuthController)
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<AnotacaoService>();
builder.Services.AddScoped<DocumentoService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TipoDocumentoService>();
builder.Services.AddScoped<GrupoService>();
builder.Services.AddScoped<GrupoUsuarioService>();
builder.Services.AddScoped<CompartilhamentoService>();
builder.Services.AddScoped<NotificacaoService>();
builder.Services.AddScoped<CalendarioService>();
builder.Services.AddScoped<ColecaoService>();
builder.Services.AddScoped<ColecaoDocumentoService>();
builder.Services.AddScoped<ColecaoPublicaService>();
builder.Services.AddScoped<MarcadorPublicacaoService>();
builder.Services.AddScoped<HistoricoLeituraService>();
builder.Services.AddScoped<PerfilService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // documentação visível sem login
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference().AllowAnonymous();
}

app.UseStaticFiles();   

app.UseCors("PermitirSite");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();