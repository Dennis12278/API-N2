namespace DreamApi.Models
{
    public class TituloSeguidoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string? Capa { get; set; }
        public string Marcador { get; set; } = "";
        public DateTime? DataPublicacao { get; set; }
    }
}