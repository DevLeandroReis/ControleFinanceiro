namespace ControleFinanceiro.Domain.Entities
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Cor { get; set; }
        public bool Ativo { get; set; }

        // Relacionamentos
        public virtual ICollection<Lancamento> Lancamentos { get; set; } = new List<Lancamento>();

        public Categoria()
        {
            Ativo = true;
        }

        public Categoria(string nome, string? descricao = null, string? cor = null) : this()
        {
            Nome = nome;
            Descricao = descricao;
            Cor = cor;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
        
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