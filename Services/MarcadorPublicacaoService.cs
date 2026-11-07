using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class MarcadorPublicacaoService
    {
        private DreamDbContext context;

        public MarcadorPublicacaoService(DreamDbContext context)
        {
            this.context = context;
        }

        public string Obter(int idUsuario, int idDocumento)
        {
            return context.MarcadoresPublicacao
                .AsNoTracking()
                .Where(m => m.IdUsuario == idUsuario && m.IdDocumento == idDocumento)
                .Select(m => m.Marcador)
                .FirstOrDefault() ?? "";
        }

        // todas as publicações marcadas pelo usuário, com o marcador de cada uma
        public List<TituloSeguidoDto> ListarDoUsuario(int idUsuario)
        {
            return context.MarcadoresPublicacao
                .AsNoTracking()
                .Where(m => m.IdUsuario == idUsuario)
                .Join(
                    context.Documentos.AsNoTracking(),
                    m => m.IdDocumento,
                    d => d.Id,
                    (m, d) => new TituloSeguidoDto
                    {
                        Id = d.Id,
                        Titulo = d.Titulo,
                        Capa = d.Capa,
                        Marcador = m.Marcador,
                        DataPublicacao = d.DataPublicacao
                    })
                .OrderByDescending(t => t.DataPublicacao)
                .ToList();
        }

        public void Definir(int idUsuario, int idDocumento, string marcador)
        {
            MarcadorPublicacao? existente = context.MarcadoresPublicacao
                .FirstOrDefault(m => m.IdUsuario == idUsuario && m.IdDocumento == idDocumento);

            // marcador vazio remove o que existia
            if (string.IsNullOrWhiteSpace(marcador))
            {
                if (existente != null)
                {
                    context.MarcadoresPublicacao.Remove(existente);
                    context.SaveChanges();
                }

                return;
            }

            if (existente != null)
            {
                existente.Marcador = marcador;
            }
            else
            {
                context.MarcadoresPublicacao.Add(new MarcadorPublicacao
                {
                    IdUsuario = idUsuario,
                    IdDocumento = idDocumento,
                    Marcador = marcador
                });
            }

            context.SaveChanges();
        }
    }
}