using DreamApi.Models;
using DreamApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DreamApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly AuthService authService;
        private readonly TokenService tokenService;

        public AuthController(AuthService authService, TokenService tokenService)
        {
            this.authService = authService;
            this.tokenService = tokenService;
        }

        [HttpPost("Cadastrar")]
        public IActionResult Cadastrar([FromBody] CadastroDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Nome))
            {
                return BadRequest("Informe o nome.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email) || !dto.Email.Contains('@'))
            {
                return BadRequest("Informe um e-mail válido.");
            }

            if (string.IsNullOrWhiteSpace(dto.Senha) || dto.Senha.Length < 6)
            {
                return BadRequest("A senha precisa ter pelo menos 6 caracteres.");
            }

            if (authService.EmailJaExiste(dto.Email))
            {
                return Conflict("Já existe uma conta com este e-mail.");
            }

            Usuario usuario = authService.Cadastrar(dto);

            return Ok(new
            {
                id = usuario.Id,
                nome = usuario.Nome,
                email = usuario.Email,
                token = tokenService.Gerar(usuario)
            });
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginDto dto)
        {
            Usuario? usuario = authService.Validar(dto);

            if (usuario == null)
            {
                // mesma mensagem para e-mail errado e senha errada
                return Unauthorized("E-mail ou senha inválidos.");
            }

            return Ok(new
            {
                id = usuario.Id,
                nome = usuario.Nome,
                email = usuario.Email,
                token = tokenService.Gerar(usuario)
            });
        }
    }
}