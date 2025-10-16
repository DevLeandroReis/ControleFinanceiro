using ControleFinanceiro.Application.DTOs.Lancamento;
using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Presentation.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class LancamentosController : ControllerBase
    {
        private readonly ILancamentoService _lancamentoService;
        private readonly ILogger<LancamentosController> _logger;

        public LancamentosController(ILancamentoService lancamentoService, ILogger<LancamentosController> logger)
        {
            _lancamentoService = lancamentoService;
            _logger = logger;
        }

        /// <summary>
        /// Obter todos os lan�amentos do usu�rio
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetAll()
        {
            try
            {
                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GetAllAsync(usuarioId);
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todos os lan�amentos");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amento por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(LancamentoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LancamentoDto>> GetById(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var lancamento = await _lancamentoService.GetByIdAsync(id, usuarioId);
                if (lancamento == null)
                {
                    return NotFound($"Lan�amento com ID '{id}' n�o encontrado");
                }

                return Ok(lancamento);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amento por ID: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por per�odo e contas
        /// </summary>
        [HttpGet("periodo")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim,
            [FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (dataInicio > dataFim)
                {
                    return BadRequest("A data de in�cio deve ser anterior � data de fim");
                }

                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GetLancamentosPorPeriodoAsync(dataInicio, dataFim, contaIds, usuarioId);
                return Ok(lancamentos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por per�odo: {DataInicio} - {DataFim}", dataInicio, dataFim);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por categoria e contas
        /// </summary>
        [HttpGet("categoria/{categoriaId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorCategoria(
            Guid categoriaId,
            [FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GetLancamentosPorCategoriaAsync(categoriaId, contaIds, usuarioId);
                return Ok(lancamentos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por categoria: {CategoriaId}", categoriaId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por conta
        /// </summary>
        [HttpGet("conta/{contaId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorConta(Guid contaId)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GetLancamentosPorContaAsync(contaId, usuarioId);
                return Ok(lancamentos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por conta: {ContaId}", contaId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por tipo e contas
        /// </summary>
        [HttpGet("tipo/{tipo}")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorTipo(
            TipoLancamento tipo,
            [FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GetLancamentosPorTipoAsync(tipo, contaIds, usuarioId);
                return Ok(lancamentos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por tipo: {Tipo}", tipo);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por status e contas
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorStatus(
            StatusLancamento status,
            [FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GetLancamentosPorStatusAsync(status, contaIds, usuarioId);
                return Ok(lancamentos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por status: {Status}", status);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos vencidos por contas
        /// </summary>
        [HttpGet("vencidos")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetVencidos([FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GetLancamentosVencidosAsync(contaIds, usuarioId);
                return Ok(lancamentos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos vencidos");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos recorrentes por contas
        /// </summary>
        [HttpGet("recorrentes")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetRecorrentes([FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GetLancamentosRecorrentesAsync(contaIds, usuarioId);
                return Ok(lancamentos);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos recorrentes");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter saldo por per�odo e contas
        /// </summary>
        [HttpGet("saldo")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<decimal>> GetSaldoPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim,
            [FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (dataInicio > dataFim)
                {
                    return BadRequest("A data de in�cio deve ser anterior � data de fim");
                }

                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var saldo = await _lancamentoService.GetSaldoPorPeriodoAsync(dataInicio, dataFim, contaIds, usuarioId);
                return Ok(saldo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter saldo por per�odo: {DataInicio} - {DataFim}", dataInicio, dataFim);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter total de receitas por per�odo e contas
        /// </summary>
        [HttpGet("receitas/total")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<decimal>> GetTotalReceitasPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim,
            [FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (dataInicio > dataFim)
                {
                    return BadRequest("A data de in�cio deve ser anterior � data de fim");
                }

                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var total = await _lancamentoService.GetTotalReceitasPorPeriodoAsync(dataInicio, dataFim, contaIds, usuarioId);
                return Ok(total);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter total de receitas por per�odo: {DataInicio} - {DataFim}", dataInicio, dataFim);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter total de despesas por per�odo e contas
        /// </summary>
        [HttpGet("despesas/total")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<decimal>> GetTotalDespesasPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim,
            [FromQuery] List<Guid> contaIds)
        {
            try
            {
                if (dataInicio > dataFim)
                {
                    return BadRequest("A data de in�cio deve ser anterior � data de fim");
                }

                if (contaIds == null || !contaIds.Any())
                {
                    return BadRequest("� necess�rio informar pelo menos uma conta");
                }

                var usuarioId = User.GetUserId();
                var total = await _lancamentoService.GetTotalDespesasPorPeriodoAsync(dataInicio, dataFim, contaIds, usuarioId);
                return Ok(total);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter total de despesas por per�odo: {DataInicio} - {DataFim}", dataInicio, dataFim);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Criar um novo lan�amento
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(LancamentoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LancamentoDto>> Create([FromBody] CreateLancamentoDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuarioId = User.GetUserId();
                var lancamento = await _lancamentoService.CreateAsync(createDto, usuarioId);
                return CreatedAtAction(nameof(GetById), new { id = lancamento.Id }, lancamento);
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
                _logger.LogError(ex, "Erro ao criar lan�amento");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Criar lan�amentos recorrentes
        /// </summary>
        [HttpPost("recorrentes")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> CreateRecorrentes([FromBody] CreateLancamentoDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!createDto.EhRecorrente)
                {
                    return BadRequest("Para criar lan�amentos recorrentes, o campo EhRecorrente deve ser true");
                }

                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.CriarLancamentosRecorrentesAsync(createDto, usuarioId);
                return Created("", lancamentos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                _logger.LogError(ex, "Erro ao criar lan�amentos recorrentes");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualizar um lan�amento
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(LancamentoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LancamentoDto>> Update(Guid id, [FromBody] UpdateLancamentoDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuarioId = User.GetUserId();
                var lancamento = await _lancamentoService.UpdateAsync(id, updateDto, usuarioId);
                return Ok(lancamento);
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
                _logger.LogError(ex, "Erro ao atualizar lan�amento: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Atualizar lan�amento recorrente (pai e filhos futuros)
        /// </summary>
        [HttpPut("{id:guid}/recorrente")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> UpdateRecorrente(Guid id, [FromBody] UpdateLancamentoDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.UpdateLancamentoRecorrenteAsync(id, updateDto, usuarioId);
                return Ok(lancamentos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                _logger.LogError(ex, "Erro ao atualizar lan�amento recorrente: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Gerar lan�amentos futuros para um lan�amento pai
        /// </summary>
        [HttpPost("{id:guid}/gerar-futuros")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GerarLancamentosFuturos(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var lancamentos = await _lancamentoService.GerarLancamentosFuturosAsync(id, usuarioId);
                return Created("", lancamentos);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
                _logger.LogError(ex, "Erro ao gerar lan�amentos futuros: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Excluir um lan�amento
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
                await _lancamentoService.DeleteAsync(id, usuarioId);
                return NoContent();
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
                _logger.LogError(ex, "Erro ao excluir lan�amento: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Marcar lan�amento como pago
        /// </summary>
        [HttpPatch("{id:guid}/pagar")]
        [ProducesResponseType(typeof(LancamentoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LancamentoDto>> MarcarComoPago(Guid id, [FromBody] DateTime? dataPagamento = null)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var lancamento = await _lancamentoService.MarcarComoPagoAsync(id, usuarioId, dataPagamento);
                return Ok(lancamento);
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
                _logger.LogError(ex, "Erro ao marcar lan�amento como pago: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Marcar lan�amento como pendente
        /// </summary>
        [HttpPatch("{id:guid}/pendente")]
        [ProducesResponseType(typeof(LancamentoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LancamentoDto>> MarcarComoPendente(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var lancamento = await _lancamentoService.MarcarComoPendenteAsync(id, usuarioId);
                return Ok(lancamento);
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
                _logger.LogError(ex, "Erro ao marcar lan�amento como pendente: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Cancelar lan�amento
        /// </summary>
        [HttpPatch("{id:guid}/cancelar")]
        [ProducesResponseType(typeof(LancamentoDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LancamentoDto>> Cancelar(Guid id)
        {
            try
            {
                var usuarioId = User.GetUserId();
                var lancamento = await _lancamentoService.CancelarAsync(id, usuarioId);
                return Ok(lancamento);
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
                _logger.LogError(ex, "Erro ao cancelar lan�amento: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}