using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories
{
    public class UsuarioContaRepository : BaseRepository<UsuarioConta>, IUsuarioContaRepository
    {
        public UsuarioContaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<UsuarioConta?> GetByUsuarioContaAsync(Guid usuarioId, Guid contaId)
        {
            return await _context.UsuarioContas
                .FirstOrDefaultAsync(uc => uc.UsuarioId == usuarioId && uc.ContaId == contaId && !uc.IsDeleted);
        }

        public async Task<IEnumerable<UsuarioConta>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.UsuarioContas
                .Include(uc => uc.Conta)
                .Where(uc => uc.UsuarioId == usuarioId && !uc.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<UsuarioConta>> GetByContaIdAsync(Guid contaId)
        {
            return await _context.UsuarioContas
                .Include(uc => uc.Usuario)
                .Where(uc => uc.ContaId == contaId && !uc.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> UsuarioJaVinculadoContaAsync(Guid usuarioId, Guid contaId)
        {
            return await _context.UsuarioContas
                .AnyAsync(uc => uc.UsuarioId == usuarioId && uc.ContaId == contaId && !uc.IsDeleted);
        }
    }
}