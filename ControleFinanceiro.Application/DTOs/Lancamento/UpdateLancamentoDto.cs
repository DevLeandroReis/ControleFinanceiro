using ControleFinanceiro.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ControleFinanceiro.Application.DTOs.Lancamento
{
    public class UpdateLancamentoDto
    {
        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(200, ErrorMessage = "A descrição deve ter no máximo 200 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "A data de vencimento é obrigatória")]
        public DateTime DataVencimento { get; set; }

        [Required(ErrorMessage = "O tipo é obrigatório")]
        public TipoLancamento Tipo { get; set; }

        [StringLength(1000, ErrorMessage = "As observações devem ter no máximo 1000 caracteres")]
        public string? Observacoes { get; set; }

        public bool EhRecorrente { get; set; } = false;

        public TipoRecorrencia TipoRecorrencia { get; set; } = TipoRecorrencia.Nenhuma;

        [Range(1, int.MaxValue, ErrorMessage = "A quantidade de parcelas deve ser maior que zero")]
        public int? QuantidadeParcelas { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória")]
        public Guid CategoriaId { get; set; }

        [Required(ErrorMessage = "A conta é obrigatória")]
        public Guid ContaId { get; set; }
    }
}