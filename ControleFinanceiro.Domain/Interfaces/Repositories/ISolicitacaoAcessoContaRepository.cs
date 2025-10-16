using ControleFinanceiro.Domain.Entities;
using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Domain.Interfaces.Repositories
{
    public interface ISolicitacaoAcessoContaRepository : IBaseRepository<SolicitacaoAcessoConta>
    {
        Task<IEnumerable<SolicitacaoAcessoConta>> GetByProprietarioIdAsync(Guid proprietarioId);
        Task<IEnumerable<SolicitacaoAcessoConta>> GetBySolicitanteIdAsync(Guid solicitanteId);
        Task<SolicitacaoAcessoConta?> GetPendenteBySolicitanteContaAsync(Guid solicitanteId, Guid contaId);
        Task<IEnumerable<SolicitacaoAcessoConta>> GetByStatusAsync(StatusSolicitacao status);
    }
}