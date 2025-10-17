using ControleFinanceiro.Application.Interfaces.Services;

namespace ControleFinanceiro.Infrastructure.Services
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly string _confirmacaoEmailTemplatePath;
        private readonly string _recuperacaoSenhaTemplatePath;

        public EmailTemplateService()
        {
            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            _confirmacaoEmailTemplatePath = Path.Combine(basePath, "Services", "Email", "Templates", "ConfirmacaoEmailTemplate.html");
            _recuperacaoSenhaTemplatePath = Path.Combine(basePath, "Services", "Email", "Templates", "RecuperacaoSenhaTemplate.html");
        }

        public string GerarTemplateConfirmacaoEmail(string nomeUsuario, string urlConfirmacao)
        {
            var template = File.ReadAllText(_confirmacaoEmailTemplatePath);
            return template
                .Replace("{nomeUsuario}", nomeUsuario)
                .Replace("{urlConfirmacao}", urlConfirmacao);
        }

        public string GerarTemplateRecuperacaoSenha(string nomeUsuario, string urlRecuperacao)
        {
            var template = File.ReadAllText(_recuperacaoSenhaTemplatePath);
            return template
                .Replace("{nomeUsuario}", nomeUsuario)
                .Replace("{urlRecuperacao}", urlRecuperacao);
        }
    }
}
