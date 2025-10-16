using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    public interface ICategoriaRepository : IReadOnlyRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasAtivasAsync();
        Task<IEnumerable<Categoria>> GetCategoriasDestacadasAsync();
        Task<Categoria?> GetByNomeAsync(string nome);
    }
}