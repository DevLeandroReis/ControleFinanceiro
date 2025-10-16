using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Categoria>> GetCategoriasAtivasAsync()
        {
            return await _dbSet
                .Where(x => x.Ativo)
                .OrderBy(x => x.Nome)
                .ToListAsync();
        }

        public async Task<Categoria?> GetByNomeAsync(string nome)
        {
            return await _dbSet
                .FirstOrDefaultAsync(x => x.Nome.ToLower() == nome.ToLower());
        }

        public async Task<bool> NomeJaExisteAsync(string nome, Guid? excludeId = null)
        {
            var query = _dbSet.Where(x => x.Nome.ToLower() == nome.ToLower());
            
            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}