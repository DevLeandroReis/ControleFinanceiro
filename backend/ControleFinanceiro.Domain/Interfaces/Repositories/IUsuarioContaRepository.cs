using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    public interface IUsuarioContaRepository : IBaseRepository<UsuarioConta>
    {
        Task<UsuarioConta?> GetByUsuarioContaAsync(Guid usuarioId, Guid contaId);
        Task<IEnumerable<UsuarioConta>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<IEnumerable<UsuarioConta>> GetByContaIdAsync(Guid contaId);
        Task<bool> UsuarioJaVinculadoContaAsync(Guid usuarioId, Guid contaId);
    }
}