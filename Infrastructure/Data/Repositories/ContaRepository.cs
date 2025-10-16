using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories
{
    public class ContaRepository : BaseRepository<Conta>, IContaRepository
    {
        public ContaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Conta>> GetByUsuarioIdAsync(Guid usuarioId)
        {
            return await _context.Contas
                .Where(c => (c.ProprietarioId == usuarioId || 
                           c.UsuarioContas.Any(uc => uc.UsuarioId == usuarioId && uc.Ativo)) && 
                           c.Ativa && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Conta>> GetByProprietarioIdAsync(Guid proprietarioId)
        {
            return await _context.Contas
                .Where(c => c.ProprietarioId == proprietarioId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> UsuarioTemAcessoContaAsync(Guid usuarioId, Guid contaId)
        {
            return await _context.Contas
                .AnyAsync(c => c.Id == contaId && 
                             (c.ProprietarioId == usuarioId || 
                              c.UsuarioContas.Any(uc => uc.UsuarioId == usuarioId && uc.Ativo)) && 
                             c.Ativa && !c.IsDeleted);
        }

        public async Task<Conta?> GetWithUsuariosAsync(Guid contaId)
        {
            return await _context.Contas
                .Include(c => c.UsuarioContas)
                    .ThenInclude(uc => uc.Usuario)
                .Include(c => c.Proprietario)
                .FirstOrDefaultAsync(c => c.Id == contaId && !c.IsDeleted);
        }
    }
}