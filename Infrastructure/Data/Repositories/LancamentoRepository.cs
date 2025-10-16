using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories
{
    public class LancamentoRepository : BaseRepository<Lancamento>, ILancamentoRepository
    {
        public LancamentoRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.DataVencimento >= dataInicio && x.DataVencimento <= dataFim)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosPorCategoriaAsync(Guid categoriaId)
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.CategoriaId == categoriaId)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosPorContaAsync(Guid contaId)
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.ContaId == contaId)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosPorTipoAsync(TipoLancamento tipo)
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.Tipo == tipo)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosPorStatusAsync(StatusLancamento status)
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.Status == status)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosVencidosAsync()
        {
            var hoje = DateTime.Today;
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.Status == StatusLancamento.Pendente && x.DataVencimento < hoje)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosRecorrentesAsync()
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.EhRecorrente)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<decimal> GetSaldoPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            var receitas = await GetTotalReceitasPorPeriodoAsync(dataInicio, dataFim);
            var despesas = await GetTotalDespesasPorPeriodoAsync(dataInicio, dataFim);
            return receitas - despesas;
        }

        public async Task<decimal> GetTotalReceitasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _dbSet
                .Where(x => x.Tipo == TipoLancamento.Receita 
                           && x.Status == StatusLancamento.Pago
                           && x.DataPagamento >= dataInicio 
                           && x.DataPagamento <= dataFim)
                .SumAsync(x => x.Valor);
        }

        public async Task<decimal> GetTotalDespesasPorPeriodoAsync(DateTime dataInicio, DateTime dataFim)
        {
            return await _dbSet
                .Where(x => x.Tipo == TipoLancamento.Despesa 
                           && x.Status == StatusLancamento.Pago
                           && x.DataPagamento >= dataInicio 
                           && x.DataPagamento <= dataFim)
                .SumAsync(x => x.Valor);
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosComCategoriaAsync()
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosFilhosAsync(Guid lancamentoPaiId)
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.LancamentoPaiId == lancamentoPaiId)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lancamento>> GetLancamentosFilhosFuturosAsync(Guid lancamentoPaiId)
        {
            var hoje = DateTime.Today;
            return await _dbSet
                .Include(x => x.Categoria)
                .Where(x => x.LancamentoPaiId == lancamentoPaiId && x.DataVencimento > hoje)
                .OrderBy(x => x.DataVencimento)
                .ToListAsync();
        }

        public async Task<Lancamento?> GetByIdWithRelacionamentosAsync(Guid id)
        {
            return await _dbSet
                .Include(x => x.Categoria)
                .Include(x => x.LancamentosFilhos)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}