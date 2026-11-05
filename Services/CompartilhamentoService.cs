using DreamApi.Data;
using DreamApi.Models;

namespace DreamApi.Services
{
    public class CompartilhamentoService
    {
        private DreamDbContext context;

        public CompartilhamentoService(DreamDbContext context)
        {
            this.context = context;
        }

        public List<Compartilhamento> Listar()
        {
            return context.Compartilhamentos.ToList();
        }

        public void Adicionar(Compartilhamento compartilhamento)
        {
            context.Compartilhamentos.Add(compartilhamento);
            context.SaveChanges();
        }

        public void Editar(Compartilhamento compartilhamento)
        {
            Compartilhamento compartilhamentoExistente = context.Compartilhamentos.Find(compartilhamento.Id);

            if (compartilhamentoExistente != null)
            {
                compartilhamentoExistente.IdDocumento = compartilhamento.IdDocumento;
                compartilhamentoExistente.IdGrupo = compartilhamento.IdGrupo;
                compartilhamentoExistente.DataUltimaAcao = compartilhamento.DataUltimaAcao;

                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            Compartilhamento compartilhamento = context.Compartilhamentos.Find(id);

            if (compartilhamento != null)
            {
                context.Compartilhamentos.Remove(compartilhamento);
                context.SaveChanges();
            }
        }
    }
}