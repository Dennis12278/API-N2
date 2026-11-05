using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnotacaoController : ControllerBase
    {
        private AnotacaoService anotacaoService;

        public AnotacaoController(AnotacaoService anotacaoService)
        {
            this.anotacaoService = anotacaoService;
        }

        [HttpGet]
        public List<Anotacao> Listar()
        {
            return anotacaoService.Listar();
        }

        [HttpPost]
        public void Adicionar(Anotacao anotacao)
        {
            anotacaoService.Adicionar(anotacao);
        }

        [HttpPut]
        public void Editar(Anotacao anotacao)
        {
            anotacaoService.Editar(anotacao);
        }

        [HttpDelete("{id}")]
        public void Excluir(int id)
        {
            anotacaoService.Excluir(id);
        }
    }
}