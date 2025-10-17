namespace ControleFinanceiro.Domain.Entities
{
    public class Conta : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativa { get; set; }
        public Guid ProprietarioId { get; set; }

        // Relacionamentos
        public virtual Usuario Proprietario { get; set; } = null!;
        public virtual ICollection<UsuarioConta> UsuarioContas { get; set; } = new List<UsuarioConta>();
        public virtual ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();
        public virtual ICollection<SolicitacaoAcessoConta> Solicitacoes { get; set; } = new List<SolicitacaoAcessoConta>();

        public Conta()
        {
            Ativa = true;
        }

        public Conta(string nome, Guid proprietarioId, string? descricao = null) : this()
        {
            Nome = nome;
            ProprietarioId = proprietarioId;
            Descricao = descricao;
        }

        public void AlterarNome(string nome)
        {
            Nome = nome;
            Update();
        }

        public void AlterarDescricao(string? descricao)
        {
            Descricao = descricao;
            Update();
        }

        public void Ativar()
        {
            Ativa = true;
            Update();
        }

        public void Desativar()
        {
            Ativa = false;
            Update();
        }

        public bool UsuarioTemAcesso(Guid usuarioId)
        {
            return ProprietarioId == usuarioId || 
                   UsuarioContas.Any(uc => uc.UsuarioId == usuarioId && uc.Ativo);
        }

        public bool UsuarioPodeAdicionarOutros(Guid usuarioId)
        {
            if (ProprietarioId == usuarioId) return true;
            
            var usuarioConta = UsuarioContas.FirstOrDefault(uc => uc.UsuarioId == usuarioId);
            return usuarioConta?.PodeAdicionarUsuarios == true;
        }
    }
}