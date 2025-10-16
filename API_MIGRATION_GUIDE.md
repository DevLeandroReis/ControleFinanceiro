# Guia de Migra��o - API de Lan�amentos

## ?? Resumo

A API de lan�amentos foi atualizada para receber **lista de IDs de contas** nos m�todos de consulta, melhorando seguran�a, performance e flexibilidade.

## ?? Mudan�as na API

### Endpoints Atualizados

Todos os endpoints de consulta agora requerem o par�metro `contaIds` (lista de GUIDs):

| Endpoint Antigo | Endpoint Novo | Mudan�a |
|----------------|---------------|---------|
| `GET /api/lancamentos/periodo?dataInicio={date}&dataFim={date}` | `GET /api/lancamentos/periodo?dataInicio={date}&dataFim={date}&contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |
| `GET /api/lancamentos/categoria/{id}` | `GET /api/lancamentos/categoria/{id}?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |
| `GET /api/lancamentos/tipo/{tipo}` | `GET /api/lancamentos/tipo/{tipo}?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |
| `GET /api/lancamentos/status/{status}` | `GET /api/lancamentos/status/{status}?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |
| `GET /api/lancamentos/vencidos` | `GET /api/lancamentos/vencidos?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |
| `GET /api/lancamentos/recorrentes` | `GET /api/lancamentos/recorrentes?contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |
| `GET /api/lancamentos/saldo?dataInicio={date}&dataFim={date}` | `GET /api/lancamentos/saldo?dataInicio={date}&dataFim={date}&contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |
| `GET /api/lancamentos/receitas/total?dataInicio={date}&dataFim={date}` | `GET /api/lancamentos/receitas/total?dataInicio={date}&dataFim={date}&contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |
| `GET /api/lancamentos/despesas/total?dataInicio={date}&dataFim={date}` | `GET /api/lancamentos/despesas/total?dataInicio={date}&dataFim={date}&contaIds={guid}&contaIds={guid}` | ? Adicionado `contaIds` (obrigat�rio) |

### Endpoints Sem Mudan�a

Estes endpoints **N�O** foram alterados:

- ? `GET /api/lancamentos` - Lista todos os lan�amentos (filtra automaticamente)
- ? `GET /api/lancamentos/{id}` - Busca por ID
- ? `GET /api/lancamentos/conta/{contaId}` - Busca por uma conta espec�fica
- ? `POST /api/lancamentos` - Criar lan�amento
- ? `PUT /api/lancamentos/{id}` - Atualizar lan�amento
- ? `DELETE /api/lancamentos/{id}` - Deletar lan�amento
- ? Demais opera��es de CRUD

## ?? Exemplos de Migra��o

### JavaScript/TypeScript (Fetch API)

