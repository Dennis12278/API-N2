using DreamApi.Data;
using DreamApi.Models;

namespace DreamApi.Services
{
    public class AnotacaoService
    {
        private DreamDbContext context;

        public AnotacaoService(DreamDbContext context)
        {
            this.context = context;
        }

        public List<Anotacao> Listar()
        {
            return context.Anotacoes.ToList();
        }

        public void Adicionar(Anotacao anotacao)
        {
            context.Anotacoes.Add(anotacao);
            context.SaveChanges();
        }

        public void Editar(Anotacao anotacao)
        {
            Anotacao anotacaoExistente = context.Anotacoes.Find(anotacao.Id);

            if (anotacaoExistente != null)
            {
                anotacaoExistente.Titulo = anotacao.Titulo;
                anotacaoExistente.Texto = anotacao.Texto;

                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            Anotacao anotacao = context.Anotacoes.Find(id);

            if (anotacao != null)
            {
                context.Anotacoes.Remove(anotacao);
                context.SaveChanges();
            }
        }
    }
}