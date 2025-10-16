using ControleFinanceiro.Application.DTOs.Conta;
using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface IContaService
    {
        Task<ContaDto> CreateAsync(CreateContaDto createDto, Guid proprietarioId);
        Task<ContaDto> UpdateAsync(Guid id, UpdateContaDto updateDto, Guid usuarioId);
        Task<bool> DeleteAsync(Guid id, Guid usuarioId);
        Task<ContaDto?> GetByIdAsync(Guid id, Guid usuarioId);
        Task<IEnumerable<ContaDto>> GetByUsuarioIdAsync(Guid usuarioId);
        Task<bool> AtivarAsync(Guid id, Guid usuarioId);
        Task<bool> DesativarAsync(Guid id, Guid usuarioId);
        
        // Gerenciamento de usuários
        Task<SolicitacaoAcessoContaDto> SolicitarAcessoAsync(SolicitarAcessoContaDto solicitarDto, Guid solicitanteId);
        Task<bool> AprovarSolicitacaoAsync(Guid solicitacaoId, Guid proprietarioId);
        Task<bool> RejeitarSolicitacaoAsync(Guid solicitacaoId, Guid proprietarioId);
        Task<bool> CancelarSolicitacaoAsync(Guid solicitacaoId, Guid solicitanteId);
        Task<IEnumerable<SolicitacaoAcessoContaDto>> GetSolicitacoesPendentesByProprietarioAsync(Guid proprietarioId);
        Task<IEnumerable<SolicitacaoAcessoContaDto>> GetSolicitacoesBySolicitanteAsync(Guid solicitanteId);
        
        // Gerenciamento de permissões
        Task<bool> ConcederPermissaoAdicionarUsuariosAsync(Guid contaId, Guid usuarioId, Guid proprietarioId);
        Task<bool> RemoverPermissaoAdicionarUsuariosAsync(Guid contaId, Guid usuarioId, Guid proprietarioId);
        Task<bool> RemoverUsuarioContaAsync(Guid contaId, Guid usuarioId, Guid proprietarioId);
        Task<IEnumerable<UsuarioContaDto>> GetUsuariosByContaIdAsync(Guid contaId, Guid usuarioId);
    }
}