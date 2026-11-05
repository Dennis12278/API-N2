using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class ColecaoPublicaService
    {
        private DreamDbContext context;

        public ColecaoPublicaService(DreamDbContext context)
        {
            this.context = context;
        }

        /* =========================================
           LISTAGENS (cards)
        ========================================= */

        public List<ColecaoCardDto> ListarPublicas(int idUsuario)
        {
            List<Colecao> colecoes = context.Colecoes
                .AsNoTracking()
                .Where(c => c.Visibilidade == "publica")
                .OrderByDescending(c => c.DataCriacao)
                .Take(30)
                .ToList();

            return MontarCards(colecoes, idUsuario);
        }

        // coleções que o usuário curtiu (só as públicas)
        public List<ColecaoCardDto> ListarCurtidas(int idUsuario)
        {
            List<int> idsCurtidos = context.ColecoesCurtidas
                .AsNoTracking()
                .Where(cc => cc.IdUsuario == idUsuario)
                .Select(cc => cc.IdColecao)
                .ToList();

            List<Colecao> colecoes = context.Colecoes
                .AsNoTracking()
                .Where(c => idsCurtidos.Contains(c.Id) && c.Visibilidade == "publica")
                .OrderByDescending(c => c.DataCriacao)
                .ToList();

            return MontarCards(colecoes, idUsuario);
        }

        // coleções criadas pelo próprio usuário (públicas e privadas)
        public List<ColecaoCardDto> ListarMinhas(int idUsuario)
        {
            List<Colecao> colecoes = context.Colecoes
                .AsNoTracking()
                .Where(c => c.IdUsuario == idUsuario)
                .OrderByDescending(c => c.DataCriacao)
                .ToList();

            return MontarCards(colecoes, idUsuario);
        }

        private List<ColecaoCardDto> MontarCards(List<Colecao> colecoes, int idUsuario)
        {
            List<int> ids = colecoes.Select(c => c.Id).ToList();

            var vinculos = context.ColecoesDocumentos
                .AsNoTracking()
                .Where(cd => ids.Contains(cd.IdColecao))
                .Join(
                    context.Documentos.AsNoTracking(),
                    cd => cd.IdDocumento,
                    d => d.Id,
                    (cd, d) => new { cd.IdColecao, cd.Ordem, d.Capa })
                .ToList();

            var curtidas = context.ColecoesCurtidas
                .AsNoTracking()
                .Where(cc => ids.Contains(cc.IdColecao))
                .GroupBy(cc => cc.IdColecao)
                .ToDictionary(g => g.Key, g => g.Count());

            HashSet<int> curtidasDoUsuario = context.ColecoesCurtidas
                .AsNoTracking()
                .Where(cc => cc.IdUsuario == idUsuario && ids.Contains(cc.IdColecao))
                .Select(cc => cc.IdColecao)
                .ToHashSet();

            List<int> donos = colecoes.Select(c => c.IdUsuario).Distinct().ToList();

            Dictionary<int, string> nomes = context.Usuarios
                .AsNoTracking()
                .Where(u => donos.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u.Nome);

            return colecoes.Select(c => new ColecaoCardDto
            {
                Id = c.Id,
                IdUsuario = c.IdUsuario,
                Curtiu = curtidasDoUsuario.Contains(c.Id),
                Titulo = c.Titulo,
                Visibilidade = c.Visibilidade,
                NomeUsuario = nomes.ContainsKey(c.IdUsuario) ? nomes[c.IdUsuario] : "",
                Capas = vinculos
                    .Where(v => v.IdColecao == c.Id)
                    .OrderBy(v => v.Ordem)
                    .Take(3)
                    .Select(v => v.Capa)
                    .ToList(),
                QuantidadeTitulos = vinculos.Count(v => v.IdColecao == c.Id),
                DataCriacao = c.DataCriacao,
                Curtidas = curtidas.ContainsKey(c.Id) ? curtidas[c.Id] : 0
            }).ToList();
        }

        /* =========================================
           DETALHE
        ========================================= */

        public ColecaoDetalheDto? Obter(int id, int idUsuario)
        {
            Colecao? colecao = context.Colecoes
                .AsNoTracking()
                .FirstOrDefault(c => c.Id == id);

            if (colecao == null)
            {
                return null;
            }

            // coleção privada só aparece para o dono
            if (colecao.Visibilidade != "publica" && colecao.IdUsuario != idUsuario)
            {
                return null;
            }

            List<ColecaoItemDto> itens = context.ColecoesDocumentos
                .AsNoTracking()
                .Where(cd => cd.IdColecao == id)
                .Join(
                    context.Documentos.AsNoTracking(),
                    cd => cd.IdDocumento,
                    d => d.Id,
                    (cd, d) => new ColecaoItemDto
                    {
                        Id = d.Id,
                        Titulo = d.Titulo,
                        Capa = d.Capa,
                        DataPublicacao = d.DataPublicacao,
                        Ordem = cd.Ordem
                    })
                .OrderBy(i => i.Ordem)
                .ToList();

            var curtidas = context.ColecoesCurtidas
                .AsNoTracking()
                .Where(cc => cc.IdColecao == id);

            string nome = context.Usuarios
                .AsNoTracking()
                .Where(u => u.Id == colecao.IdUsuario)
                .Select(u => u.Nome)
                .FirstOrDefault() ?? "";

            return new ColecaoDetalheDto
            {
                Id = colecao.Id,
                Titulo = colecao.Titulo,
                Descricao = colecao.Descricao,
                Visibilidade = colecao.Visibilidade,
                IdUsuario = colecao.IdUsuario,
                NomeUsuario = nome,
                DataCriacao = colecao.DataCriacao,
                Curtidas = curtidas.Count(),
                Curtiu = curtidas.Any(cc => cc.IdUsuario == idUsuario),
                Itens = itens
            };
        }

        /* =========================================
           CRIAR / EDITAR / ITENS
        ========================================= */

        public int Criar(ColecaoSalvarDto dto)
        {
            Colecao colecao = new Colecao
            {
                IdUsuario = dto.IdUsuario,
                Titulo = dto.Titulo.Trim(),
                Descricao = dto.Descricao ?? "",
                Visibilidade = dto.Visibilidade == "privada" ? "privada" : "publica",
                DataCriacao = DateTime.UtcNow
            };

            context.Colecoes.Add(colecao);
            context.SaveChanges();

            return colecao.Id;
        }

        public bool Atualizar(int id, ColecaoSalvarDto dto)
        {
            Colecao? colecao = context.Colecoes.Find(id);

            if (colecao == null || colecao.IdUsuario != dto.IdUsuario)
            {
                return false;
            }

            colecao.Titulo = dto.Titulo.Trim();
            colecao.Descricao = dto.Descricao ?? "";
            colecao.Visibilidade = dto.Visibilidade == "privada" ? "privada" : "publica";

            context.SaveChanges();

            return true;
        }

        public bool SalvarItens(int id, ColecaoItensDto dto)
        {
            Colecao? colecao = context.Colecoes.Find(id);

            if (colecao == null || colecao.IdUsuario != dto.IdUsuario)
            {
                return false;
            }

            List<ColecaoDocumento> atuais = context.ColecoesDocumentos
                .Where(cd => cd.IdColecao == id)
                .ToList();

            context.ColecoesDocumentos.RemoveRange(atuais);

            List<int> idsUnicos = dto.Ids.Distinct().ToList();

            for (int i = 0; i < idsUnicos.Count; i++)
            {
                context.ColecoesDocumentos.Add(new ColecaoDocumento
                {
                    IdColecao = id,
                    IdDocumento = idsUnicos[i],
                    Ordem = i
                });
            }

            context.SaveChanges();

            return true;
        }

        /* =========================================
           CURTIR / EXCLUIR
        ========================================= */

        // retorna true se ficou curtida, false se a curtida foi removida
        public bool Curtir(int id, int idUsuario)
        {
            ColecaoCurtida? existente = context.ColecoesCurtidas
                .FirstOrDefault(cc => cc.IdColecao == id && cc.IdUsuario == idUsuario);

            if (existente != null)
            {
                context.ColecoesCurtidas.Remove(existente);
                context.SaveChanges();
                return false;
            }

            context.ColecoesCurtidas.Add(new ColecaoCurtida
            {
                IdColecao = id,
                IdUsuario = idUsuario
            });

            context.SaveChanges();
            return true;
        }

        public bool Excluir(int id, int idUsuario)
        {
            Colecao? colecao = context.Colecoes.Find(id);

            if (colecao == null || colecao.IdUsuario != idUsuario)
            {
                return false;
            }

            context.ColecoesCurtidas.RemoveRange(
                context.ColecoesCurtidas.Where(cc => cc.IdColecao == id));

            context.ColecoesDocumentos.RemoveRange(
                context.ColecoesDocumentos.Where(cd => cd.IdColecao == id));

            context.Colecoes.Remove(colecao);
            context.SaveChanges();

            return true;
        }
    }
}