using DreamApi.Data;
using DreamApi.Models;

namespace DreamApi.Services
{
    public class TipoDocumentoService
    {
        private DreamDbContext context;

        public TipoDocumentoService(DreamDbContext context)
        {
            this.context = context;
        }

        public List<TipoDocumento> Listar()
        {
            return context.TiposDocumento.ToList();
        }

        public void Adicionar(TipoDocumento tipo)
        {
            context.TiposDocumento.Add(tipo);
            context.SaveChanges();
        }

        public void Editar(TipoDocumento tipo)
        {
            TipoDocumento tipoExistente = context.TiposDocumento.Find(tipo.Id);

            if (tipoExistente != null)
            {
                tipoExistente.Nome = tipo.Nome;

                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            TipoDocumento tipo = context.TiposDocumento.Find(id);

            if (tipo != null)
            {
                context.TiposDocumento.Remove(tipo);
                context.SaveChanges();
            }
        }
    }
}