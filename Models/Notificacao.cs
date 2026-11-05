namespace DreamApi.Models
{
    public class Notificacao
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public string Status { get; set; }
        public DateTime DataUltimaAcao { get; set; }
    }
}