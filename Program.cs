using Scalar.AspNetCore;
using DreamApi.Services;
using DreamApi.Data;
using Microsoft.EntityFrameworkCore;

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


builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseStaticFiles();   // serve as imagens de wwwroot/capas

app.UseCors("PermitirSite");

app.UseAuthorization();

app.MapControllers();

app.Run();