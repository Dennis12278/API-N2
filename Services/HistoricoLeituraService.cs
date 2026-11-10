using DreamApi.Data;
using DreamApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DreamApi.Services
{
    public class HistoricoLeituraService
    {
        private DreamDbContext context;

        public HistoricoLeituraService(DreamDbContext context)
        {
            this.context = context;
        }

        public List<HistoricoLeituraDto> ListarDoUsuario(int idUsuario)
        {
            return context.HistoricosLeitura
                .AsNoTracking()
                .Where(h => h.IdUsuario == idUsuario)
                .Join(
                    context.Documentos.AsNoTracking(),
                    h => h.IdDocumento,
                    d => d.Id,
                    (h, d) => new HistoricoLeituraDto
                    {
                        Id = h.Id,
                        IdDocumento = d.Id,
                        Titulo = d.Titulo,
                        Capa = d.Capa,
                        Progresso = h.Progresso,
                        DataUltimaLeitura = h.DataUltimaLeitura
                    })
                .OrderByDescending(item => item.DataUltimaLeitura)
                .ToList();
        }

        public void Registrar(int idUsuario, int idDocumento)
        {
            HistoricoLeitura? existente = context.HistoricosLeitura
                .FirstOrDefault(h => h.IdUsuario == idUsuario && h.IdDocumento == idDocumento);

            if (existente != null)
            {
                existente.DataUltimaLeitura = DateTime.UtcNow;
            }
            else
            {
                context.HistoricosLeitura.Add(new HistoricoLeitura
                {
                    IdUsuario = idUsuario,
                    IdDocumento = idDocumento,
                    Progresso = 0,
                    DataUltimaLeitura = DateTime.UtcNow
                });
            }

            context.SaveChanges();
        }

        // grava o progresso (0 a 100); nunca diminui o que já foi lido
        public void AtualizarProgresso(int idUsuario, int idDocumento, int progresso)
        {
            HistoricoLeitura? historico = context.HistoricosLeitura
                .FirstOrDefault(h => h.IdUsuario == idUsuario && h.IdDocumento == idDocumento);

            int valor = Math.Clamp(progresso, 0, 100);

            if (historico == null)
            {
                context.HistoricosLeitura.Add(new HistoricoLeitura
                {
                    IdUsuario = idUsuario,
                    IdDocumento = idDocumento,
                    Progresso = valor,
                    DataUltimaLeitura = DateTime.UtcNow
                });
            }
            else
            {
                historico.Progresso = Math.Max(historico.Progresso, valor);
                historico.DataUltimaLeitura = DateTime.UtcNow;
            }

            context.SaveChanges();
        }

        public void Excluir(int id)
        {
            HistoricoLeitura? historico = context.HistoricosLeitura.Find(id);

            if (historico != null)
            {
                context.HistoricosLeitura.Remove(historico);
                context.SaveChanges();
            }
        }
    }
}