using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class PerfilService
    {
        private DreamDbContext context;

        public PerfilService(DreamDbContext context)
        {
            this.context = context;
        }

        private Usuario? Buscar(int id)
        {
            return context.Usuarios.AsNoTracking().FirstOrDefault(u => u.Id == id);
        }

        private int ContarTitulos(int id) =>
            context.MarcadoresPublicacao.Count(m => m.IdUsuario == id);

        private int ContarPublicacoes(int id) =>
            context.Documentos.Count(d => d.IdUsuario == id && d.Publicado);

        private int ContarColecoesPublicas(int id) =>
            context.Colecoes.Count(c => c.IdUsuario == id && c.Visibilidade == "publica");

        private int ContarGrupos(int id) =>
            context.GruposUsuarios.Count(g => g.IdUsuario == id);

        public PerfilPublicoDto? ObterPublico(int id)
        {
            Usuario? usuario = Buscar(id);

            if (usuario == null)
            {
                return null;
            }

            return new PerfilPublicoDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Biografia = usuario.Biografia ?? "",
                Links = usuario.Links ?? "",
                Avatar = usuario.Avatar,
                Titulos = ContarTitulos(id),
                Publicacoes = ContarPublicacoes(id),
                Colecoes = ContarColecoesPublicas(id),
                Grupos = ContarGrupos(id)
            };
        }

        public PerfilMeuDto? ObterMeu(int id)
        {
            Usuario? usuario = Buscar(id);

            if (usuario == null)
            {
                return null;
            }

            return new PerfilMeuDto
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Biografia = usuario.Biografia ?? "",
                Links = usuario.Links ?? "",
                Avatar = usuario.Avatar,
                Titulos = ContarTitulos(id),
                Publicacoes = ContarPublicacoes(id),
                Colecoes = context.Colecoes.Count(c => c.IdUsuario == id),
                Grupos = ContarGrupos(id)
            };
        }

        public bool Salvar(int id, PerfilSalvarDto dto)
        {
            Usuario? usuario = context.Usuarios.Find(id);

            if (usuario == null)
            {
                return false;
            }

            usuario.Nome = dto.Nome.Trim();
            usuario.Email = dto.Email.Trim();
            usuario.Biografia = dto.Biografia ?? "";
            usuario.Links = dto.Links ?? "";

            
            if (dto.Avatar != null)
            {
                usuario.Avatar = dto.Avatar == "" ? null : dto.Avatar;
            }

            context.SaveChanges();

            return true;
        }
    }
}