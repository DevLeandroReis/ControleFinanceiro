using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Conta
{
    public class UpdateContaDto
    {
        [Required(ErrorMessage = "O nome � obrigat�rio")]
        [StringLength(100, ErrorMessage = "O nome deve ter no m�ximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descri��o deve ter no m�ximo 500 caracteres")]
        public string? Descricao { get; set; }
    }
}