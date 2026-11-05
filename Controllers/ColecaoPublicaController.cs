using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColecaoPublicaController : ControllerBase
    {
        private ColecaoPublicaService colecaoPublicaService;

        public ColecaoPublicaController(ColecaoPublicaService colecaoPublicaService)
        {
            this.colecaoPublicaService = colecaoPublicaService;
        }

        [HttpGet("Publicas")]
        public List<ColecaoCardDto> Publicas([FromQuery] int idUsuario = 0)
        {
            return colecaoPublicaService.ListarPublicas(idUsuario);
        }

        [HttpGet("Curtidas")]
        public List<ColecaoCardDto> Curtidas([FromQuery] int idUsuario)
        {
            return colecaoPublicaService.ListarCurtidas(idUsuario);
        }

        [HttpGet("Minhas")]
        public List<ColecaoCardDto> Minhas([FromQuery] int idUsuario)
        {
            return colecaoPublicaService.ListarMinhas(idUsuario);
        }

        [HttpGet("{id:int}")]
        public IActionResult Obter(int id, [FromQuery] int idUsuario = 0)
        {
            ColecaoDetalheDto? colecao = colecaoPublicaService.Obter(id, idUsuario);

            if (colecao == null)
            {
                return NotFound();
            }

            return Ok(colecao);
        }

        [HttpPost("Criar")]
        public IActionResult Criar([FromBody] ColecaoSalvarDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo))
            {
                return BadRequest("Informe o título da coleção.");
            }

            int id = colecaoPublicaService.Criar(dto);

            return Ok(new ColecaoCriadaDto { Id = id });
        }

        [HttpPut("{id:int}")]
        public IActionResult Atualizar(int id, [FromBody] ColecaoSalvarDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo))
            {
                return BadRequest("Informe o título da coleção.");
            }

            return colecaoPublicaService.Atualizar(id, dto) ? Ok() : NotFound();
        }

        [HttpPut("{id:int}/Itens")]
        public IActionResult SalvarItens(int id, [FromBody] ColecaoItensDto dto)
        {
            return colecaoPublicaService.SalvarItens(id, dto) ? Ok() : NotFound();
        }

        [HttpPost("{id:int}/Curtir")]
        public IActionResult Curtir(int id, [FromQuery] int idUsuario)
        {
            bool curtiu = colecaoPublicaService.Curtir(id, idUsuario);

            return Ok(new { curtiu });
        }

        [HttpDelete("{id:int}")]
        public IActionResult Excluir(int id, [FromQuery] int idUsuario)
        {
            return colecaoPublicaService.Excluir(id, idUsuario) ? Ok() : NotFound();
        }
    }
}