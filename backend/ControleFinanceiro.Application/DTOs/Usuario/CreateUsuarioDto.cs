using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Usuario
{
    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [StringLength(100, ErrorMessage = "O nome deve ter no m�ximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email � obrigat�rio")]
        [EmailAddress(ErrorMessage = "Email inv�lido")]
        [StringLength(255, ErrorMessage = "O email deve ter no m�ximo 255 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha � obrigat�ria")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirma��o de senha � obrigat�ria")]
        [Compare("Senha", ErrorMessage = "As senhas n�o coincidem")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}