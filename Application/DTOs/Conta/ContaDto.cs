using ControleFinanceiro.Application.DTOs.Usuario;

namespace ControleFinanceiro.Application.DTOs.Conta
{
    public class ContaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativa { get; set; }
        public Guid ProprietarioId { get; set; }
        public string ProprietarioNome { get; set; } = string.Empty;
        public List<UsuarioContaDto> Usuarios { get; set; } = new List<UsuarioContaDto>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}