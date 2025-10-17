using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Application.DTOs.Categoria
{
    public class CategoriaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Cor { get; set; }
        public bool Ativo { get; set; }
        public bool Destacada { get; set; }
        public TipoCategoria Tipo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}