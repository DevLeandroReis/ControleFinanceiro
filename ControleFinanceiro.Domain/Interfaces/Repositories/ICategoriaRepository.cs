using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    public interface ICategoriaRepository : IBaseRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> GetCategoriasAtivasAsync();
        Task<Categoria?> GetByNomeAsync(string nome);
        Task<bool> NomeJaExisteAsync(string nome, Guid? excludeId = null);
    }
}