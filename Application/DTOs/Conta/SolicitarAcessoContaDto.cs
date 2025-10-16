using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Conta
{
    public class SolicitarAcessoContaDto
    {
        [Required(ErrorMessage = "O ID da conta � obrigat�rio")]
        public Guid ContaId { get; set; }

        [StringLength(500, ErrorMessage = "A mensagem deve ter no m�ximo 500 caracteres")]
        public string? Mensagem { get; set; }
    }
}