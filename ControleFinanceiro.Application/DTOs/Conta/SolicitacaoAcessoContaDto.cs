using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Application.DTOs.Conta
{
    public class SolicitacaoAcessoContaDto
    {
        public Guid Id { get; set; }
        public Guid SolicitanteId { get; set; }
        public string SolicitanteNome { get; set; } = string.Empty;
        public string SolicitanteEmail { get; set; } = string.Empty;
        public Guid ProprietarioId { get; set; }
        public string ProprietarioNome { get; set; } = string.Empty;
        public Guid ContaId { get; set; }
        public string ContaNome { get; set; } = string.Empty;
        public StatusSolicitacao Status { get; set; }
        public string? Mensagem { get; set; }
        public DateTime? DataResposta { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}