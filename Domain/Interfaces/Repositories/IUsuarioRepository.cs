using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IBaseRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> ExisteEmailAsync(string email);
        Task<Usuario?> GetByTokenConfirmacaoEmailAsync(string token);
        Task<Usuario?> GetByTokenRecuperacaoSenhaAsync(string token);
        Task<IEnumerable<Usuario>> GetByContaIdAsync(Guid contaId);
    }
}