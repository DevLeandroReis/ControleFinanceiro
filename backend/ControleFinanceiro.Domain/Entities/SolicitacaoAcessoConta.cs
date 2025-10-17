using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Domain.Entities
{
    public class SolicitacaoAcessoConta : BaseEntity
    {
        public Guid SolicitanteId { get; set; }
        public Guid ProprietarioId { get; set; }
        public Guid ContaId { get; set; }
        public StatusSolicitacao Status { get; set; }
        public string? Mensagem { get; set; }
        public DateTime? DataResposta { get; set; }

        // Relacionamentos
        public virtual Usuario Solicitante { get; set; } = null!;
        public virtual Usuario Proprietario { get; set; } = null!;
        public virtual Conta Conta { get; set; } = null!;

        public SolicitacaoAcessoConta()
        {
            Status = StatusSolicitacao.Pendente;
        }

        public SolicitacaoAcessoConta(Guid solicitanteId, Guid proprietarioId, Guid contaId, string? mensagem = null) : this()
        {
            SolicitanteId = solicitanteId;
            ProprietarioId = proprietarioId;
            ContaId = contaId;
            Mensagem = mensagem;
        }

        public void Aprovar()
        {
            Status = StatusSolicitacao.Aprovada;
            DataResposta = DateTime.UtcNow;
            Update();
        }

        public void Rejeitar()
        {
            Status = StatusSolicitacao.Rejeitada;
            DataResposta = DateTime.UtcNow;
            Update();
        }

        public void Cancelar()
        {
            Status = StatusSolicitacao.Cancelada;
            DataResposta = DateTime.UtcNow;
            Update();
        }

        public bool PodeSerRespondida()
        {
            return Status == StatusSolicitacao.Pendente;
        }
    }
}