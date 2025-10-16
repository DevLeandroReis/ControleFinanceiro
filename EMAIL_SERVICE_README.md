# Configuração do Serviço de Email

## Visão Geral

O sistema de email foi implementado seguindo a arquitetura DDD (Domain-Driven Design) com separação clara de responsabilidades:

- **Application Layer**: Interfaces dos serviços (`IEmailService`, `IEmailTemplateService`)
- **Infrastructure Layer**: Implementações concretas (`EmailService`, `EmailTemplateService`)
- **Presentation Layer**: Configuração e injeção de dependências

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

## Configuração

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

### 2. Configuração para Gmail

Para usar o Gmail, você precisa criar uma **Senha de Aplicativo**:

1. Acesse sua conta Google
2. Vá para "Segurança" > "Verificação em duas etapas" (ative se não estiver ativo)
3. Vá para "Senhas de app"
4. Selecione "Email" e "Windows Computer" (ou outro dispositivo)
5. Copie a senha gerada (16 caracteres sem espaços)
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

### 1. Confirmação de Email
Enviado automaticamente após o registro de um novo usuário.

```csharp
await _emailService.EnviarConfirmacaoEmailAsync(email, nomeUsuario, token);
```

### 2. Recuperação de Senha
Enviado quando o usuário solicita recuperação de senha.

```csharp
await _emailService.EnviarTokenRecuperacaoSenhaAsync(email, nomeUsuario, token);
```

### 3. Email Genérico
Para enviar qualquer tipo de email personalizado.

```csharp
await _emailService.EnviarEmailAsync(destinatario, assunto, corpoHtml);
```

## Templates de Email

Os templates são gerenciados pelo `EmailTemplateService` e são completamente separados da lógica de envio.

### Templates Disponíveis:

1. **Template de Confirmação de Email**: Design responsivo com botão de confirmação
2. **Template de Recuperação de Senha**: Design responsivo com avisos de segurança

### Como Adicionar Novos Templates:

1. **Adicionar método na interface `IEmailTemplateService`**:
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
        <!-- Seu conteúdo HTML aqui -->
        <p>{parametro1}</p>
        <p>{parametro2}</p>
    </div>
</body>
</html>";
}
```

3. **Criar método wrapper em `IEmailService` e `EmailService`** (opcional mas recomendado):
```csharp
// Interface
Task EnviarNovaMensagemAsync(string email, string parametro1, string parametro2);

// Implementação
public async Task EnviarNovaMensagemAsync(string email, string parametro1, string parametro2)
{
    var corpoHtml = _emailTemplateService.GerarTemplateNovaMensagem(parametro1, parametro2);
    await EnviarEmailAsync(email, "Assunto da Mensagem", corpoHtml);
}
```

## Logging

O serviço de email inclui logging automático para:
- Emails enviados com sucesso
- Erros ao enviar emails

Exemplo de logs:
```
[Information] Email enviado com sucesso para usuario@example.com
[Information] Email de confirmação enviado para usuario@example.com
[Error] Erro ao enviar email para usuario@example.com
```

## Tratamento de Erros

O serviço de email lança exceções em caso de erro, permitindo que a camada de aplicação decida como tratá-las:

```csharp
try
{
    await _emailService.EnviarEmailAsync(email, assunto, corpo);
}
catch (Exception ex)
{
    _logger.LogError(ex, "Falha ao enviar email");
    // Decidir se deve falhar a operação ou continuar
}
```

## Segurança

### Boas Práticas:

1. **Nunca commite senhas reais no repositório**
2. Use variáveis de ambiente em produção
3. Use senhas de aplicativo, não senhas normais
4. Valide sempre o destinatário do email
5. Limite a taxa de envio para evitar spam
6. Use HTTPS na BaseUrl em produção

### Configuração com Variáveis de Ambiente:

```bash
export EmailSettings__SmtpServer="smtp.gmail.com"
export EmailSettings__SmtpPort="587"
export EmailSettings__SenderEmail="seu-email@gmail.com"
export EmailSettings__Password="sua-senha-de-aplicativo"
```

## Testes

Para testar o envio de emails em desenvolvimento:

1. Configure um email de teste no `appsettings.Development.json`
2. Registre um novo usuário via API
3. Verifique os logs para confirmar o envio
4. Confira a caixa de entrada do email configurado

## Próximos Passos

Possíveis melhorias futuras:

- [ ] Fila de emails assíncronos (usando Hangfire ou similar)
- [ ] Templates dinâmicos com Razor Pages
- [ ] Envio de emails em lote
- [ ] Rastreamento de emails (abertos/clicados)
- [ ] Templates multilíngues
- [ ] Anexos de arquivos
- [ ] Email de boas-vindas personalizado
- [ ] Email de notificação de transações

## Dependências

- **MailKit 4.14.1**: Biblioteca robusta para envio de emails via SMTP
- **MimeKit**: Para construção de mensagens MIME

## Suporte

Para problemas comuns:

1. **Erro de autenticação**: Verifique se está usando senha de aplicativo
2. **Timeout de conexão**: Verifique firewall e porta SMTP
3. **Email não chega**: Verifique pasta de spam
4. **SSL/TLS Error**: Ajuste a configuração `EnableSsl`
