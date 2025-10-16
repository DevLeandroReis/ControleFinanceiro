using ControleFinanceiro.Application.DTOs.Categoria;
using ControleFinanceiro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleFinanceiro.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Authorize]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(ICategoriaService categoriaService, ILogger<CategoriasController> logger)
        {
            _categoriaService = categoriaService;
            _logger = logger;
        }

        /// <summary>
        /// Obter todas as categorias
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAll()
        {
            try
            {
                var categorias = await _categoriaService.GetAllAsync();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter todas as categorias");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter apenas categorias ativas
        /// </summary>
        [HttpGet("ativas")]
        [ProducesResponseType(typeof(IEnumerable<CategoriaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> GetAtivas()
        {
            try
            {
                var categorias = await _categoriaService.GetCategoriasAtivasAsync();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categorias ativas");
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter categoria por ID
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDto>> GetById(Guid id)
        {
            try
            {
                var categoria = await _categoriaService.GetByIdAsync(id);
                if (categoria == null)
                {
                    return NotFound($"Categoria com ID '{id}' não encontrada");
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categoria por ID: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Obter categoria por nome
        /// </summary>
        [HttpGet("nome/{nome}")]
        [ProducesResponseType(typeof(CategoriaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoriaDto>> GetByNome(string nome)
        {
            try
            {
                var categoria = await _categoriaService.GetByNomeAsync(nome);
                if (categoria == null)
                {
                    return NotFound($"Categoria com nome '{nome}' não encontrada");
                }

                return Ok(categoria);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter categoria por nome: {Nome}", nome);
                return StatusCode(500, "Erro interno do servidor");
            }
        }

        /// <summary>
        /// Verificar se uma categoria existe
        /// </summary>
        [HttpHead("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Exists(Guid id)
        {
            try
            {
                var exists = await _categoriaService.ExistsAsync(id);
                return exists ? Ok() : NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar existência da categoria: {Id}", id);
                return StatusCode(500, "Erro interno do servidor");
            }
        }
    }
}
