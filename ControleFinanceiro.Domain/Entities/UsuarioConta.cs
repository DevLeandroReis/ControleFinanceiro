using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Domain.Entities
{
    public class UsuarioConta : BaseEntity
    {
        public Guid UsuarioId { get; set; }
        public Guid ContaId { get; set; }
        public bool PodeAdicionarUsuarios { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAdesao { get; set; }

        // Relacionamentos
        public virtual Usuario Usuario { get; set; } = null!;
        public virtual Conta Conta { get; set; } = null!;

        public UsuarioConta()
        {
            Ativo = true;
            DataAdesao = DateTime.UtcNow;
            PodeAdicionarUsuarios = false;
        }

        public UsuarioConta(Guid usuarioId, Guid contaId, bool podeAdicionarUsuarios = false) : this()
        {
            UsuarioId = usuarioId;
            ContaId = contaId;
            PodeAdicionarUsuarios = podeAdicionarUsuarios;
        }

        public void ConcederPermissaoAdicionarUsuarios()
        {
            PodeAdicionarUsuarios = true;
            Update();
        }

        public void RemoverPermissaoAdicionarUsuarios()
        {
            PodeAdicionarUsuarios = false;
            Update();
        }

        public void Ativar()
        {
            Ativo = true;
            Update();
        }

        public void Desativar()
        {
            Ativo = false;
            Update();
        }
    }
}