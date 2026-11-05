using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColecaoDocumentoController : ControllerBase
    {
        private ColecaoDocumentoService colecaoDocumentoService;

        public ColecaoDocumentoController(ColecaoDocumentoService colecaoDocumentoService)
        {
            this.colecaoDocumentoService = colecaoDocumentoService;
        }

        [HttpGet("Colecoes")]
        public List<ColecaoOpcaoDto> Colecoes([FromQuery] int idUsuario, [FromQuery] int idDocumento)
        {
            return colecaoDocumentoService.ListarComMarcacao(idUsuario, idDocumento);
        }

        [HttpPost("Adicionar")]
        public IActionResult Adicionar([FromQuery] int idColecao, [FromQuery] int idDocumento)
        {
            colecaoDocumentoService.Adicionar(idColecao, idDocumento);

            return Ok();
        }

        [HttpDelete("Remover")]
        public IActionResult Remover([FromQuery] int idColecao, [FromQuery] int idDocumento)
        {
            colecaoDocumentoService.Remover(idColecao, idDocumento);

            return Ok();
        }
    }
}