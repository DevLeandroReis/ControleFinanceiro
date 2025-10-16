using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Application.DTOs.Lancamento
{
    public class LancamentoDto
    {
        public Guid Id { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public TipoLancamento Tipo { get; set; }
        public StatusLancamento Status { get; set; }
        public string? Observacoes { get; set; }
        public bool EhRecorrente { get; set; }
        public TipoRecorrencia TipoRecorrencia { get; set; }
        public int? QuantidadeParcelas { get; set; }
        public int? ParcelaAtual { get; set; }
        public Guid? LancamentoPaiId { get; set; }
        public Guid CategoriaId { get; set; }
        public string? CategoriaNome { get; set; }
        public Guid ContaId { get; set; }
        public string? ContaNome { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}