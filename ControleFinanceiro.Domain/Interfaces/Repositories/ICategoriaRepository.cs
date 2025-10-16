using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    public interface ICategoriaRepository : IReadOnlyRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasAtivasAsync();
        Task<Categoria?> GetByNomeAsync(string nome);
    }
}