using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificacaoController : ControllerBase
    {
        private NotificacaoService notificacaoService;

        public NotificacaoController(NotificacaoService notificacaoService)
        {
            this.notificacaoService = notificacaoService;
        }

        [HttpGet]
        public List<Notificacao> Listar()
        {
            return notificacaoService.Listar();
        }

        [HttpPost]
        public void Adicionar(Notificacao notificacao)
        {
            notificacaoService.Adicionar(notificacao);
        }

        [HttpPut]
        public void Editar(Notificacao notificacao)
        {
            notificacaoService.Editar(notificacao);
        }

        [HttpPut("Ler/{id}")]
        public void Ler(int id)
        {
            notificacaoService.Ler(id);
        }

        [HttpDelete("{id}")]
        public void Excluir(int id)
        {
            notificacaoService.Excluir(id);
        }
    }
}