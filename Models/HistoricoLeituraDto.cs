namespace DreamApi.Models
{
    public class HistoricoLeituraDto
    {
        public int Id { get; set; }
        public int IdDocumento { get; set; }
        public string Titulo { get; set; } = "";
        public string? Capa { get; set; }
        public int Progresso { get; set; }
        public DateTime DataUltimaLeitura { get; set; }
    }
}