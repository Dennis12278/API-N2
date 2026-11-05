using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistoricoLeituraController : ControllerBase
    {
        private HistoricoLeituraService historicoLeituraService;

        public HistoricoLeituraController(HistoricoLeituraService historicoLeituraService)
        {
            this.historicoLeituraService = historicoLeituraService;
        }

        [HttpGet]
        public List<HistoricoLeituraDto> Listar([FromQuery] int idUsuario)
        {
            return historicoLeituraService.ListarDoUsuario(idUsuario);
        }

        [HttpPost("Registrar")]
        public IActionResult Registrar([FromQuery] int idUsuario, [FromQuery] int idDocumento)
        {
            historicoLeituraService.Registrar(idUsuario, idDocumento);

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            historicoLeituraService.Excluir(id);

            return Ok();
        }
    }
}