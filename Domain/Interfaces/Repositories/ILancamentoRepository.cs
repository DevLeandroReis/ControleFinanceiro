using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    public interface ILancamentoRepository : IBaseRepository<Lancamento>
    {
        Task<IEnumerable<Lancamento>> GetLancamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Lancamento>> GetLancamentosPorCategoriaAsync(Guid categoriaId);
        Task<IEnumerable<Lancamento>> GetLancamentosPorContaAsync(Guid contaId);
        Task<IEnumerable<Lancamento>> GetLancamentosPorTipoAsync(TipoLancamento tipo);
        Task<IEnumerable<Lancamento>> GetLancamentosPorStatusAsync(StatusLancamento status);
        Task<IEnumerable<Lancamento>> GetLancamentosVencidosAsync();
        Task<IEnumerable<Lancamento>> GetLancamentosRecorrentesAsync();
        Task<decimal> GetSaldoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<decimal> GetTotalReceitasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<decimal> GetTotalDespesasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<Lancamento>> GetLancamentosComCategoriaAsync();
        Task<IEnumerable<Lancamento>> GetLancamentosFilhosAsync(Guid lancamentoPaiId);
        Task<IEnumerable<Lancamento>> GetLancamentosFilhosFuturosAsync(Guid lancamentoPaiId);
        Task<Lancamento?> GetByIdWithRelacionamentosAsync(Guid id);
    }
}