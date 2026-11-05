namespace DreamApi.Models
{
    public class Compartilhamento
    {
        public int Id { get; set; }
        public int IdDocumento { get; set; }
        public int IdGrupo { get; set; }
        public DateTime DataUltimaAcao { get; set; }
    }
}