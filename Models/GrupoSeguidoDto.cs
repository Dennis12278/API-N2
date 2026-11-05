namespace DreamApi.Models
{
    public class GrupoSeguidoDto
    {
        public int Id { get; set; }
        public int IdGrupo { get; set; }
        public string Nome { get; set; } = "";
        public string Descricao { get; set; } = "";
    }
}