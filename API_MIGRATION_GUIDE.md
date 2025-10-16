# Guia de Migração - API de Lançamentos

## ?? Resumo

A API de lançamentos foi atualizada para receber **lista de IDs de contas** nos métodos de consulta, melhorando segurança, performance e flexibilidade.

## ?? Mudanças na API

### Endpoints Atualizados

Todos os endpoints de consulta agora requerem o parâmetro `contaIds` (lista de GUIDs):

| Endpoint Antigo | Endpoint Novo | Mudança |
|----------------|---------------|---------|
| `GET /api/lancamentos/periodo?dataInicio={date}&dataFim={date}` | `GET /api/lancamentos/periodo?dataInicio={date}&dataFim={date}&contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |
| `GET /api/lancamentos/categoria/{id}` | `GET /api/lancamentos/categoria/{id}?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |
| `GET /api/lancamentos/tipo/{tipo}` | `GET /api/lancamentos/tipo/{tipo}?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |
| `GET /api/lancamentos/status/{status}` | `GET /api/lancamentos/status/{status}?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |
| `GET /api/lancamentos/vencidos` | `GET /api/lancamentos/vencidos?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |
| `GET /api/lancamentos/recorrentes` | `GET /api/lancamentos/recorrentes?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |
| `GET /api/lancamentos/saldo?dataInicio={date}&dataFim={date}` | `GET /api/lancamentos/saldo?dataInicio={date}&dataFim={date}&contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |
| `GET /api/lancamentos/receitas/total?dataInicio={date}&dataFim={date}` | `GET /api/lancamentos/receitas/total?dataInicio={date}&dataFim={date}&contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |
| `GET /api/lancamentos/despesas/total?dataInicio={date}&dataFim={date}` | `GET /api/lancamentos/despesas/total?dataInicio={date}&dataFim={date}&contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigatório) |

### Endpoints Sem Mudança

Estes endpoints **NÃO** foram alterados:

- ? `GET /api/lancamentos` - Lista todos os lançamentos (filtra automaticamente)
- ? `GET /api/lancamentos/{id}` - Busca por ID
- ? `GET /api/lancamentos/conta/{contaId}` - Busca por uma conta específica
- ? `POST /api/lancamentos` - Criar lançamento
- ? `PUT /api/lancamentos/{id}` - Atualizar lançamento
- ? `DELETE /api/lancamentos/{id}` - Deletar lançamento
- ? Demais operações de CRUD

## ?? Exemplos de Migração

### JavaScript/TypeScript (Fetch API)

#### Antes:
```javascript
// Buscar lançamentos por período
const response = await fetch(
  `/api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31`,
  {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  }
);
```

#### Depois:
```javascript
// Buscar lançamentos por período de contas específicas
const contaIds = ['550e8400-e29b-41d4-a716-446655440000', '550e8400-e29b-41d4-a716-446655440001'];
const params = new URLSearchParams({
  dataInicio: '2024-01-01',
  dataFim: '2024-01-31'
});

// Adicionar múltiplos contaIds
contaIds.forEach(id => params.append('contaIds', id));

const response = await fetch(
  `/api/lancamentos/periodo?${params.toString()}`,
  {
    headers: {
      'Authorization': `Bearer ${token}`
    }
  }
);
```

### C# (HttpClient)

#### Antes:
```csharp
var response = await httpClient.GetAsync(
    $"/api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31"
);
```

#### Depois:
```csharp
var contaIds = new List<Guid> 
{ 
    Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
    Guid.Parse("550e8400-e29b-41d4-a716-446655440001")
};

var queryParams = new StringBuilder($"?dataInicio=2024-01-01&dataFim=2024-01-31");
foreach (var contaId in contaIds)
{
    queryParams.Append($"&contaIds={contaId}");
}

var response = await httpClient.GetAsync($"/api/lancamentos/periodo{queryParams}");
```

### Python (Requests)

#### Antes:
```python
response = requests.get(
    f"{base_url}/api/lancamentos/periodo",
    params={
        'dataInicio': '2024-01-01',
        'dataFim': '2024-01-31'
    },
    headers={'Authorization': f'Bearer {token}'}
)
```

#### Depois:
```python
conta_ids = [
    '550e8400-e29b-41d4-a716-446655440000',
    '550e8400-e29b-41d4-a716-446655440001'
]

params = {
    'dataInicio': '2024-01-01',
    'dataFim': '2024-01-31'
}

# Adicionar múltiplos contaIds
for conta_id in conta_ids:
    params.setdefault('contaIds', []).append(conta_id)

response = requests.get(
    f"{base_url}/api/lancamentos/periodo",
    params=params,
    headers={'Authorization': f'Bearer {token}'}
)
```

### Axios (JavaScript/TypeScript)

#### Antes:
```typescript
const response = await axios.get('/api/lancamentos/periodo', {
  params: {
    dataInicio: '2024-01-01',
    dataFim: '2024-01-31'
  },
  headers: {
    'Authorization': `Bearer ${token}`
  }
});
```

#### Depois:
```typescript
const contaIds = [
  '550e8400-e29b-41d4-a716-446655440000',
  '550e8400-e29b-41d4-a716-446655440001'
];

