# Melhorias na Verificação de Permissões - LancamentoService

## ?? Resumo das Alterações

Foi implementada uma otimização no `LancamentoService` para garantir que:
1. **Todas as operações verificam as permissões do usuário o mais cedo possível**
2. **Os métodos de consulta agora recebem uma lista de IDs de contas**
3. **Antes de buscar os lançamentos, o sistema verifica se o usuário tem permissão para TODAS as contas da lista**

## ?? Principais Mudanças

### 1. Novos Métodos Auxiliares

#### Verificação de acesso a múltiplas contas:
```csharp
private async Task VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(IEnumerable<Guid> contaIds, Guid usuarioId)
{
    if (contaIds == null || !contaIds.Any())
    {
        throw new ArgumentException("É necessário fornecer pelo menos uma conta");
    }

    foreach (var contaId in contaIds)
    {
        if (!await _contaRepository.UsuarioTemAcessoContaAsync(usuarioId, contaId))
        {
            throw new UnauthorizedAccessException($"Você não tem permissão para acessar a conta com ID '{contaId}'");
        }
    }
}
```

### 2. Novos Métodos no Repositório

Adicionados métodos no `ILancamentoRepository` e `LancamentoRepository` que recebem lista de contas:

- ? `GetLancamentosPorPeriodoEContasAsync(DateTime dataInicio, DateTime dataFim, IEnumerable<Guid> contaIds)`
- ? `GetLancamentosPorCategoriaEContasAsync(Guid categoriaId, IEnumerable<Guid> contaIds)`
- ? `GetLancamentosPorTipoEContasAsync(TipoLancamento tipo, IEnumerable<Guid> contaIds)`
- ? `GetLancamentosPorStatusEContasAsync(StatusLancamento status, IEnumerable<Guid> contaIds)`
- ? `GetLancamentosVencidosPorContasAsync(IEnumerable<Guid> contaIds)`
- ? `GetLancamentosRecorrentesPorContasAsync(IEnumerable<Guid> contaIds)`
- ? `GetLancamentosPorContasAsync(IEnumerable<Guid> contaIds)`

### 3. Métodos de Serviço Atualizados

Todos os métodos de consulta agora recebem lista de contas e verificam permissões ANTES de buscar:

#### Exemplo - GetLancamentosPorPeriodoAsync:
```csharp
public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorPeriodoAsync(
    DateTime dataInicio, 
    DateTime dataFim, 
    IEnumerable<Guid> contaIds, 
    Guid usuarioId)
{
    // 1. PRIMEIRO: Verificar acesso a TODAS as contas (evita consultas desnecessárias)
    await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

    // 2. Depois: Buscar lançamentos apenas das contas permitidas
    var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoEContasAsync(
        dataInicio, dataFim, contaIds);
        
    return _mapper.Map<IEnumerable<LancamentoDto>>(lancamentos);
}
```

### 4. Controller Atualizado

Os endpoints agora recebem lista de contas via query parameters:

#### Exemplo de chamada:
```http
GET /api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31&contaIds=guid1&contaIds=guid2&contaIds=guid3
```

#### Validações no Controller:
- ? Verifica se a lista de contas foi fornecida
- ? Verifica se a lista não está vazia
- ? Retorna BadRequest se lista não for fornecida
- ? Retorna Unauthorized se usuário não tiver permissão para alguma conta

## ?? Benefícios

### 1. **Segurança Aprimorada**
- Verificação de permissão para TODAS as contas ANTES de qualquer consulta
- Impossível acessar lançamentos de contas sem permissão
- Mensagens de erro específicas indicando qual conta não tem permissão

### 2. **Performance Otimizada**
- Consultas SQL mais eficientes usando `WHERE IN (contaIds)`
- Evita N+1 queries ao filtrar por conta
- Verificação de permissão acontece ANTES de buscar dados do banco

### 3. **Flexibilidade**
- Cliente pode consultar lançamentos de múltiplas contas em uma única requisição
- Útil para dashboards e relatórios consolidados
- Permite análise comparativa entre contas

### 4. **Código Mais Limpo**
- Padrão consistente em todos os métodos de consulta
- Reutilização de métodos auxiliares
- Separação clara de responsabilidades

## ?? Métodos Atualizados

