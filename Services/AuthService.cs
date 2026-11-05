using DreamApi.Data;
using DreamApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class AuthService
    {
        private readonly DreamDbContext context;
        private readonly PasswordHasher<Usuario> hasher = new PasswordHasher<Usuario>();

        public AuthService(DreamDbContext context)
        {
            this.context = context;
        }

        public bool EmailJaExiste(string email)
        {
            string emailLimpo = email.Trim();

            return context.Usuarios.Any(u => u.Email == emailLimpo);
        }

        public Usuario Cadastrar(CadastroDto dto)
        {
            Usuario usuario = new Usuario
            {
                Nome = dto.Nome.Trim(),
                Email = dto.Email.Trim()
            };

            usuario.Senha = hasher.HashPassword(usuario, dto.Senha);

            context.Usuarios.Add(usuario);
            context.SaveChanges();

            return usuario;
        }

        public Usuario? Validar(LoginDto dto)
        {
            string emailLimpo = dto.Email.Trim();

            Usuario? usuario = context.Usuarios
                .AsNoTracking()
                .FirstOrDefault(u => u.Email == emailLimpo);

            if (usuario == null)
            {
                return null;
            }

            try
            {
                PasswordVerificationResult resultado =
                    hasher.VerifyHashedPassword(usuario, usuario.Senha, dto.Senha);

                return resultado == PasswordVerificationResult.Failed
                    ? null
                    : usuario;
            }
            catch (FormatException)
            {
                // senha antiga em texto puro (do script): não é um hash válido
                return null;
            }
        }
    }
}