namespace DreamApi.Models
{
    // perfil público: nunca mostra o e-mail
    public class PerfilPublicoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Biografia { get; set; } = "";
        public string Links { get; set; } = "";
        public string? Avatar { get; set; }
        public int Titulos { get; set; }
        public int Publicacoes { get; set; }
        public int Colecoes { get; set; }
        public int Grupos { get; set; }
    }

    // perfil do próprio usuário (para editar)
    public class PerfilMeuDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Biografia { get; set; } = "";
        public string Links { get; set; } = "";
        public string? Avatar { get; set; }
        public int Titulos { get; set; }
        public int Publicacoes { get; set; }
        public int Colecoes { get; set; }
        public int Grupos { get; set; }
    }

    public class PerfilSalvarDto
    {
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string? Biografia { get; set; }
        public string? Links { get; set; }
        public string? Avatar { get; set; }
    }
}