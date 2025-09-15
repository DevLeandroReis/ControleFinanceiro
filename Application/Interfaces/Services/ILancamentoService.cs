using ControleFinanceiro.Application.DTOs.Lancamento;
using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface ILancamentoService
    {
        Task<IEnumerable<LancamentoDto>> GetAllAsync();
        Task<LancamentoDto?> GetByIdAsync(Guid id);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorCategoriaAsync(Guid categoriaId);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorTipoAsync(TipoLancamento tipo);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorStatusAsync(StatusLancamento status);
        Task<IEnumerable<LancamentoDto>> GetLancamentosVencidosAsync();
        Task<IEnumerable<LancamentoDto>> GetLancamentosRecorrentesAsync();
        Task<decimal> GetSaldoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<decimal> GetTotalReceitasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<decimal> GetTotalDespesasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<LancamentoDto> CreateAsync(CreateLancamentoDto createDto);
        Task<LancamentoDto> UpdateAsync(Guid id, UpdateLancamentoDto updateDto);
        Task DeleteAsync(Guid id);
        Task<LancamentoDto> MarcarComoPagoAsync(Guid id, DateTime? dataPagamento = null);
        Task<LancamentoDto> MarcarComoPendenteAsync(Guid id);
        Task<LancamentoDto> CancelarAsync(Guid id);
        Task<IEnumerable<LancamentoDto>> CriarLancamentosRecorrentesAsync(CreateLancamentoDto createDto);
    }
}