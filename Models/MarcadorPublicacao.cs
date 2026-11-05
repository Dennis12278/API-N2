namespace DreamApi.Models
{
    public class MarcadorPublicacao
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public int IdDocumento { get; set; }
        public string Marcador { get; set; } = "";
    }
}