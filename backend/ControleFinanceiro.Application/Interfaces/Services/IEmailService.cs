namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml);
        Task EnviarConfirmacaoEmailAsync(string email, string nomeUsuario, string token);
        Task EnviarTokenRecuperacaoSenhaAsync(string email, string nomeUsuario, string token);
    }
}
