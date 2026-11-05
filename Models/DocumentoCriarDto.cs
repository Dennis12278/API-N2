namespace DreamApi.Models
{
    public class DocumentoCriarDto
    {
        public int IdUsuario { get; set; }
        public int IdTipoDocumento { get; set; }
        public string Titulo { get; set; } = "";
        public string? Conteudo { get; set; }
        public string? Visibilidade { get; set; }
        public IFormFile? Capa { get; set; }
    }
}