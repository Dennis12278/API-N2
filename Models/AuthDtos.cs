namespace DreamApi.Models
{
    public class CadastroDto
    {
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
        public string Senha { get; set; } = "";
    }

    public class LoginDto
    {
        public string Email { get; set; } = "";
        public string Senha { get; set; } = "";
    }

    // o que a API devolve: nunca a senha nem o hash
    public class UsuarioSessaoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "";
        public string Email { get; set; } = "";
    }
}