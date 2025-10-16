namespace ControleFinanceiro.Application.DTOs.Usuario
{
    public class UsuarioDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmado { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}