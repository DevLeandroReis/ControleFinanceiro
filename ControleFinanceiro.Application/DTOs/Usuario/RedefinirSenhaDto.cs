using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Usuario
{
    public class RedefinirSenhaDto
    {
        [Required(ErrorMessage = "O token � obrigat�rio")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha � obrigat�ria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirma��o de senha � obrigat�ria")]
        [Compare("NovaSenha", ErrorMessage = "As senhas n�o coincidem")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }
}