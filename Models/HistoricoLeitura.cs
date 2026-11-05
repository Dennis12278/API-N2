namespace DreamApi.Models
{
    public class HistoricoLeitura
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdDocumento { get; set; }
        public int Progresso { get; set; }
        public DateTime DataUltimaLeitura { get; set; }
    }
}