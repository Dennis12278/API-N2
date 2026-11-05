using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GrupoController : ControllerBase
    {
        private GrupoService grupoService;

        public GrupoController(GrupoService grupoService)
        {
            this.grupoService = grupoService;
        }

        [HttpGet]
        public List<Grupo> Listar()
        {
            return grupoService.Listar();
        }

        [HttpPost]
        public void Adicionar(Grupo grupo)
        {
            grupoService.Adicionar(grupo);
        }

        [HttpPut]
        public void Editar(Grupo grupo)
        {
            grupoService.Editar(grupo);
        }

        [HttpDelete("{id}")]
        public void Excluir(int id)
        {
            grupoService.Excluir(id);
        }
    }
}