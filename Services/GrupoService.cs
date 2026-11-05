using DreamApi.Data;
using DreamApi.Models;

namespace DreamApi.Services
{
    public class GrupoService
    {
        private DreamDbContext context;

        public GrupoService(DreamDbContext context)
        {
            this.context = context;
        }

        public List<Grupo> Listar()
        {
            return context.Grupos.ToList();
        }

        public void Adicionar(Grupo grupo)
        {
            context.Grupos.Add(grupo);
            context.SaveChanges();
        }

        public void Editar(Grupo grupo)
        {
            Grupo? grupoExistente = context.Grupos.Find(grupo.Id);

            if (grupoExistente != null)
            {
                grupoExistente.Nome = grupo.Nome;
                grupoExistente.Descricao = grupo.Descricao;
                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            Grupo? grupo = context.Grupos.Find(id);

            if (grupo != null)
            {
                context.Grupos.Remove(grupo);
                context.SaveChanges();
            }
        }
    }
}