using ControleFinanceiro.Application.DTOs.Usuario;

namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface IUsuarioService
    {
        Task<UsuarioDto> CreateAsync(CreateUsuarioDto createDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<bool> RecuperarSenhaAsync(RecuperarSenhaDto recuperarSenhaDto);
        Task<bool> RedefinirSenhaAsync(RedefinirSenhaDto redefinirSenhaDto);
        Task<bool> ConfirmarEmailAsync(string token);
        Task<bool> ReenviarEmailConfirmacaoAsync(ReenviarEmailConfirmacaoDto dto);
        Task<UsuarioDto?> GetByIdAsync(Guid id);
        Task<UsuarioDto?> GetByEmailAsync(string email);
    }
}