using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class CalendarioService
    {
        private DreamDbContext context;

        public CalendarioService(DreamDbContext context)
        {
            this.context = context;
        }

        public List<Calendario> Listar(int? idUsuario)
        {
            IQueryable<Calendario> consulta = context.Calendarios.AsNoTracking();

            if (idUsuario.HasValue)
            {
                consulta = consulta.Where(c => c.IdUsuario == idUsuario.Value);
            }

            return consulta.OrderBy(c => c.Data).ToList();
        }

        public Calendario Adicionar(Calendario calendario)
        {
            context.Calendarios.Add(calendario);
            context.SaveChanges();

            return calendario;
        }

        public bool Editar(Calendario calendario)
        {
            Calendario? existente = context.Calendarios.Find(calendario.Id);

            if (existente == null)
            {
                return false;
            }

            existente.IdUsuario = calendario.IdUsuario;
            existente.Data = calendario.Data;
            existente.Descricao = calendario.Descricao;

            context.SaveChanges();

            return true;
        }

        public bool Excluir(int id)
        {
            Calendario? calendario = context.Calendarios.Find(id);

            if (calendario == null)
            {
                return false;
            }

            context.Calendarios.Remove(calendario);
            context.SaveChanges();

            return true;
        }
    }
}