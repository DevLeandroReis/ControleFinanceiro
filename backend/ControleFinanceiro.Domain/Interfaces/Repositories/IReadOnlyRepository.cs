using ControleFinanceiro.Domain.Entities;
using System.Linq.Expressions;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Interface para repositórios somente leitura
    /// </summary>
    public interface IReadOnlyRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> FindFirstAsync(Expression<Func<TEntity, bool>> predicate);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> predicate);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
    }
}
