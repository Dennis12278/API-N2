using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DocumentoController : ControllerBase
    {
        private DocumentoService documentoService;

        public DocumentoController(DocumentoService documentoService)
        {
            this.documentoService = documentoService;
        }

        [HttpGet]
        public List<DocumentoListaDto> Listar(
            [FromQuery] bool? publicado,
            [FromQuery] int? idUsuario)
        {
            return documentoService.Listar(publicado, idUsuario);
        }

        [HttpGet("{id}")]
        public IActionResult Obter(int id)
        {
            Documento? documento = documentoService.Obter(id);

            if (documento == null)
            {
                return NotFound();
            }

            return Ok(documento);
        }

        [HttpPost]
        public async Task<IActionResult> Adicionar([FromForm] DocumentoCriarDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo))
            {
                return BadRequest("Informe o título.");
            }

            string? erroCapa = documentoService.ValidarCapa(dto.Capa);

            if (erroCapa != null)
            {
                return BadRequest(erroCapa);
            }

            Documento documento = await documentoService.Adicionar(dto);

            return Ok(documento);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Atualizar(int id, [FromForm] DocumentoEditarDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Titulo))
            {
                return BadRequest("Informe o título.");
            }

            string? erroCapa = documentoService.ValidarCapa(dto.Capa);

            if (erroCapa != null)
            {
                return BadRequest(erroCapa);
            }

            Documento? documento = await documentoService.Atualizar(id, dto);

            if (documento == null)
            {
                return NotFound();
            }

            return Ok(documento);
        }

        [HttpPut]
        public void Editar(Documento documento)
        {
            documentoService.Editar(documento);
        }

        [HttpPut("Revisar/{id}")]
        public void Revisar(int id)
        {
            documentoService.Revisar(id);
        }

        [HttpPut("Publicar/{id}")]
        public void Publicar(int id)
        {
            documentoService.Publicar(id);
        }

        [HttpDelete("{id}")]
        public void Excluir(int id)
        {
            documentoService.Excluir(id);
        }
    }
}