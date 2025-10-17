using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Usuario
{
    public class RedefinirSenhaDto
    {
        [Required(ErrorMessage = "O token é obrigatório")]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "A nova senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string NovaSenha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarNovaSenha { get; set; } = string.Empty;
    }
}