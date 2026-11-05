namespace DreamApi.Models
{
    public class Calendario
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }
    }
}