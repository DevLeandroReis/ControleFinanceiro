using ControleFinanceiro.Application.DTOs.Lancamento;
using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface ILancamentoService
    {
        Task<IEnumerable<LancamentoDto>> GetAllAsync(Guid usuarioId);
        Task<LancamentoDto?> GetByIdAsync(Guid id, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorCategoriaAsync(Guid categoriaId, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorTipoAsync(TipoLancamento tipo, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorStatusAsync(StatusLancamento status, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> GetLancamentosVencidosAsync(Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> GetLancamentosRecorrentesAsync(Guid usuarioId);
        Task<decimal> GetSaldoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid usuarioId);
        Task<decimal> GetTotalReceitasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid usuarioId);
        Task<decimal> GetTotalDespesasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid usuarioId);
        Task<LancamentoDto> CreateAsync(CreateLancamentoDto createDto, Guid usuarioId);
        Task<LancamentoDto> UpdateAsync(Guid id, UpdateLancamentoDto updateDto, Guid usuarioId);
        Task DeleteAsync(Guid id, Guid usuarioId);
        Task<LancamentoDto> MarcarComoPagoAsync(Guid id, Guid usuarioId, DateTime? dataPagamento = null);
        Task<LancamentoDto> MarcarComoPendenteAsync(Guid id, Guid usuarioId);
        Task<LancamentoDto> CancelarAsync(Guid id, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> CriarLancamentosRecorrentesAsync(CreateLancamentoDto createDto, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> UpdateLancamentoRecorrenteAsync(Guid id, UpdateLancamentoDto updateDto, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> GerarLancamentosFuturosAsync(Guid lancamentoPaiId, Guid usuarioId);
        Task<IEnumerable<LancamentoDto>> GetLancamentosPorContaAsync(Guid contaId, Guid usuarioId);
    }
}