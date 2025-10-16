using System.Security.Claims;

namespace ControleFinanceiro.Presentation.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal principal)
        {
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                throw new UnauthorizedAccessException("Usu�rio n�o autenticado");
            }
            return userId;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.Email)?.Value 
                ?? throw new UnauthorizedAccessException("Email n�o encontrado no token");
        }

        public static string GetUserName(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.Name)?.Value 
                ?? throw new UnauthorizedAccessException("Nome n�o encontrado no token");
        }
    }
}
