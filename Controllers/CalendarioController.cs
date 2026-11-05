using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalendarioController : ControllerBase
    {
        private CalendarioService calendarioService;

        public CalendarioController(CalendarioService calendarioService)
        {
            this.calendarioService = calendarioService;
        }

        [HttpGet]
        public List<Calendario> Listar([FromQuery] int? idUsuario)
        {
            return calendarioService.Listar(idUsuario);
        }

        [HttpPost]
        public IActionResult Adicionar([FromBody] Calendario calendario)
        {
            return Ok(calendarioService.Adicionar(calendario));
        }

        [HttpPut]
        public IActionResult Editar([FromBody] Calendario calendario)
        {
            return calendarioService.Editar(calendario) ? Ok() : NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            return calendarioService.Excluir(id) ? Ok() : NotFound();
        }
    }
}