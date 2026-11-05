namespace DreamApi.Models
{
    public class Colecao
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public string Titulo { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Visibilidade { get; set; } = "";
        public DateTime DataCriacao { get; set; }
    }
}