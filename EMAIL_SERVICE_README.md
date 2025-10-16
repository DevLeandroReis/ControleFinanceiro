# Configura��o do Servi�o de Email

## Vis�o Geral

O sistema de email foi implementado seguindo a arquitetura DDD (Domain-Driven Design) com separa��o clara de responsabilidades:

- **Application Layer**: Interfaces dos servi�os (`IEmailService`, `IEmailTemplateService`)
- **Infrastructure Layer**: Implementa��es concretas (`EmailService`, `EmailTemplateService`)
- **Presentation Layer**: Configura��o e inje��o de depend�ncias

## Estrutura de Arquivos

```
ControleFinanceiro.Application/
??? Interfaces/Services/
?   ??? IEmailService.cs
?   ??? IEmailTemplateService.cs
??? Models/
?   ??? EmailSettings.cs
??? Services/
    ??? UsuarioService.cs (atualizado)

ControleFinanceiro.Infrastructure/
??? Services/
    ??? EmailService.cs
    ??? EmailTemplateService.cs

ControleFinanceiro.Presentation/
??? Program.cs (atualizado)
??? appsettings.json (atualizado)
??? appsettings.Development.json (atualizado)
```

## Configura��o

### 1. Configurar o SMTP no appsettings.json

```json
{
  "EmailSettings": {
    "SmtpServer": "smtp.gmail.com",
    "SmtpPort": 587,
    "SenderName": "Controle Financeiro",
    "SenderEmail": "seu-email@gmail.com",
    "Username": "seu-email@gmail.com",
    "Password": "sua-senha-de-aplicativo",
    "EnableSsl": true,
    "BaseUrl": "http://localhost:5000"
  }
}
```

### 2. Configura��o para Gmail

Para usar o Gmail, voc� precisa criar uma **Senha de Aplicativo**:

1. Acesse sua conta Google
2. V� para "Seguran�a" > "Verifica��o em duas etapas" (ative se n�o estiver ativo)
3. V� para "Senhas de app"
4. Selecione "Email" e "Windows Computer" (ou outro dispositivo)
5. Copie a senha gerada (16 caracteres sem espa�os)
6. Cole no campo `Password` do `EmailSettings`

**Importante**: Nunca use sua senha normal do Gmail. Sempre use senhas de aplicativo.

### 3. Outros Provedores SMTP

#### Outlook/Hotmail
```json
{
  "SmtpServer": "smtp-mail.outlook.com",
  "SmtpPort": 587,
  "EnableSsl": true
}
```

#### SendGrid
```json
{
  "SmtpServer": "smtp.sendgrid.net",
  "SmtpPort": 587,
  "Username": "apikey",
  "Password": "SUA_API_KEY_SENDGRID",
  "EnableSsl": true
}
```

#### AWS SES
```json
{
  "SmtpServer": "email-smtp.us-east-1.amazonaws.com",
  "SmtpPort": 587,
  "Username": "SEU_SMTP_USERNAME",
  "Password": "SEU_SMTP_PASSWORD",
  "EnableSsl": true
}
```

## Funcionalidades Implementadas

### 1. Confirma��o de Email
Enviado automaticamente ap�s o registro de um novo usu�rio.

```csharp
await _emailService.EnviarConfirmacaoEmailAsync(email, nomeUsuario, token);
```

### 2. Recupera��o de Senha
Enviado quando o usu�rio solicita recupera��o de senha.

```csharp
await _emailService.EnviarTokenRecuperacaoSenhaAsync(email, nomeUsuario, token);
```

### 3. Email Gen�rico
Para enviar qualquer tipo de email personalizado.

```csharp
await _emailService.EnviarEmailAsync(destinatario, assunto, corpoHtml);
```

## Templates de Email

Os templates s�o gerenciados pelo `EmailTemplateService` e s�o completamente separados da l�gica de envio.

### Templates Dispon�veis:

1. **Template de Confirma��o de Email**: Design responsivo com bot�o de confirma��o
2. **Template de Recupera��o de Senha**: Design responsivo com avisos de seguran�a

