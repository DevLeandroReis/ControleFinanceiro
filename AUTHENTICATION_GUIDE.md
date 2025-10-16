# Controle Financeiro API - Autenticação e Autorização

## ?? Visão Geral de Segurança

A API implementa autenticação JWT (JSON Web Token) e autorização baseada em usuários e contas. Todas as rotas, exceto registro e login, requerem autenticação.

## ?? Regras de Negócio

### Contas
- **Proprietário**: Usuário que criou a conta
  - Pode visualizar, editar, excluir e desativar a conta
  - Pode adicionar/remover outros usuários
  - Pode conceder/remover permissões

- **Usuários Compartilhados**: Usuários com acesso à conta
  - Podem visualizar e gerenciar lançamentos e categorias
  - Podem ter permissão para adicionar outros usuários (concedida pelo proprietário)

### Lançamentos e Categorias
- Somente usuários com acesso à conta podem:
  - Visualizar lançamentos e categorias
  - Criar novos registros
  - Editar registros existentes
  - Excluir registros

## ?? Como Usar a API

### 1. Registrar Novo Usuário

```http
POST /api/usuarios/registrar
Content-Type: application/json

{
  "nome": "João Silva",
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
    "nome": "João Silva",
    "email": "joao@example.com",
    "emailConfirmado": false,
    "ativo": true
  },
  "expiresAt": "2024-01-02T12:00:00Z"
}
```

### 3. Usar o Token nas Requisições

Todas as requisições autenticadas devem incluir o token no cabeçalho:

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

#### Aprovar Solicitação (Proprietário)
```http
POST /api/contas/solicitacoes/{solicitacaoId}/aprovar
Authorization: Bearer {token}
```

### Gerenciamento de Lançamentos

#### Criar Lançamento
```http
POST /api/lancamentos
Authorization: Bearer {token}
Content-Type: application/json

{
  "descricao": "Salário",
  "valor": 5000.00,
  "dataVencimento": "2024-01-05",
  "tipo": "Receita",
  "categoriaId": "550e8400-e29b-41d4-a716-446655440000",
  "contaId": "550e8400-e29b-41d4-a716-446655440001",
  "observacoes": "Pagamento mensal"
}
```

#### Listar Lançamentos
```http
GET /api/lancamentos
Authorization: Bearer {token}
```

#### Filtrar por Conta
```http
GET /api/lancamentos/conta/{contaId}
Authorization: Bearer {token}
```

#### Filtrar por Período
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

### Criar Lançamentos Recorrentes
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

## ?? Configuração JWT

As configurações JWT estão no `appsettings.json`:

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

?? **IMPORTANTE**: Em produção, use variáveis de ambiente para a `SecretKey` e nunca commit ela no repositório.

## ??? Tratamento de Erros

### 401 Unauthorized
Você receberá este erro se:
- Não fornecer o token
- O token estiver expirado
- O token for inválido
- Tentar acessar um recurso sem permissão

### 403 Forbidden
Você não tem permissão para acessar o recurso solicitado.

### 404 Not Found
O recurso solicitado não existe.

## ?? Exemplo de Uso com Swagger

1. Acesse `http://localhost:5000` (ou a porta configurada)
2. Clique em "Authorize" no canto superior direito
3. Insira o token no formato: `Bearer {seu_token}`
4. Clique em "Authorize" e depois "Close"
5. Agora você pode testar todos os endpoints

## ?? Fluxo de Compartilhamento de Conta

```mermaid
sequenceDiagram
    participant U1 as Usuário 1 (Proprietário)
    participant U2 as Usuário 2 (Solicitante)
    participant API as API

    U1->>API: POST /contas (Cria conta)
    API-->>U1: Conta criada
    
    U2->>API: POST /contas/solicitar-acesso
    API-->>U2: Solicitação criada
    API-->>U1: Notificação de solicitação
    
    U1->>API: GET /contas/solicitacoes/recebidas
    API-->>U1: Lista de solicitações
    
    U1->>API: POST /contas/solicitacoes/{id}/aprovar
    API-->>U1: Solicitação aprovada
    API-->>U2: Acesso concedido
    
    U2->>API: GET /lancamentos (Visualiza lançamentos da conta)
    API-->>U2: Lançamentos permitidos
```

## ?? Notas de Segurança

1. **Tokens JWT**: Válidos por 24 horas por padrão
2. **Senhas**: Armazenadas com BCrypt (hash seguro)
3. **HTTPS**: Recomendado para produção
4. **CORS**: Configure adequadamente para seu frontend
5. **Rate Limiting**: Considere implementar para produção

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

# Criar lançamento
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

| Método | Endpoint | Autenticação | Descrição |
|--------|----------|--------------|-----------|
| POST | `/api/usuarios/registrar` | ? Não | Registrar novo usuário |
| POST | `/api/usuarios/login` | ? Não | Fazer login |
| GET | `/api/usuarios/{id}` | ? Sim | Obter usuário por ID |
| POST | `/api/contas` | ? Sim | Criar conta |
| GET | `/api/contas/minhas` | ? Sim | Listar minhas contas |
| GET | `/api/contas/{id}` | ? Sim | Obter conta (se tiver acesso) |
| POST | `/api/contas/solicitar-acesso` | ? Sim | Solicitar acesso à conta |
| GET | `/api/lancamentos` | ? Sim | Listar lançamentos (filtra por acesso) |
| POST | `/api/lancamentos` | ? Sim | Criar lançamento |
| PUT | `/api/lancamentos/{id}` | ? Sim | Atualizar lançamento |
| DELETE | `/api/lancamentos/{id}` | ? Sim | Excluir lançamento |
| GET | `/api/categorias` | ? Sim | Listar categorias |

---

**Desenvolvido com ?? usando .NET 8 e Entity Framework Core**
