using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    public interface IContaRepository : IBaseRepository<Conta>
    {
        Task<IEnumerable<Conta>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<IEnumerable<Conta>> GetByProprietarioIdAsync(Guid proprietarioId);
        Task<bool> UsuarioTemAcessoContaAsync(Guid usuarioId, Guid contaId);
        Task<Conta?> GetWithUsuariosAsync(Guid contaId);
    }
}