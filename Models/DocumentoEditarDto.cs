namespace DreamApi.Models
{
    public class DocumentoEditarDto
    {
        public string Titulo { get; set; } = "";
        public string? Conteudo { get; set; }
        public string? Visibilidade { get; set; }
        public IFormFile? Capa { get; set; }
        public bool RemoverCapa { get; set; }
    }
}