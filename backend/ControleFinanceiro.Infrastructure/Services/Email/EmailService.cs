using ControleFinanceiro.Application.Interfaces.Services;
using ControleFinanceiro.Application.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ControleFinanceiro.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly IEmailTemplateService _emailTemplateService;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IOptions<EmailSettings> emailSettings,
            IEmailTemplateService emailTemplateService,
            ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _emailTemplateService = emailTemplateService;
            _logger = logger;
        }

        public async Task EnviarEmailAsync(string destinatario, string assunto, string corpoHtml)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
                message.To.Add(MailboxAddress.Parse(destinatario));
                message.Subject = assunto;

                var builder = new BodyBuilder
                {
                    HtmlBody = corpoHtml
                };

                message.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                
                await smtp.ConnectAsync(
                    _emailSettings.SmtpServer, 
                    _emailSettings.SmtpPort, 
                    _emailSettings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.None);

                await smtp.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("Email enviado com sucesso para {Destinatario}", destinatario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email para {Destinatario}", destinatario);
                throw;
            }
        }

        public async Task EnviarConfirmacaoEmailAsync(string email, string nomeUsuario, string token)
        {
            try
            {
                var urlConfirmacao = $"{_emailSettings.BaseUrl}/api/usuarios/confirmar-email?token={token}";
                var corpoHtml = _emailTemplateService.GerarTemplateConfirmacaoEmail(nomeUsuario, urlConfirmacao);

                await EnviarEmailAsync(email, "Confirmação de Email - Controle Financeiro", corpoHtml);

                _logger.LogInformation("Email de confirmação enviado para {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email de confirmação para {Email}", email);
                throw;
            }
        }

        public async Task EnviarTokenRecuperacaoSenhaAsync(string email, string nomeUsuario, string token)
        {
            try
            {
                var urlRecuperacao = $"{_emailSettings.BaseUrl}/redefinir-senha?token={token}";
                var corpoHtml = _emailTemplateService.GerarTemplateRecuperacaoSenha(nomeUsuario, urlRecuperacao);

                await EnviarEmailAsync(email, "Recuperação de Senha - Controle Financeiro", corpoHtml);

                _logger.LogInformation("Email de recuperação de senha enviado para {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao enviar email de recuperação de senha para {Email}", email);
                throw;
            }
        }
    }
}
