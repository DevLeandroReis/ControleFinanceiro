using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public bool EmailConfirmado { get; set; }
        public string? TokenConfirmacaoEmail { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public bool Ativo { get; set; }
        public string? TokenRecuperacaoSenha { get; set; }
        public DateTime? TokenRecuperacaoExpira { get; set; }

        // Relacionamentos
        public virtual ICollection<UsuarioConta> UsuarioContas { get; set; } = new List<UsuarioConta>();
        public virtual ICollection<SolicitacaoAcessoConta> SolicitacoesEnviadas { get; set; } = new List<SolicitacaoAcessoConta>();
        public virtual ICollection<SolicitacaoAcessoConta> SolicitacoesRecebidas { get; set; } = new List<SolicitacaoAcessoConta>();

        public Usuario()
        {
            Ativo = true;
            EmailConfirmado = false;
        }

        public Usuario(string nome, string email, string senhaHash) : this()
        {
            Nome = nome;
            Email = email;
            SenhaHash = senhaHash;
        }

        public void ConfirmarEmail()
        {
            EmailConfirmado = true;
            TokenConfirmacaoEmail = null;
            Update();
        }

        public void DefinirTokenConfirmacaoEmail(string token)
        {
            TokenConfirmacaoEmail = token;
            Update();
        }

        public void DefinirTokenRecuperacaoSenha(string token, DateTime expiracao)
        {
            TokenRecuperacaoSenha = token;
            TokenRecuperacaoExpira = expiracao;
            Update();
        }

        public void AlterarSenha(string novaSenhaHash)
        {
            SenhaHash = novaSenhaHash;
            TokenRecuperacaoSenha = null;
            TokenRecuperacaoExpira = null;
            Update();
        }

        public void AtualizarUltimoLogin()
        {
            UltimoLogin = DateTime.UtcNow;
            Update();
        }

        public bool PodeRecuperarSenha()
        {
            return !string.IsNullOrEmpty(TokenRecuperacaoSenha) &&
                   TokenRecuperacaoExpira.HasValue &&
                   TokenRecuperacaoExpira.Value > DateTime.UtcNow;
        }
    }
}