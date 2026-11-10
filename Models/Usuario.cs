namespace DreamApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Senha { get; set; } = "";
        public string? Biografia { get; set; }
        public string? Links { get; set; }
        public string? Avatar { get; set; }
    }
}