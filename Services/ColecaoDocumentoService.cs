using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class ColecaoDocumentoService
    {
        private DreamDbContext context;

        public ColecaoDocumentoService(DreamDbContext context)
        {
            this.context = context;
        }

        // todas as coleções do usuário, marcando as que já têm a publicação
        public List<ColecaoOpcaoDto> ListarComMarcacao(int idUsuario, int idDocumento)
        {
            List<int> idsNaColecao = context.ColecoesDocumentos
                .AsNoTracking()
                .Where(cd => cd.IdDocumento == idDocumento)
                .Select(cd => cd.IdColecao)
                .ToList();

            List<Colecao> colecoes = context.Colecoes
                .AsNoTracking()
                .Where(c => c.IdUsuario == idUsuario)
                .OrderBy(c => c.Titulo)
                .ToList();

            return colecoes
                .Select(c => new ColecaoOpcaoDto
                {
                    Id = c.Id,
                    Titulo = c.Titulo,
                    Contem = idsNaColecao.Contains(c.Id)
                })
                .ToList();
        }

        public void Adicionar(int idColecao, int idDocumento)
        {
            bool existe = context.ColecoesDocumentos
                .Any(cd => cd.IdColecao == idColecao && cd.IdDocumento == idDocumento);

            if (!existe)
            {
                context.ColecoesDocumentos.Add(new ColecaoDocumento
                {
                    IdColecao = idColecao,
                    IdDocumento = idDocumento
                });

                context.SaveChanges();
            }
        }

        public void Remover(int idColecao, int idDocumento)
        {
            ColecaoDocumento? item = context.ColecoesDocumentos
                .FirstOrDefault(cd => cd.IdColecao == idColecao && cd.IdDocumento == idDocumento);

            if (item != null)
            {
                context.ColecoesDocumentos.Remove(item);
                context.SaveChanges();
            }
        }
    }
}