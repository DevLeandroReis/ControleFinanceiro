using ControleFinanceiro.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Lancamento
{
    public class UpdateLancamentoDto
    {
        [Required(ErrorMessage = "A descri��o � obrigat�ria")]
        [StringLength(200, ErrorMessage = "A descri��o deve ter no m�ximo 200 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O valor � obrigat�rio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A data de vencimento � obrigat�ria")]
        public DateTime DataVencimento { get; set; }

        [Required(ErrorMessage = "O tipo � obrigat�rio")]
        public TipoLancamento Tipo { get; set; }

        [StringLength(1000, ErrorMessage = "As observa��es devem ter no m�ximo 1000 caracteres")]
        public string? Observacoes { get; set; }

        public bool EhRecorrente { get; set; } = false;

        public TipoRecorrencia TipoRecorrencia { get; set; } = TipoRecorrencia.Nenhuma;

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade de parcelas deve ser maior que zero")]
        public int? QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "A categoria � obrigat�ria")]
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "A conta � obrigat�ria")]
        public Guid ContaId { get; set; }
    }
}