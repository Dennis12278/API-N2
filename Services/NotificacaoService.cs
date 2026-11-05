using DreamApi.Data;
using DreamApi.Models;

namespace DreamApi.Services
{
    public class NotificacaoService
    {
        private DreamDbContext context;

        public NotificacaoService(DreamDbContext context)
        {
            this.context = context;
        }

        public List<Notificacao> Listar()
        {
            return context.Notificacoes.ToList();
        }

        public void Adicionar(Notificacao notificacao)
        {
            context.Notificacoes.Add(notificacao);
            context.SaveChanges();
        }

        public void Editar(Notificacao notificacao)
        {
            Notificacao notificacaoExistente = context.Notificacoes.Find(notificacao.Id);

            if (notificacaoExistente != null)
            {
                notificacaoExistente.Mensagem = notificacao.Mensagem;
                notificacaoExistente.Status = notificacao.Status;
                notificacaoExistente.DataUltimaAcao = notificacao.DataUltimaAcao;

                context.SaveChanges();
            }
        }

        public void Ler(int id)
        {
            Notificacao notificacao = context.Notificacoes.Find(id);

            if (notificacao != null)
            {
                notificacao.Status = "lida";
                notificacao.DataUltimaAcao = DateTime.Now;

                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            Notificacao notificacao = context.Notificacoes.Find(id);

            if (notificacao != null)
            {
                context.Notificacoes.Remove(notificacao);
                context.SaveChanges();
            }
        }
    }
}