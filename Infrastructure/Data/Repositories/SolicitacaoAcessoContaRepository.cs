using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories
{
    public class SolicitacaoAcessoContaRepository : BaseRepository<SolicitacaoAcessoConta>, ISolicitacaoAcessoContaRepository
    {
        public SolicitacaoAcessoContaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SolicitacaoAcessoConta>> GetByProprietarioIdAsync(Guid proprietarioId)
        {
            return await _context.SolicitacoesAcessoConta
                .Include(s => s.Solicitante)
                .Include(s => s.Conta)
                .Where(s => s.ProprietarioId == proprietarioId && !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<SolicitacaoAcessoConta>> GetBySolicitanteIdAsync(Guid solicitanteId)
        {
            return await _context.SolicitacoesAcessoConta
                .Include(s => s.Proprietario)
                .Include(s => s.Conta)
                .Where(s => s.SolicitanteId == solicitanteId && !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }

        public async Task<SolicitacaoAcessoConta?> GetPendenteBySolicitanteContaAsync(Guid solicitanteId, Guid contaId)
        {
            return await _context.SolicitacoesAcessoConta
                .FirstOrDefaultAsync(s => s.SolicitanteId == solicitanteId && 
                                        s.ContaId == contaId && 
                                        s.Status == StatusSolicitacao.Pendente && 
                                        !s.IsDeleted);
        }

        public async Task<IEnumerable<SolicitacaoAcessoConta>> GetByStatusAsync(StatusSolicitacao status)
        {
            return await _context.SolicitacoesAcessoConta
                .Include(s => s.Solicitante)
                .Include(s => s.Proprietario)
                .Include(s => s.Conta)
                .Where(s => s.Status == status && !s.IsDeleted)
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();
        }
    }
}