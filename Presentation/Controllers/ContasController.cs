using ControleFinanceiro.Application.DTOs.Conta;
using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class ContasController : ControllerBase
    {
        private readonly IContaService _contaService;
        private readonly ILogger<ContasController> _logger;

        public ContasController(IContaService contaService, ILogger<ContasController> logger)
        {
            _contaService = contaService;
            _logger = logger;
        }

        /// <summary>
        /// Criar nova conta
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ContaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContaDto>> Create([FromBody] CreateContaDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuarioId = User.GetUserId();
                var conta = await _contaService.CreateAsync(createDto, usuarioId);
                return CreatedAtAction(nameof(GetById), new { id = conta.Id }, conta);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar conta");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter conta por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(ContaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ContaDto>> GetById(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var conta = await _contaService.GetByIdAsync(id, usuarioId);
                
                if (conta == null)
                {
                    return NotFound($"Conta com ID '{id}' n�o encontrada");
                }

                return Ok(conta);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter conta por ID: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter contas do usu�rio autenticado
        /// </summary>
        [HttpGet("minhas")]
        [ProducesResponseType(typeof(IEnumerable<ContaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContaDto>>> GetMinhasContas()
        {
            try
            {
                var usuarioId = User.GetUserId();
                var contas = await _contaService.GetByUsuarioIdAsync(usuarioId);
                return Ok(contas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter contas do usu�rio");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualizar conta
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ContaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ContaDto>> Update(Guid id, [FromBody] UpdateContaDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuarioId = User.GetUserId();
                var conta = await _contaService.UpdateAsync(id, updateDto, usuarioId);
                return Ok(conta);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar conta: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Excluir conta (apenas propriet�rio)
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var sucesso = await _contaService.DeleteAsync(id, usuarioId);
                
                if (!sucesso)
                {
                    return NotFound($"Conta com ID '{id}' n�o encontrada");
                }

                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir conta: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Ativar conta (apenas propriet�rio)
        /// </summary>
        [HttpPatch("{id:guid}/ativar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Ativar(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var sucesso = await _contaService.AtivarAsync(id, usuarioId);
                
                if (!sucesso)
                {
                    return NotFound($"Conta com ID '{id}' n�o encontrada");
                }

                return Ok(new { message = "Conta ativada com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao ativar conta: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Desativar conta (apenas propriet�rio)
        /// </summary>
        [HttpPatch("{id:guid}/desativar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Desativar(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var sucesso = await _contaService.DesativarAsync(id, usuarioId);
                
                if (!sucesso)
                {
                    return NotFound($"Conta com ID '{id}' n�o encontrada");
                }

                return Ok(new { message = "Conta desativada com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao desativar conta: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        // Gerenciamento de Usu�rios

        /// <summary>
        /// Solicitar acesso a uma conta
        /// </summary>
        [HttpPost("solicitar-acesso")]
        [ProducesResponseType(typeof(SolicitacaoAcessoContaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SolicitacaoAcessoContaDto>> SolicitarAcesso([FromBody] SolicitarAcessoContaDto solicitarDto)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var solicitacao = await _contaService.SolicitarAcessoAsync(solicitarDto, usuarioId);
                return Created("", solicitacao);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao solicitar acesso � conta");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Aprovar solicita��o de acesso (apenas propriet�rio)
        /// </summary>
        [HttpPost("solicitacoes/{solicitacaoId:guid}/aprovar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> AprovarSolicitacao(Guid solicitacaoId)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var sucesso = await _contaService.AprovarSolicitacaoAsync(solicitacaoId, usuarioId);
                
                if (!sucesso)
                {
                    return NotFound($"Solicita��o com ID '{solicitacaoId}' n�o encontrada");
                }

                return Ok(new { message = "Solicita��o aprovada com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao aprovar solicita��o: {Id}", solicitacaoId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Rejeitar solicita��o de acesso (apenas propriet�rio)
        /// </summary>
        [HttpPost("solicitacoes/{solicitacaoId:guid}/rejeitar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> RejeitarSolicitacao(Guid solicitacaoId)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var sucesso = await _contaService.RejeitarSolicitacaoAsync(solicitacaoId, usuarioId);
                
                if (!sucesso)
                {
                    return NotFound($"Solicita��o com ID '{solicitacaoId}' n�o encontrada");
                }

                return Ok(new { message = "Solicita��o rejeitada com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao rejeitar solicita��o: {Id}", solicitacaoId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cancelar solicita��o de acesso (apenas solicitante)
        /// </summary>
        [HttpPost("solicitacoes/{solicitacaoId:guid}/cancelar")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> CancelarSolicitacao(Guid solicitacaoId)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var sucesso = await _contaService.CancelarSolicitacaoAsync(solicitacaoId, usuarioId);
                
                if (!sucesso)
                {
                    return NotFound($"Solicita��o com ID '{solicitacaoId}' n�o encontrada");
                }

                return Ok(new { message = "Solicita��o cancelada com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cancelar solicita��o: {Id}", solicitacaoId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter solicita��es pendentes recebidas (apenas propriet�rio)
        /// </summary>
        [HttpGet("solicitacoes/recebidas")]
        [ProducesResponseType(typeof(IEnumerable<SolicitacaoAcessoContaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SolicitacaoAcessoContaDto>>> GetSolicitacoesRecebidas()
        {
            try
            {
                var usuarioId = User.GetUserId();
                var solicitacoes = await _contaService.GetSolicitacoesPendentesByProprietarioAsync(usuarioId);
                return Ok(solicitacoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter solicita��es recebidas");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter solicita��es enviadas pelo usu�rio
        /// </summary>
        [HttpGet("solicitacoes/enviadas")]
        [ProducesResponseType(typeof(IEnumerable<SolicitacaoAcessoContaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SolicitacaoAcessoContaDto>>> GetSolicitacoesEnviadas()
        {
            try
            {
                var usuarioId = User.GetUserId();
                var solicitacoes = await _contaService.GetSolicitacoesBySolicitanteAsync(usuarioId);
                return Ok(solicitacoes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter solicita��es enviadas");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        // Gerenciamento de Permiss�es

        /// <summary>
        /// Conceder permiss�o para adicionar usu�rios (apenas propriet�rio)
        /// </summary>
        [HttpPost("{contaId:guid}/usuarios/{usuarioId:guid}/conceder-permissao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> ConcederPermissao(Guid contaId, Guid usuarioId)
        {
            try
            {
                var proprietarioId = User.GetUserId();
                var sucesso = await _contaService.ConcederPermissaoAdicionarUsuariosAsync(contaId, usuarioId, proprietarioId);
                
                if (!sucesso)
                {
                    return NotFound("Conta n�o encontrada");
                }

                return Ok(new { message = "Permiss�o concedida com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao conceder permiss�o");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Remover permiss�o para adicionar usu�rios (apenas propriet�rio)
        /// </summary>
        [HttpPost("{contaId:guid}/usuarios/{usuarioId:guid}/remover-permissao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> RemoverPermissao(Guid contaId, Guid usuarioId)
        {
            try
            {
                var proprietarioId = User.GetUserId();
                var sucesso = await _contaService.RemoverPermissaoAdicionarUsuariosAsync(contaId, usuarioId, proprietarioId);
                
                if (!sucesso)
                {
                    return NotFound("Conta n�o encontrada");
                }

                return Ok(new { message = "Permiss�o removida com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover permiss�o");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Remover usu�rio da conta (apenas propriet�rio)
        /// </summary>
        [HttpDelete("{contaId:guid}/usuarios/{usuarioId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> RemoverUsuario(Guid contaId, Guid usuarioId)
        {
            try
            {
                var proprietarioId = User.GetUserId();
                var sucesso = await _contaService.RemoverUsuarioContaAsync(contaId, usuarioId, proprietarioId);
                
                if (!sucesso)
                {
                    return NotFound("Conta n�o encontrada");
                }

                return Ok(new { message = "Usu�rio removido com sucesso" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao remover usu�rio da conta");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter usu�rios da conta
        /// </summary>
        [HttpGet("{id:guid}/usuarios")]
        [ProducesResponseType(typeof(IEnumerable<UsuarioContaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<UsuarioContaDto>>> GetUsuariosConta(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var usuarios = await _contaService.GetUsuariosByContaIdAsync(id, usuarioId);
                return Ok(usuarios);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter usu�rios da conta: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
