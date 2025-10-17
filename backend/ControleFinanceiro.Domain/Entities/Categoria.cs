using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Domain.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Cor { get; set; }
        public bool Ativo { get; set; }
        public bool Destacada { get; set; }
        public TipoCategoria Tipo { get; set; }

        // Relacionamentos
        public virtual ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();

        public Categoria()
        {
            Ativo = true;
            Destacada = false;
        }

        public Categoria(string nome, TipoCategoria tipo, string? descricao = null, string? cor = null) : this()
        {
            Nome = nome;
            Tipo = tipo;
            Descricao = descricao;
            Cor = cor;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
        public void MarcarComoDestacada() => Destacada = true;
        public void DesmarcarComoDestacada() => Destacada = false;
        
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
    }
}