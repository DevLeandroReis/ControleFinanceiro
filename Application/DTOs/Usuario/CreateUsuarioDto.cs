using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Usuario
{
    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [StringLength(255, ErrorMessage = "O email deve ter no máximo 255 caracteres")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação de senha é obrigatória")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        public string ConfirmarSenha { get; set; } = string.Empty;
    }
}