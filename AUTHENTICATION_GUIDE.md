# Controle Financeiro API - Autentica��o e Autoriza��o

## ?? Vis�o Geral de Seguran�a

A API implementa autentica��o JWT (JSON Web Token) e autoriza��o baseada em usu�rios e contas. Todas as rotas, exceto registro e login, requerem autentica��o.

## ?? Regras de Neg�cio

### Contas
- **Propriet�rio**: Usu�rio que criou a conta
  - Pode visualizar, editar, excluir e desativar a conta
  - Pode adicionar/remover outros usu�rios
  - Pode conceder/remover permiss�es

- **Usu�rios Compartilhados**: Usu�rios com acesso � conta
  - Podem visualizar e gerenciar lan�amentos e categorias
  - Podem ter permiss�o para adicionar outros usu�rios (concedida pelo propriet�rio)

### Lan�amentos e Categorias
- Somente usu�rios com acesso � conta podem:
  - Visualizar lan�amentos e categorias
  - Criar novos registros
  - Editar registros existentes
  - Excluir registros

## ?? Como Usar a API

### 1. Registrar Novo Usu�rio

```http
POST /api/usuarios/registrar
Content-Type: application/json

{
  "nome": "Jo�o Silva",
  "email": "joao@example.com",
  "senha": "SenhaForte123!",
  "confirmarSenha": "SenhaForte123!"
}
```

### 2. Fazer Login

```http
POST /api/usuarios/login
Content-Type: application/json

{
  "email": "joao@example.com",
  "senha": "SenhaForte123!"
}
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "usuario": {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "nome": "Jo�o Silva",
    "email": "joao@example.com",
    "emailConfirmado": false,
    "ativo": true
  },
  "expiresAt": "2024-01-02T12:00:00Z"
}
```

### 3. Usar o Token nas Requisi��es

Todas as requisi��es autenticadas devem incluir o token no cabe�alho:

```http
GET /api/contas/minhas
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## ?? Fluxo Completo de Uso

### Gerenciamento de Contas

#### Criar uma Conta
```http
POST /api/contas
Authorization: Bearer {token}
Content-Type: application/json

{
  "nome": "Conta Pessoal",
  "descricao": "Minha conta principal"
}
```

#### Listar Minhas Contas
```http
GET /api/contas/minhas
Authorization: Bearer {token}
```

#### Solicitar Acesso a uma Conta
```http
POST /api/contas/solicitar-acesso
Authorization: Bearer {token}
Content-Type: application/json

{
  "contaId": "550e8400-e29b-41d4-a716-446655440000",
  "mensagem": "Gostaria de ter acesso para gerenciar despesas"
}
```

#### Aprovar Solicita��o (Propriet�rio)
```http
POST /api/contas/solicitacoes/{solicitacaoId}/aprovar
Authorization: Bearer {token}
```

### Gerenciamento de Lan�amentos

#### Criar Lan�amento
```http
POST /api/lancamentos
Authorization: Bearer {token}
Content-Type: application/json

{
  "descricao": "Sal�rio",
  "valor": 5000.00,
  "dataVencimento": "2024-01-05",
  "tipo": "Receita",
  "categoriaId": "550e8400-e29b-41d4-a716-446655440000",
  "contaId": "550e8400-e29b-41d4-a716-446655440001",
  "observacoes": "Pagamento mensal"
}
```

#### Listar Lan�amentos
```http
GET /api/lancamentos
Authorization: Bearer {token}
```

#### Filtrar por Conta
```http
GET /api/lancamentos/conta/{contaId}
Authorization: Bearer {token}
```

#### Filtrar por Per�odo
```http
GET /api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31
Authorization: Bearer {token}
```

#### Marcar como Pago
```http
PATCH /api/lancamentos/{id}/pagar
Authorization: Bearer {token}
Content-Type: application/json

"2024-01-05T10:30:00Z"
```

### Criar Lan�amentos Recorrentes
```http
POST /api/lancamentos/recorrentes
Authorization: Bearer {token}
Content-Type: application/json

