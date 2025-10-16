using System.ComponentModel.DataAnnotations;
using ControleFinanceiro.Domain.Enums;

namespace ControleFinanceiro.Application.DTOs.Categoria
{
    public class UpdateCategoriaDto
    {
        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres")]
        public string Nome { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        [StringLength(7, ErrorMessage = "A cor deve ter no máximo 7 caracteres")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "A cor deve estar no formato hexadecimal (#FFFFFF)")]
        public string? Cor { get; set; }

        [Required(ErrorMessage = "O tipo é obrigatório")]
        public TipoCategoria Tipo { get; set; }

        public bool Ativo { get; set; } = true;

        public bool Destacada { get; set; } = false;
    }
}