const response = await axios.get('/api/lancamentos/periodo', {
  params: {
    dataInicio: '2024-01-01',
    dataFim: '2024-01-31',
    contaIds: contaIds
  },
  headers: {
    'Authorization': `Bearer ${token}`
  },
  paramsSerializer: params => {
    // Axios serializa arrays automaticamente como key=value1&key=value2
    return new URLSearchParams(
      Object.entries(params).flatMap(([key, values]) =>
        Array.isArray(values)
          ? values.map(value => [key, value])
          : [[key, values]]
      )
    ).toString();
  }
});
```

## ?? Tratamento de Erros

### Novos Códigos de Status

| Status Code | Situação | Mensagem |
|-------------|----------|----------|
| `400 Bad Request` | Lista de contas não fornecida | `"É necessário informar pelo menos uma conta"` |
| `400 Bad Request` | Lista de contas vazia | `"É necessário informar pelo menos uma conta"` |
| `401 Unauthorized` | Sem permissão para alguma conta | `"Você não tem permissão para acessar a conta com ID '{contaId}'"` |

### Exemplo de Tratamento:

```typescript
try {
  const response = await fetch('/api/lancamentos/periodo?...');
  
  if (!response.ok) {
    if (response.status === 400) {
      // Lista de contas inválida
      const error = await response.json();
      console.error('Erro de validação:', error);
    } else if (response.status === 401) {
      // Sem permissão para alguma conta
      const error = await response.json();
      console.error('Sem permissão:', error);
    }
  }
  
  const data = await response.json();
  return data;
} catch (error) {
  console.error('Erro na requisição:', error);
}
```

## ?? Benefícios da Mudança

### 1. Segurança
- ? Verificação explícita de permissões antes de consultar dados
- ? Impossível acessar lançamentos de contas não autorizadas
- ? Mensagens de erro específicas

### 2. Performance
- ? Queries SQL mais eficientes (WHERE IN ao invés de múltiplas queries)
- ? Menos dados trafegados na rede
- ? Melhor uso de índices do banco de dados

### 3. Flexibilidade
- ? Buscar lançamentos de múltiplas contas em uma única requisição
- ? Útil para dashboards consolidados
- ? Permite análises comparativas

## ?? Casos de Uso Comuns

### 1. Dashboard com Múltiplas Contas

```typescript
// Buscar saldo consolidado de todas as contas do usuário
const minhasContas = await getMinhasContas(); // ['conta1', 'conta2', 'conta3']
const saldo = await getSaldo('2024-01-01', '2024-01-31', minhasContas);
```

### 2. Relatório de Uma Conta Específica

```typescript
// Buscar lançamentos de uma única conta
const contaId = '550e8400-e29b-41d4-a716-446655440000';
const lancamentos = await getLancamentosPorPeriodo(
  '2024-01-01', 
  '2024-01-31', 
  [contaId] // Array com uma única conta
);
```

### 3. Comparação Entre Contas

```typescript
// Comparar receitas de duas contas
const conta1 = '550e8400-e29b-41d4-a716-446655440000';
const conta2 = '550e8400-e29b-41d4-a716-446655440001';

const receitasConta1 = await getTotalReceitas('2024-01-01', '2024-01-31', [conta1]);
const receitasConta2 = await getTotalReceitas('2024-01-01', '2024-01-31', [conta2]);
```

## ?? Breaking Changes

### O que deixará de funcionar:

? Chamadas sem o parâmetro `contaIds` nos endpoints atualizados:
```
GET /api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31
? Retornará 400 Bad Request
```

? Chamadas com `contaIds` vazio:
```
GET /api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31&contaIds=
? Retornará 400 Bad Request
```

### O que continuará funcionando:

? Endpoint genérico sem filtros específicos:
```
GET /api/lancamentos
? Retorna todos os lançamentos do usuário (filtrando automaticamente por acesso)
```

? Endpoint de conta específica:
```
GET /api/lancamentos/conta/{contaId}
? Mantido sem alterações
```

## ?? Checklist de Migração

- [ ] Identificar todas as chamadas aos endpoints atualizados
- [ ] Adicionar lógica para obter lista de contas do usuário
- [ ] Atualizar chamadas HTTP para incluir `contaIds` como array
- [ ] Implementar tratamento de erros 400 e 401
- [ ] Testar com uma conta
- [ ] Testar com múltiplas contas
- [ ] Testar com conta sem permissão
- [ ] Testar com lista vazia
- [ ] Atualizar documentação do frontend
- [ ] Atualizar testes automatizados

## ?? Suporte

Se tiver dúvidas sobre a migração ou encontrar problemas:

1. Verifique se está passando a lista de contas corretamente
2. Confirme que o usuário tem permissão para todas as contas da lista
3. Verifique os logs do servidor para mensagens de erro detalhadas
4. Consulte a documentação Swagger da API

---

**Data da Mudança**: Janeiro 2024  
**Versão da API**: 2.0  
**Breaking Changes**: Sim (endpoints de consulta requerem `contaIds`)
