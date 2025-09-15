using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Domain.Entities
{
    public class Lancamento : BaseEntity
    {
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public TipoLancamento Tipo { get; set; }
        public StatusLancamento Status { get; set; }
        public string? Observacoes { get; set; }
        
        // Campos para recorrência
        public bool EhRecorrente { get; set; }
        public TipoRecorrencia TipoRecorrencia { get; set; }
        public int? QuantidadeParcelas { get; set; }
        public int? ParcelaAtual { get; set; }
        public Guid? LancamentoPaiId { get; set; }
        
        // Relacionamentos
        public Guid CategoriaId { get; set; }
        public virtual Categoria Categoria { get; set; } = null!;
        public virtual Lancamento? LancamentoPai { get; set; }
        public virtual ICollection<Lancamento> LancamentosFilhos { get; set; } = new List<Lancamento>();

        public Lancamento() 
        {
            Status = StatusLancamento.Pendente;
            EhRecorrente = false;
            TipoRecorrencia = TipoRecorrencia.Nenhuma;
        }

        public Lancamento(string descricao, decimal valor, DateTime dataVencimento, TipoLancamento tipo, Guid categoriaId) : this()
        {
            Descricao = descricao;
            Valor = valor;
            DataVencimento = dataVencimento;
            Tipo = tipo;
            CategoriaId = categoriaId;
        }

        public void MarcarComoPago(DateTime? dataPagamento = null)
        {
            Status = StatusLancamento.Pago;
            DataPagamento = dataPagamento ?? DateTime.UtcNow;
            Update();
        }

        public void MarcarComoPendente()
        {
            Status = StatusLancamento.Pendente;
            DataPagamento = null;
            Update();
        }

        public void Cancelar()
        {
            Status = StatusLancamento.Cancelado;
            Update();
        }

        public bool EstaVencido()
        {
            return Status == StatusLancamento.Pendente && DataVencimento < DateTime.Today;
        }

        public void ConfigurarRecorrencia(TipoRecorrencia tipoRecorrencia, int? quantidadeParcelas = null)
        {
            EhRecorrente = true;
            TipoRecorrencia = tipoRecorrencia;
            QuantidadeParcelas = quantidadeParcelas;
            ParcelaAtual = 1;
            Update();
        }

        public void RemoverRecorrencia()
        {
            EhRecorrente = false;
            TipoRecorrencia = TipoRecorrencia.Nenhuma;
            QuantidadeParcelas = null;
            ParcelaAtual = null;
            Update();
        }
    }
}