#### Antes:
```javascript
// Buscar lan�amentos por per�odo
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
// Buscar lan�amentos por per�odo de contas espec�ficas
const contaIds = ['550e8400-e29b-41d4-a716-446655440000', '550e8400-e29b-41d4-a716-446655440001'];
const params = new URLSearchParams({
  dataInicio: '2024-01-01',
  dataFim: '2024-01-31'
});

// Adicionar m�ltiplos contaIds
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

# Adicionar m�ltiplos contaIds
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

### Novos C�digos de Status

| Status Code | Situa��o | Mensagem |
|-------------|----------|----------|
| `400 Bad Request` | Lista de contas n�o fornecida | `"� necess�rio informar pelo menos uma conta"` |
| `400 Bad Request` | Lista de contas vazia | `"� necess�rio informar pelo menos uma conta"` |
| `401 Unauthorized` | Sem permiss�o para alguma conta | `"Voc� n�o tem permiss�o para acessar a conta com ID '{contaId}'"` |

### Exemplo de Tratamento:

```typescript
try {
  const response = await fetch('/api/lancamentos/periodo?...');
  
  if (!response.ok) {
    if (response.status === 400) {
      // Lista de contas inv�lida
      const error = await response.json();
      console.error('Erro de valida��o:', error);
    } else if (response.status === 401) {
      // Sem permiss�o para alguma conta
      const error = await response.json();
      console.error('Sem permiss�o:', error);
    }
  }
  
  const data = await response.json();
  return data;
} catch (error) {
  console.error('Erro na requisi��o:', error);
}
```

## ?? Benef�cios da Mudan�a

### 1. Seguran�a
- ? Verifica��o expl�cita de permiss�es antes de consultar dados
- ? Imposs�vel acessar lan�amentos de contas n�o autorizadas
- ? Mensagens de erro espec�ficas

### 2. Performance
- ? Queries SQL mais eficientes (WHERE IN ao inv�s de m�ltiplas queries)
- ? Menos dados trafegados na rede
- ? Melhor uso de �ndices do banco de dados

### 3. Flexibilidade
- ? Buscar lan�amentos de m�ltiplas contas em uma �nica requisi��o
- ? �til para dashboards consolidados
- ? Permite an�lises comparativas

## ?? Casos de Uso Comuns

### 1. Dashboard com M�ltiplas Contas

```typescript
// Buscar saldo consolidado de todas as contas do usu�rio
const minhasContas = await getMinhasContas(); // ['conta1', 'conta2', 'conta3']
const saldo = await getSaldo('2024-01-01', '2024-01-31', minhasContas);
```

### 2. Relat�rio de Uma Conta Espec�fica

```typescript
// Buscar lan�amentos de uma �nica conta
const contaId = '550e8400-e29b-41d4-a716-446655440000';
const lancamentos = await getLancamentosPorPeriodo(
  '2024-01-01', 
  '2024-01-31', 
  [contaId] // Array com uma �nica conta
);
```

### 3. Compara��o Entre Contas

```typescript
// Comparar receitas de duas contas
const conta1 = '550e8400-e29b-41d4-a716-446655440000';
const conta2 = '550e8400-e29b-41d4-a716-446655440001';

const receitasConta1 = await getTotalReceitas('2024-01-01', '2024-01-31', [conta1]);
const receitasConta2 = await getTotalReceitas('2024-01-01', '2024-01-31', [conta2]);
```

## ?? Breaking Changes

### O que deixar� de funcionar:

? Chamadas sem o par�metro `contaIds` nos endpoints atualizados:
```
GET /api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31
? Retornar� 400 Bad Request
```

? Chamadas com `contaIds` vazio:
```
GET /api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31&contaIds=
? Retornar� 400 Bad Request
```

### O que continuar� funcionando:

? Endpoint gen�rico sem filtros espec�ficos:
```
GET /api/lancamentos
? Retorna todos os lan�amentos do usu�rio (filtrando automaticamente por acesso)
```

? Endpoint de conta espec�fica:
```
GET /api/lancamentos/conta/{contaId}
? Mantido sem altera��es
```

## ?? Checklist de Migra��o

- [ ] Identificar todas as chamadas aos endpoints atualizados
- [ ] Adicionar l�gica para obter lista de contas do usu�rio
- [ ] Atualizar chamadas HTTP para incluir `contaIds` como array
- [ ] Implementar tratamento de erros 400 e 401
- [ ] Testar com uma conta
- [ ] Testar com m�ltiplas contas
- [ ] Testar com conta sem permiss�o
- [ ] Testar com lista vazia
- [ ] Atualizar documenta��o do frontend
- [ ] Atualizar testes automatizados

## ?? Suporte

Se tiver d�vidas sobre a migra��o ou encontrar problemas:

1. Verifique se est� passando a lista de contas corretamente
2. Confirme que o usu�rio tem permiss�o para todas as contas da lista
3. Verifique os logs do servidor para mensagens de erro detalhadas
4. Consulte a documenta��o Swagger da API

---

**Data da Mudan�a**: Janeiro 2024  
**Vers�o da API**: 2.0  
**Breaking Changes**: Sim (endpoints de consulta requerem `contaIds`)
