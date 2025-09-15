using ControleFinanceiro.Application.DTOs.Lancamento;
using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
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
        /// Obter todos os lan�amentos
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetAll()
        {
            try
            {
                var lancamentos = await _lancamentoService.GetAllAsync();
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
        public async Task<ActionResult<LancamentoDto>> GetById(Guid id)
        {
            try
            {
                var lancamento = await _lancamentoService.GetByIdAsync(id);
                if (lancamento == null)
                {
                    return NotFound($"Lan�amento com ID '{id}' n�o encontrado");
                }

                return Ok(lancamento);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amento por ID: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por per�odo
        /// </summary>
        [HttpGet("periodo")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim)
        {
            try
            {
                if (dataInicio > dataFim)
                {
                    return BadRequest("A data de in�cio deve ser anterior � data de fim");
                }

                var lancamentos = await _lancamentoService.GetLancamentosPorPeriodoAsync(dataInicio, dataFim);
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por per�odo: {DataInicio} - {DataFim}", dataInicio, dataFim);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por categoria
        /// </summary>
        [HttpGet("categoria/{categoriaId:guid}")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorCategoria(Guid categoriaId)
        {
            try
            {
                var lancamentos = await _lancamentoService.GetLancamentosPorCategoriaAsync(categoriaId);
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por categoria: {CategoriaId}", categoriaId);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por tipo
        /// </summary>
        [HttpGet("tipo/{tipo}")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorTipo(TipoLancamento tipo)
        {
            try
            {
                var lancamentos = await _lancamentoService.GetLancamentosPorTipoAsync(tipo);
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por tipo: {Tipo}", tipo);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos por status
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetPorStatus(StatusLancamento status)
        {
            try
            {
                var lancamentos = await _lancamentoService.GetLancamentosPorStatusAsync(status);
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos por status: {Status}", status);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos vencidos
        /// </summary>
        [HttpGet("vencidos")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetVencidos()
        {
            try
            {
                var lancamentos = await _lancamentoService.GetLancamentosVencidosAsync();
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos vencidos");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter lan�amentos recorrentes
        /// </summary>
        [HttpGet("recorrentes")]
        [ProducesResponseType(typeof(IEnumerable<LancamentoDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<LancamentoDto>>> GetRecorrentes()
        {
            try
            {
                var lancamentos = await _lancamentoService.GetLancamentosRecorrentesAsync();
                return Ok(lancamentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter lan�amentos recorrentes");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter saldo por per�odo
        /// </summary>
        [HttpGet("saldo")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<decimal>> GetSaldoPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim)
        {
            try
            {
                if (dataInicio > dataFim)
                {
                    return BadRequest("A data de in�cio deve ser anterior � data de fim");
                }

                var saldo = await _lancamentoService.GetSaldoPorPeriodoAsync(dataInicio, dataFim);
                return Ok(saldo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter saldo por per�odo: {DataInicio} - {DataFim}", dataInicio, dataFim);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter total de receitas por per�odo
        /// </summary>
        [HttpGet("receitas/total")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<decimal>> GetTotalReceitasPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim)
        {
            try
            {
                if (dataInicio > dataFim)
                {
                    return BadRequest("A data de in�cio deve ser anterior � data de fim");
                }

                var total = await _lancamentoService.GetTotalReceitasPorPeriodoAsync(dataInicio, dataFim);
                return Ok(total);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter total de receitas por per�odo: {DataInicio} - {DataFim}", dataInicio, dataFim);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter total de despesas por per�odo
        /// </summary>
        [HttpGet("despesas/total")]
        [ProducesResponseType(typeof(decimal), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<decimal>> GetTotalDespesasPorPeriodo(
            [FromQuery] DateTime dataInicio, 
            [FromQuery] DateTime dataFim)
        {
            try
            {
                if (dataInicio > dataFim)
                {
                    return BadRequest("A data de in�cio deve ser anterior � data de fim");
                }

                var total = await _lancamentoService.GetTotalDespesasPorPeriodoAsync(dataInicio, dataFim);
                return Ok(total);
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
        public async Task<ActionResult<LancamentoDto>> Create([FromBody] CreateLancamentoDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var lancamento = await _lancamentoService.CreateAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = lancamento.Id }, lancamento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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

                var lancamentos = await _lancamentoService.CriarLancamentosRecorrentesAsync(createDto);
                return Created("", lancamentos);
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
        public async Task<ActionResult<LancamentoDto>> Update(Guid id, [FromBody] UpdateLancamentoDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var lancamento = await _lancamentoService.UpdateAsync(id, updateDto);
                return Ok(lancamento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar lan�amento: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Excluir um lan�amento
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _lancamentoService.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<ActionResult<LancamentoDto>> MarcarComoPago(Guid id, [FromBody] DateTime? dataPagamento = null)
        {
            try
            {
                var lancamento = await _lancamentoService.MarcarComoPagoAsync(id, dataPagamento);
                return Ok(lancamento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<ActionResult<LancamentoDto>> MarcarComoPendente(Guid id)
        {
            try
            {
                var lancamento = await _lancamentoService.MarcarComoPendenteAsync(id);
                return Ok(lancamento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
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
        public async Task<ActionResult<LancamentoDto>> Cancelar(Guid id)
        {
            try
            {
                var lancamento = await _lancamentoService.CancelarAsync(id);
                return Ok(lancamento);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao cancelar lan�amento: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}