using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CompartilhamentoController : ControllerBase
    {
        private CompartilhamentoService compartilhamentoService;

        public CompartilhamentoController(CompartilhamentoService compartilhamentoService)
        {
            this.compartilhamentoService = compartilhamentoService;
        }

        [HttpGet]
        public List<Compartilhamento> Listar()
        {
            return compartilhamentoService.Listar();
        }

        [HttpPost]
        public void Adicionar(Compartilhamento compartilhamento)
        {
            compartilhamentoService.Adicionar(compartilhamento);
        }

        [HttpPut]
        public void Editar(Compartilhamento compartilhamento)
        {
            compartilhamentoService.Editar(compartilhamento);
        }

        [HttpDelete("{id}")]
        public void Excluir(int id)
        {
            compartilhamentoService.Excluir(id);
        }
    }
}