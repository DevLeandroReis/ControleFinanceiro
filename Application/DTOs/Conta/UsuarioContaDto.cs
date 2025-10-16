namespace ControleFinanceiro.Application.DTOs.Conta
{
    public class UsuarioContaDto
    {
        public Guid Id { get; set; }
        public Guid UsuarioId { get; set; }
        public string UsuarioNome { get; set; } = string.Empty;
        public string UsuarioEmail { get; set; } = string.Empty;
        public Guid ContaId { get; set; }
        public string ContaNome { get; set; } = string.Empty;
        public bool PodeAdicionarUsuarios { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataAdesao { get; set; }
    }
}