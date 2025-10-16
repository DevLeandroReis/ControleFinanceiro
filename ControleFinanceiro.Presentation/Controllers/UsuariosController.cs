using ControleFinanceiro.Application.DTOs.Usuario;
using ControleFinanceiro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly ILogger<UsuariosController> _logger;

        public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
        {
            _usuarioService = usuarioService;
            _logger = logger;
        }

        /// <summary>
        /// Registrar novo usu�rio
        /// </summary>
        [HttpPost("registrar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UsuarioDto>> Registrar([FromBody] CreateUsuarioDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuario = await _usuarioService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = usuario.Id }, usuario);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao registrar usu�rio");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Login de usu�rio
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _usuarioService.LoginAsync(loginDto);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao fazer login");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Recuperar senha
        /// </summary>
        [HttpPost("recuperar-senha")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RecuperarSenha([FromBody] RecuperarSenhaDto recuperarSenhaDto)
        {
            try
            {
                await _usuarioService.RecuperarSenhaAsync(recuperarSenhaDto);
                return Ok(new { message = "Se o email existir, voc� receber� instru��es para recupera��o de senha" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao recuperar senha");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Redefinir senha
        /// </summary>
        [HttpPost("redefinir-senha")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> RedefinirSenha([FromBody] RedefinirSenhaDto redefinirSenhaDto)
        {
            try
            {
                await _usuarioService.RedefinirSenhaAsync(redefinirSenhaDto);
                return Ok(new { message = "Senha redefinida com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao redefinir senha");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Confirmar email
        /// </summary>
        [HttpGet("confirmar-email")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ConfirmarEmail([FromQuery] string token)
        {
            try
            {
                var sucesso = await _usuarioService.ConfirmarEmailAsync(token);
                if (!sucesso)
                {
                    return BadRequest("Token inv�lido");
                }
                return Ok(new { message = "Email confirmado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao confirmar email");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter usu�rio por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioDto>> GetById(Guid id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);
                if (usuario == null)
                {
                    return NotFound($"Usu�rio com ID '{id}' n�o encontrado");
                }

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter usu�rio por ID: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter todos os usu�rios
        /// </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetAll()
        {
            try
            {
                var usuarios = await _usuarioService.GetAllAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os usu�rios");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Ativar usu�rio
        /// </summary>
        [HttpPatch("{id:guid}/ativar")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Ativar(Guid id)
        {
            try
            {
                var sucesso = await _usuarioService.AtivarAsync(id);
                if (!sucesso)
                {
                    return NotFound($"Usu�rio com ID '{id}' n�o encontrado");
                }
                return Ok(new { message = "Usu�rio ativado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao ativar usu�rio: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Desativar usu�rio
        /// </summary>
        [HttpPatch("{id:guid}/desativar")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Desativar(Guid id)
        {
            try
            {
                var sucesso = await _usuarioService.DesativarAsync(id);
                if (!sucesso)
                {
                    return NotFound($"Usu�rio com ID '{id}' n�o encontrado");
                }
                return Ok(new { message = "Usu�rio desativado com sucesso" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desativar usu�rio: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
