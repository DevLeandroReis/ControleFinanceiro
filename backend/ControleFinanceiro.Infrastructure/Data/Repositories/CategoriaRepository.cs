using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Interfaces.Repositories;
using ControleFinanceiro.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ControleFinanceiro.Infrastructure.Data.Repositories
{
    public class CategoriaRepository : ReadOnlyRepository<Categoria>, ICategoriaRepository
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

        public async Task<IEnumerable<Categoria>> GetCategoriasDestacadasAsync()
        {
            return await _dbSet
                .Where(x => x.Ativo && x.Destacada)
                .OrderBy(x => x.Nome)
                .ToListAsync();
        }

        public async Task<IEnumerable<Categoria>> GetByNomeAsync(string nome)
        {
            return await _dbSet
                .Where(x => x.Nome.ToLower().Contains(nome.ToLower()))
                .OrderBy(x => x.Nome)
                .ToListAsync();
        }
    }
}