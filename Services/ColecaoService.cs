using DreamApi.Data;
using DreamApi.Models;

namespace DreamApi.Services
{
    public class ColecaoService
    {
        private DreamDbContext context;

        public ColecaoService(DreamDbContext context)
        {
            this.context = context;
        }

        public List<Colecao> Listar()
        {
            return context.Colecoes.ToList();
        }

        public void Adicionar(Colecao colecao)
        {
            context.Colecoes.Add(colecao);
            context.SaveChanges();
        }

        public void Editar(Colecao colecao)
        {
            Colecao? colecaoExistente = context.Colecoes.Find(colecao.Id);

            if (colecaoExistente != null)
            {
                colecaoExistente.IdUsuario = colecao.IdUsuario;
                colecaoExistente.Titulo = colecao.Titulo;
                colecaoExistente.Descricao = colecao.Descricao;
                colecaoExistente.Visibilidade = colecao.Visibilidade;

                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            Colecao? colecao = context.Colecoes.Find(id);

            if (colecao != null)
            {
                context.Colecoes.Remove(colecao);
                context.SaveChanges();
            }
        }
    }
}