{
  "descricao": "Aluguel",
  "valor": 1200.00,
  "dataVencimento": "2024-01-10",
  "tipo": "Despesa",
  "categoriaId": "550e8400-e29b-41d4-a716-446655440002",
  "contaId": "550e8400-e29b-41d4-a716-446655440001",
  "ehRecorrente": true,
  "tipoRecorrencia": "Mensal",
  "quantidadeParcelas": 12
}
```

## ?? Configura��o JWT

As configura��es JWT est�o no `appsettings.json`:

```json
{
  "JWT": {
    "SecretKey": "S3cr3t_K3y_F0r_C0ntr0l3_F1n4nc31r0_4ppl1c4t10n_2024_M1n1mum_256_b1ts",
    "Issuer": "ControleFinanceiro",
    "Audience": "ControleFinanceiro",
    "ExpirationHours": "24"
  }
}
```

?? **IMPORTANTE**: Em produ��o, use vari�veis de ambiente para a `SecretKey` e nunca commit ela no reposit�rio.

## ??? Tratamento de Erros

### 401 Unauthorized
Voc� receber� este erro se:
- N�o fornecer o token
- O token estiver expirado
- O token for inv�lido
- Tentar acessar um recurso sem permiss�o

### 403 Forbidden
Voc� n�o tem permiss�o para acessar o recurso solicitado.

### 404 Not Found
O recurso solicitado n�o existe.

## ?? Exemplo de Uso com Swagger

1. Acesse `http://localhost:5000` (ou a porta configurada)
2. Clique em "Authorize" no canto superior direito
3. Insira o token no formato: `Bearer {seu_token}`
4. Clique em "Authorize" e depois "Close"
5. Agora voc� pode testar todos os endpoints

## ?? Fluxo de Compartilhamento de Conta

```mermaid
sequenceDiagram
    participant U1 as Usu�rio 1 (Propriet�rio)
    participant U2 as Usu�rio 2 (Solicitante)
    participant API as API

    U1->>API: POST /contas (Cria conta)
    API-->>U1: Conta criada
    
    U2->>API: POST /contas/solicitar-acesso
    API-->>U2: Solicita��o criada
    API-->>U1: Notifica��o de solicita��o
    
    U1->>API: GET /contas/solicitacoes/recebidas
    API-->>U1: Lista de solicita��es
    
    U1->>API: POST /contas/solicitacoes/{id}/aprovar
    API-->>U1: Solicita��o aprovada
    API-->>U2: Acesso concedido
    
    U2->>API: GET /lancamentos (Visualiza lan�amentos da conta)
    API-->>U2: Lan�amentos permitidos
```

## ?? Notas de Seguran�a

1. **Tokens JWT**: V�lidos por 24 horas por padr�o
2. **Senhas**: Armazenadas com BCrypt (hash seguro)
3. **HTTPS**: Recomendado para produ��o
4. **CORS**: Configure adequadamente para seu frontend
5. **Rate Limiting**: Considere implementar para produ��o

## ?? Testando com cURL

```bash
# Login
curl -X POST http://localhost:5000/api/usuarios/login \
  -H "Content-Type: application/json" \
  -d '{"email":"joao@example.com","senha":"SenhaForte123!"}'

# Usar token retornado
TOKEN="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."

# Listar contas
curl -X GET http://localhost:5000/api/contas/minhas \
  -H "Authorization: Bearer $TOKEN"

# Criar lan�amento
curl -X POST http://localhost:5000/api/lancamentos \
  -H "Authorization: Bearer $TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "descricao": "Compra supermercado",
    "valor": 250.00,
    "dataVencimento": "2024-01-15",
    "tipo": "Despesa",
    "categoriaId": "...",
    "contaId": "..."
  }'
```

## ?? Resumo de Endpoints

| M�todo | Endpoint | Autentica��o | Descri��o |
|--------|----------|--------------|-----------|
| POST | `/api/usuarios/registrar` | ? N�o | Registrar novo usu�rio |
| POST | `/api/usuarios/login` | ? N�o | Fazer login |
| GET | `/api/usuarios/{id}` | ? Sim | Obter usu�rio por ID |
| POST | `/api/contas` | ? Sim | Criar conta |
| GET | `/api/contas/minhas` | ? Sim | Listar minhas contas |
| GET | `/api/contas/{id}` | ? Sim | Obter conta (se tiver acesso) |
| POST | `/api/contas/solicitar-acesso` | ? Sim | Solicitar acesso � conta |
| GET | `/api/lancamentos` | ? Sim | Listar lan�amentos (filtra por acesso) |
| POST | `/api/lancamentos` | ? Sim | Criar lan�amento |
| PUT | `/api/lancamentos/{id}` | ? Sim | Atualizar lan�amento |
| DELETE | `/api/lancamentos/{id}` | ? Sim | Excluir lan�amento |
| GET | `/api/categorias` | ? Sim | Listar categorias |

---

**Desenvolvido com ?? usando .NET 8 e Entity Framework Core**
