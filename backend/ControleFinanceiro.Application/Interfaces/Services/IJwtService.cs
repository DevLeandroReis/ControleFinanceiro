using ControleFinanceiro.Domain.Entities;

namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface IJwtService
    {
        string GenerateToken(Usuario usuario);
        bool ValidateToken(string token);
        Guid? GetUserIdFromToken(string token);
    }
}