### Como Adicionar Novos Templates:

1. **Adicionar m�todo na interface `IEmailTemplateService`**:
```csharp
string GerarTemplateNovaMensagem(string parametro1, string parametro2);
```

2. **Implementar o template em `EmailTemplateService`**:
```csharp
public string GerarTemplateNovaMensagem(string parametro1, string parametro2)
{
    return $@"
<!DOCTYPE html>
<html lang='pt-BR'>
<head>
    <meta charset='UTF-8'>
    <style>
        /* Seus estilos CSS aqui */
    </style>
</head>
<body>
    <div class='container'>
        <!-- Seu conte�do HTML aqui -->
        <p>{parametro1}</p>
        <p>{parametro2}</p>
    </div>
</body>
</html>";
}
```

3. **Criar m�todo wrapper em `IEmailService` e `EmailService`** (opcional mas recomendado):
```csharp
// Interface
Task EnviarNovaMensagemAsync(string email, string parametro1, string parametro2);

// Implementa��o
public async Task EnviarNovaMensagemAsync(string email, string parametro1, string parametro2)
{
    var corpoHtml = _emailTemplateService.GerarTemplateNovaMensagem(parametro1, parametro2);
    await EnviarEmailAsync(email, "Assunto da Mensagem", corpoHtml);
}
```

## Logging

O servi�o de email inclui logging autom�tico para:
- Emails enviados com sucesso
- Erros ao enviar emails

Exemplo de logs:
```
[Information] Email enviado com sucesso para usuario@example.com
[Information] Email de confirma��o enviado para usuario@example.com
[Error] Erro ao enviar email para usuario@example.com
```

## Tratamento de Erros

O servi�o de email lan�a exce��es em caso de erro, permitindo que a camada de aplica��o decida como trat�-las:

```csharp
try
{
    await _emailService.EnviarEmailAsync(email, assunto, corpo);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Falha ao enviar email");
    // Decidir se deve falhar a opera��o ou continuar
}
```

## Seguran�a

### Boas Pr�ticas:

1. **Nunca commite senhas reais no reposit�rio**
2. Use vari�veis de ambiente em produ��o
3. Use senhas de aplicativo, n�o senhas normais
4. Valide sempre o destinat�rio do email
5. Limite a taxa de envio para evitar spam
6. Use HTTPS na BaseUrl em produ��o

### Configura��o com Vari�veis de Ambiente:

```bash
export EmailSettings__SmtpServer="smtp.gmail.com"
export EmailSettings__SmtpPort="587"
export EmailSettings__SenderEmail="seu-email@gmail.com"
export EmailSettings__Password="sua-senha-de-aplicativo"
```

## Testes

Para testar o envio de emails em desenvolvimento:

1. Configure um email de teste no `appsettings.Development.json`
2. Registre um novo usu�rio via API
3. Verifique os logs para confirmar o envio
4. Confira a caixa de entrada do email configurado

## Pr�ximos Passos

Poss�veis melhorias futuras:

- [ ] Fila de emails ass�ncronos (usando Hangfire ou similar)
- [ ] Templates din�micos com Razor Pages
- [ ] Envio de emails em lote
- [ ] Rastreamento de emails (abertos/clicados)
- [ ] Templates multil�ngues
- [ ] Anexos de arquivos
- [ ] Email de boas-vindas personalizado
- [ ] Email de notifica��o de transa��es

## Depend�ncias

- **MailKit 4.14.1**: Biblioteca robusta para envio de emails via SMTP
- **MimeKit**: Para constru��o de mensagens MIME

## Suporte

Para problemas comuns:

1. **Erro de autentica��o**: Verifique se est� usando senha de aplicativo
2. **Timeout de conex�o**: Verifique firewall e porta SMTP
3. **Email n�o chega**: Verifique pasta de spam
4. **SSL/TLS Error**: Ajuste a configura��o `EnableSsl`