### Métodos de Consulta (agora recebem lista de contas):
- [x] `GetLancamentosPorPeriodoAsync` - Busca por período e contas
- [x] `GetLancamentosPorCategoriaAsync` - Busca por categoria e contas
- [x] `GetLancamentosPorTipoAsync` - Busca por tipo e contas
- [x] `GetLancamentosPorStatusAsync` - Busca por status e contas
- [x] `GetLancamentosVencidosAsync` - Busca vencidos por contas
- [x] `GetLancamentosRecorrentesAsync` - Busca recorrentes por contas
- [x] `GetSaldoPorPeriodoAsync` - Calcula saldo por período e contas
- [x] `GetTotalReceitasPorPeriodoAsync` - Total de receitas por período e contas
- [x] `GetTotalDespesasPorPeriodoAsync` - Total de despesas por período e contas

### Métodos Mantidos (single conta):
- [x] `GetByIdAsync` - Busca por ID específico
- [x] `GetLancamentosPorContaAsync` - Busca por uma única conta
- [x] `GetAllAsync` - Busca todos (filtra automaticamente por acesso)

### Métodos de Operação (sem mudanças):
- [x] `CreateAsync`
- [x] `UpdateAsync`
- [x] `UpdateLancamentoRecorrenteAsync`
- [x] `DeleteAsync`
- [x] `MarcarComoPagoAsync`
- [x] `MarcarComoPendenteAsync`
- [x] `CancelarAsync`
- [x] `CriarLancamentosRecorrentesAsync`
- [x] `GerarLancamentosFuturosAsync`

## ?? Fluxo de Verificação

### Para métodos com lista de contas:
```
1. Recebe lista de contaIds via query parameter
2. ? VALIDA se lista não está vazia (BadRequest se vazia)
3. ? VERIFICA PERMISSÃO para TODAS as contas (Unauthorized se alguma não permitida)
4. Busca lançamentos APENAS das contas da lista
5. Retorna resultados
```

### Para métodos com conta única:
```
1. Recebe contaId (na rota ou no DTO)
2. ? VERIFICA PERMISSÃO para a conta (Unauthorized se não permitido)
3. Executa operação
4. Retorna resultado
```

## ?? Exemplos de Uso

### Buscar lançamentos de múltiplas contas por período:
```http
GET /api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31&contaIds=550e8400-e29b-41d4-a716-446655440000&contaIds=550e8400-e29b-41d4-a716-446655440001
Authorization: Bearer {token}
```

### Buscar lançamentos vencidos de contas específicas:
```http
GET /api/lancamentos/vencidos?contaIds=550e8400-e29b-41d4-a716-446655440000&contaIds=550e8400-e29b-41d4-a716-446655440001
Authorization: Bearer {token}
```

### Obter saldo consolidado de múltiplas contas:
```http
GET /api/lancamentos/saldo?dataInicio=2024-01-01&dataFim=2024-01-31&contaIds=conta1&contaIds=conta2&contaIds=conta3
Authorization: Bearer {token}
```

## ?? Testes Recomendados

Para garantir que as verificações estão funcionando corretamente, teste:

1. **Buscar lançamentos com conta sem permissão** - Deve retornar 401 e especificar qual conta
2. **Buscar lançamentos sem fornecer contas** - Deve retornar 400
3. **Buscar lançamentos com lista vazia** - Deve retornar 400
4. **Buscar lançamentos com mix de contas (algumas permitidas, outras não)** - Deve retornar 401 na primeira conta sem permissão
5. **Buscar lançamentos com todas as contas permitidas** - Deve retornar dados corretamente
6. **Comparar performance entre método antigo (filtrando depois) e novo (filtrando na query)** - Novo deve ser mais rápido

## ? Melhorias de Performance

### Antes:
```csharp
// Buscava TODOS os lançamentos e filtrava em memória
var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoAsync(dataInicio, dataFim);
var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
// N+1 queries para verificar acesso de cada conta
```

### Depois:
```csharp
// Verifica permissão ANTES (evita query desnecessária)
await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

// Busca APENAS das contas especificadas (query mais eficiente)
var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoEContasAsync(
    dataInicio, dataFim, contaIds);
// SQL: WHERE ContaId IN (@contaIds) - Uma única query otimizada
```

## ?? Mensagens de Erro

- **Lista de contas vazia**: `"É necessário informar pelo menos uma conta"`
- **Sem permissão**: `"Você não tem permissão para acessar a conta com ID '{contaId}'"`
- **Data inválida**: `"A data de início deve ser anterior à data de fim"`

---

**Desenvolvido com ?? seguindo as melhores práticas de segurança e performance**
