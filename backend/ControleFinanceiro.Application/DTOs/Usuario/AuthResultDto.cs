namespace ControleFinanceiro.Application.DTOs.Usuario
{
    public class AuthResultDto
    {
        public string Token { get; set; } = string.Empty;
        public UsuarioDto Usuario { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
}