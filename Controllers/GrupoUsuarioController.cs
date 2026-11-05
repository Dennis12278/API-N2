using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrupoUsuarioController : ControllerBase
    {
        private GrupoUsuarioService grupoUsuarioService;

        public GrupoUsuarioController(GrupoUsuarioService grupoUsuarioService)
        {
            this.grupoUsuarioService = grupoUsuarioService;
        }

        [HttpGet]
        public List<GrupoUsuario> Listar()
        {
            return grupoUsuarioService.Listar();
        }

        [HttpGet("Seguidos")]
        public List<GrupoSeguidoDto> ListarSeguidos([FromQuery] int idUsuario)
        {
            return grupoUsuarioService.ListarSeguidos(idUsuario);
        }

        [HttpPost]
        public void Adicionar(GrupoUsuario grupoUsuario)
        {
            grupoUsuarioService.Adicionar(grupoUsuario);
        }

        [HttpPut]
        public void Editar(GrupoUsuario grupoUsuario)
        {
            grupoUsuarioService.Editar(grupoUsuario);
        }

        [HttpDelete("{id}")]
        public void Excluir(int id)
        {
            grupoUsuarioService.Excluir(id);
        }
    }
}