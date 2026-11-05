using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MarcadorPublicacaoController : ControllerBase
    {
        private MarcadorPublicacaoService marcadorService;

        public MarcadorPublicacaoController(MarcadorPublicacaoService marcadorService)
        {
            this.marcadorService = marcadorService;
        }

        [HttpGet]
        public IActionResult Obter([FromQuery] int idUsuario, [FromQuery] int idDocumento)
        {
            return Ok(new { marcador = marcadorService.Obter(idUsuario, idDocumento) });
        }

        [HttpPut]
        public IActionResult Definir([FromBody] MarcadorPublicacao dados)
        {
            marcadorService.Definir(dados.IdUsuario, dados.IdDocumento, dados.Marcador);

            return Ok();
        }
    }
}