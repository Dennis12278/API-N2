namespace DreamApi.Models
{
    public class DocumentoListaDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; } = "";
        public string? Conteudo { get; set; }
        public bool Revisado { get; set; }
        public bool Publicado { get; set; }
        public string? Visibilidade { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string? Capa { get; set; }
    }
}