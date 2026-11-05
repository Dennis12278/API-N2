namespace DreamApi.Models
{
    // card da Home
    public class ColecaoCardDto
    {
        public int Id { get; set; }
        public int IdUsuario { get; set; }
        public bool Curtiu { get; set; }
        public string Titulo { get; set; } = "";
        public string Visibilidade { get; set; } = "";
        public string NomeUsuario { get; set; } = "";
        public List<string?> Capas { get; set; } = new();
        public int QuantidadeTitulos { get; set; }
        public DateTime DataCriacao { get; set; }
        public int Curtidas { get; set; }
    }

    // um título dentro da coleção
    public class ColecaoItemDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string? Capa { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public int Ordem { get; set; }
    }

    // página da coleção
    public class ColecaoDetalheDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = "";
        public string Descricao { get; set; } = "";
        public string Visibilidade { get; set; } = "";
        public int IdUsuario { get; set; }
        public string NomeUsuario { get; set; } = "";
        public DateTime DataCriacao { get; set; }
        public int Curtidas { get; set; }
        public bool Curtiu { get; set; }
        public List<ColecaoItemDto> Itens { get; set; } = new();
    }

    // criar ou editar a coleção
    public class ColecaoSalvarDto
    {
        public int IdUsuario { get; set; }
        public string Titulo { get; set; } = "";
        public string? Descricao { get; set; }
        public string? Visibilidade { get; set; }
    }

    // definir a lista de títulos e a ordem
    public class ColecaoItensDto
    {
        public int IdUsuario { get; set; }
        public List<int> Ids { get; set; } = new();
    }

    public class ColecaoCriadaDto
    {
        public int Id { get; set; }
    }
}