# Cole��o Postman - Controle Financeiro API (Usu�rios)

Este diret�rio cont�m os arquivos de cole��o e environment do Postman para testar todas as rotas do **UsuariosController** da API de Controle Financeiro.

## ?? Arquivos

- **ControleFinanceiro.Postman.Collection.json** - Cole��o com todas as requisi��es do UsuariosController
- **ControleFinanceiro.Postman.Environment.json** - Vari�veis de ambiente para facilitar os testes

## ?? Como Importar

### Op��o 1: Importar via Interface do Postman

1. Abra o Postman
2. Clique em **Import** (canto superior esquerdo)
3. Arraste os dois arquivos JSON ou clique em **Upload Files**
4. Selecione ambos os arquivos:
   - `ControleFinanceiro.Postman.Collection.json`
   - `ControleFinanceiro.Postman.Environment.json`
5. Clique em **Import**

### Op��o 2: Importar via Linha de Comando

Se voc� usa o Postman CLI:

```bash
postman collection import ControleFinanceiro.Postman.Collection.json
postman environment import ControleFinanceiro.Postman.Environment.json
```

## ?? Configura��o

### Selecionar o Environment

1. No canto superior direito do Postman, clique no dropdown de environments
2. Selecione **Controle Financeiro - Local**

### Configurar a URL Base (se necess�rio)

Se sua API roda em uma porta diferente, edite a vari�vel `base_url`:

1. Clique no �cone de "olho" ao lado do dropdown de environments
2. Clique em **Edit** na se��o do environment ativo
3. Altere o valor de `base_url` para a URL correta (ex: `http://localhost:5000`)

## ?? Rotas Dispon�veis

A cole��o cont�m as seguintes requisi��es, organizadas em ordem l�gica de teste:

### 1. Rotas P�blicas (N�o Requerem Autentica��o)

1. **01 - Registrar Novo Usu�rio** (POST)
   - Endpoint: `/api/usuarios/registrar`
   - Cria um novo usu�rio no sistema
   - Salva automaticamente o `usuario_id` ap�s sucesso

2. **02 - Login de Usu�rio** (POST)
   - Endpoint: `/api/usuarios/login`
   - Autentica o usu�rio e retorna token JWT
   - Salva automaticamente o `auth_token` ap�s sucesso

3. **03 - Recuperar Senha** (POST)
   - Endpoint: `/api/usuarios/recuperar-senha`
   - Inicia o processo de recupera��o de senha

4. **04 - Redefinir Senha** (POST)
   - Endpoint: `/api/usuarios/redefinir-senha`
   - Redefine a senha usando o token recebido

5. **05 - Confirmar Email** (GET)
   - Endpoint: `/api/usuarios/confirmar-email`
   - Confirma o email do usu�rio

### 2. Rotas Protegidas (Requerem Autentica��o)

6. **06 - Obter Usu�rio por ID** (GET)
   - Endpoint: `/api/usuarios/{id}`
   - Retorna os dados de um usu�rio espec�fico

7. **07 - Obter Todos os Usu�rios** (GET)
   - Endpoint: `/api/usuarios`
   - Lista todos os usu�rios cadastrados

8. **08 - Ativar Usu�rio** (PATCH)
   - Endpoint: `/api/usuarios/{id}/ativar`
   - Ativa um usu�rio desativado

9. **09 - Desativar Usu�rio** (PATCH)
   - Endpoint: `/api/usuarios/{id}/desativar`
   - Desativa um usu�rio ativo

## ?? Autentica��o

As rotas protegidas utilizam **Bearer Token** para autentica��o. O token � obtido automaticamente ap�s fazer login com sucesso (requisi��o 02).

### Fluxo de Autentica��o Autom�tica

1. Execute a requisi��o **02 - Login de Usu�rio**
2. O token JWT ser� salvo automaticamente na vari�vel `auth_token`
3. Todas as requisi��es subsequentes que requerem autentica��o usar�o este token automaticamente

### Autentica��o Manual

Se precisar configurar o token manualmente:

1. Fa�a login e copie o token da resposta
2. Edite o environment e cole o token na vari�vel `auth_token`

## ?? Testes Automatizados

Cada requisi��o possui testes automatizados que verificam:

- Status code da resposta
- Estrutura do JSON retornado
- Salvamento autom�tico de vari�veis importantes

### Executar Todos os Testes

Para executar todos os testes em sequ�ncia:

1. Clique nos tr�s pontos (...) ao lado da cole��o
2. Selecione **Run collection**
3. Configure a ordem de execu��o (recomendado manter a ordem padr�o)
4. Clique em **Run Controle Financeiro - Usuarios API**

## ?? Vari�veis de Environment

| Vari�vel | Descri��o | Tipo |
|----------|-----------|------|
| `base_url` | URL base da API | String |
| `auth_token` | Token JWT de autentica��o | Secret |
| `usuario_id` | ID do usu�rio criado/logado | String |

## ?? Dicas de Uso

### Ordem Recomendada de Execu��o

Para testar o fluxo completo:

1. **Registrar Novo Usu�rio** - Cria um usu�rio
2. **Login de Usu�rio** - Obt�m o token
3. **Obter Usu�rio por ID** - Verifica os dados
4. **Obter Todos os Usu�rios** - Lista todos
5. **Desativar Usu�rio** - Testa desativa��o
6. **Ativar Usu�rio** - Testa ativa��o

### Testando Recupera��o de Senha

?? **Nota**: Para testar as rotas de recupera��o e redefini��o de senha, voc� precisar�:

1. Executar **Recuperar Senha**
2. Verificar o token no banco de dados ou logs (se configurado para desenvolvimento)
3. Copiar o token e usar na requisi��o **Redefinir Senha**

### Testando Confirma��o de Email

?? **Nota**: Para testar a confirma��o de email, voc� precisar�:

1. Ap�s registrar um usu�rio, obter o `tokenConfirmacaoEmail` do banco de dados
2. Usar este token na requisi��o **Confirmar Email**

## ?? Troubleshooting

### Erro 401 (Unauthorized)

- Verifique se voc� executou o login (requisi��o 02)
- Verifique se o token n�o expirou
- Certifique-se de que o environment est� selecionado

### Erro 404 (Not Found)

- Verifique se a API est� rodando
- Confirme se a `base_url` est� correta
- Verifique se o `usuario_id` est� definido (execute o registro ou login primeiro)

### Erro de Conex�o

- Certifique-se de que a API est� rodando: `dotnet run`
- Verifique se a porta est� correta (padr�o: 5001 para HTTPS)
- Desative verifica��o SSL se necess�rio (n�o recomendado para produ��o)

## ?? Recursos Adicionais

- [Documenta��o do Postman](https://learning.postman.com/docs/getting-started/introduction/)
- [Swagger da API](https://localhost:5001) - Ap�s iniciar a aplica��o

## ?? Licen�a

Este arquivo de cole��o faz parte do projeto Controle Financeiro.
