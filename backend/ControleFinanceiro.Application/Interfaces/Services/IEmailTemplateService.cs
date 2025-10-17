namespace ControleFinanceiro.Application.Interfaces.Services
{
    public interface IEmailTemplateService
    {
        string GerarTemplateConfirmacaoEmail(string nomeUsuario, string urlConfirmacao);
        string GerarTemplateRecuperacaoSenha(string nomeUsuario, string urlRecuperacao);
    }
}
