using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TipoDocumentoController : ControllerBase
    {
        private TipoDocumentoService tipoDocumentoService;

        public TipoDocumentoController(TipoDocumentoService tipoDocumentoService)
        {
            this.tipoDocumentoService = tipoDocumentoService;
        }

        [HttpGet]
        public List<TipoDocumento> Listar()
        {
            return tipoDocumentoService.Listar();
        }

        [HttpPost]
        public void Adicionar(TipoDocumento tipo)
        {
            tipoDocumentoService.Adicionar(tipo);
        }

        [HttpPut]
        public void Editar(TipoDocumento tipo)
        {
            tipoDocumentoService.Editar(tipo);
        }

        [HttpDelete("{id}")]
        public void Excluir(int id)
        {
            tipoDocumentoService.Excluir(id);
        }
    }
}