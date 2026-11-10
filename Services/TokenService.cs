using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DreamApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace DreamApi.Services
{
    public class TokenService
    {
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Gerar(Usuario usuario)
        {
            string chave = configuration["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key não está configurada no appsettings.json.");

            SigningCredentials credenciais = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave)),
                SecurityAlgorithms.HmacSha256);

            Claim[] claims =
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Nome),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credenciais);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}