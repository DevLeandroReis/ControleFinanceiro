using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted);
        }

        public async Task<bool> ExisteEmailAsync(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email.ToLower() == email.ToLower() && !u.IsDeleted);
        }

        public async Task<Usuario?> GetByTokenConfirmacaoEmailAsync(string token)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.TokenConfirmacaoEmail == token && !u.IsDeleted);
        }

        public async Task<Usuario?> GetByTokenRecuperacaoSenhaAsync(string token)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.TokenRecuperacaoSenha == token && 
                                       u.TokenRecuperacaoExpira > DateTime.UtcNow && 
                                       !u.IsDeleted);
        }
    }
}