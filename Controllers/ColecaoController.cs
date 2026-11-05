using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ColecaoController : ControllerBase
    {
        private ColecaoService colecaoService;

        public ColecaoController(ColecaoService colecaoService)
        {
            this.colecaoService = colecaoService;
        }

        [HttpGet]
        public List<Colecao> Listar()
        {
            return colecaoService.Listar();
        }

        [HttpPost]
        public void Adicionar(Colecao colecao)
        {
            colecaoService.Adicionar(colecao);
        }

        [HttpPut]
        public void Editar(Colecao colecao)
        {
            colecaoService.Editar(colecao);
        }

        [HttpDelete("{id}")]
        public void Excluir(int id)
        {
            colecaoService.Excluir(id);
        }
    }
}