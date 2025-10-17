# Coleção Postman - Controle Financeiro API (Usuários)

Este diretório contém os arquivos de coleção e environment do Postman para testar todas as rotas do **UsuariosController** da API de Controle Financeiro.

## ?? Arquivos

- **ControleFinanceiro.Postman.Collection.json** - Coleção com todas as requisições do UsuariosController
- **ControleFinanceiro.Postman.Environment.json** - Variáveis de ambiente para facilitar os testes

## ?? Como Importar

### Opção 1: Importar via Interface do Postman

1. Abra o Postman
2. Clique em **Import** (canto superior esquerdo)
3. Arraste os dois arquivos JSON ou clique em **Upload Files**
4. Selecione ambos os arquivos:
   - `ControleFinanceiro.Postman.Collection.json`
   - `ControleFinanceiro.Postman.Environment.json`
5. Clique em **Import**

### Opção 2: Importar via Linha de Comando

Se você usa o Postman CLI:

```bash
postman collection import ControleFinanceiro.Postman.Collection.json
postman environment import ControleFinanceiro.Postman.Environment.json
```

## ?? Configuração

### Selecionar o Environment

1. No canto superior direito do Postman, clique no dropdown de environments
2. Selecione **Controle Financeiro - Local**

### Configurar a URL Base (se necessário)

Se sua API roda em uma porta diferente, edite a variável `base_url`:

1. Clique no ícone de "olho" ao lado do dropdown de environments
2. Clique em **Edit** na seção do environment ativo
3. Altere o valor de `base_url` para a URL correta (ex: `http://localhost:5000`)

## ?? Rotas Disponíveis

A coleção contém as seguintes requisições, organizadas em ordem lógica de teste:

### 1. Rotas Públicas (Não Requerem Autenticação)

1. **01 - Registrar Novo Usuário** (POST)
   - Endpoint: `/api/usuarios/registrar`
   - Cria um novo usuário no sistema
   - Salva automaticamente o `usuario_id` após sucesso

2. **02 - Login de Usuário** (POST)
   - Endpoint: `/api/usuarios/login`
   - Autentica o usuário e retorna token JWT
   - Salva automaticamente o `auth_token` após sucesso

3. **03 - Recuperar Senha** (POST)
   - Endpoint: `/api/usuarios/recuperar-senha`
   - Inicia o processo de recuperação de senha

4. **04 - Redefinir Senha** (POST)
   - Endpoint: `/api/usuarios/redefinir-senha`
   - Redefine a senha usando o token recebido

5. **05 - Confirmar Email** (GET)
   - Endpoint: `/api/usuarios/confirmar-email`
   - Confirma o email do usuário

### 2. Rotas Protegidas (Requerem Autenticação)

6. **06 - Obter Usuário por ID** (GET)
   - Endpoint: `/api/usuarios/{id}`
   - Retorna os dados de um usuário específico

7. **07 - Obter Todos os Usuários** (GET)
   - Endpoint: `/api/usuarios`
   - Lista todos os usuários cadastrados

8. **08 - Ativar Usuário** (PATCH)
   - Endpoint: `/api/usuarios/{id}/ativar`
   - Ativa um usuário desativado

9. **09 - Desativar Usuário** (PATCH)
   - Endpoint: `/api/usuarios/{id}/desativar`
   - Desativa um usuário ativo

## ?? Autenticação

As rotas protegidas utilizam **Bearer Token** para autenticação. O token é obtido automaticamente após fazer login com sucesso (requisição 02).

### Fluxo de Autenticação Automática

1. Execute a requisição **02 - Login de Usuário**
2. O token JWT será salvo automaticamente na variável `auth_token`
3. Todas as requisições subsequentes que requerem autenticação usarão este token automaticamente

### Autenticação Manual

Se precisar configurar o token manualmente:

1. Faça login e copie o token da resposta
2. Edite o environment e cole o token na variável `auth_token`

## ?? Testes Automatizados

Cada requisição possui testes automatizados que verificam:

- Status code da resposta
- Estrutura do JSON retornado
- Salvamento automático de variáveis importantes

### Executar Todos os Testes

Para executar todos os testes em sequência:

1. Clique nos três pontos (...) ao lado da coleção
2. Selecione **Run collection**
3. Configure a ordem de execução (recomendado manter a ordem padrão)
4. Clique em **Run Controle Financeiro - Usuarios API**

## ?? Variáveis de Environment

| Variável | Descrição | Tipo |
|----------|-----------|------|
| `base_url` | URL base da API | String |
| `auth_token` | Token JWT de autenticação | Secret |
| `usuario_id` | ID do usuário criado/logado | String |

## ?? Dicas de Uso

### Ordem Recomendada de Execução

Para testar o fluxo completo:

1. **Registrar Novo Usuário** - Cria um usuário
2. **Login de Usuário** - Obtém o token
3. **Obter Usuário por ID** - Verifica os dados
4. **Obter Todos os Usuários** - Lista todos
5. **Desativar Usuário** - Testa desativação
6. **Ativar Usuário** - Testa ativação

### Testando Recuperação de Senha

?? **Nota**: Para testar as rotas de recuperação e redefinição de senha, você precisará:

1. Executar **Recuperar Senha**
2. Verificar o token no banco de dados ou logs (se configurado para desenvolvimento)
3. Copiar o token e usar na requisição **Redefinir Senha**

### Testando Confirmação de Email

?? **Nota**: Para testar a confirmação de email, você precisará:

1. Após registrar um usuário, obter o `tokenConfirmacaoEmail` do banco de dados
2. Usar este token na requisição **Confirmar Email**

## ?? Troubleshooting

### Erro 401 (Unauthorized)

- Verifique se você executou o login (requisição 02)
- Verifique se o token não expirou
- Certifique-se de que o environment está selecionado

### Erro 404 (Not Found)

- Verifique se a API está rodando
- Confirme se a `base_url` está correta
- Verifique se o `usuario_id` está definido (execute o registro ou login primeiro)

### Erro de Conexão

- Certifique-se de que a API está rodando: `dotnet run`
- Verifique se a porta está correta (padrão: 5001 para HTTPS)
- Desative verificação SSL se necessário (não recomendado para produção)

## ?? Recursos Adicionais

- [Documentação do Postman](https://learning.postman.com/docs/getting-started/introduction/)
- [Swagger da API](https://localhost:5001) - Após iniciar a aplicação

## ?? Licença

Este arquivo de coleção faz parte do projeto Controle Financeiro.
