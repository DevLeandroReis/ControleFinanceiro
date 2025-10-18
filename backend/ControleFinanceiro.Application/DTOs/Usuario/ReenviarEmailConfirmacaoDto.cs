using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Usuario
{
    public class ReenviarEmailConfirmacaoDto
    {
        [Required(ErrorMessage = "O email � obrigat�rio")]
        [EmailAddress(ErrorMessage = "Email inv�lido")]
        public string Email { get; set; } = string.Empty;
    }
}
