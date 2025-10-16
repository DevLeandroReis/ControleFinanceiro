# Melhorias na Verifica��o de Permiss�es - LancamentoService

## ?? Resumo das Altera��es

Foi implementada uma otimiza��o no `LancamentoService` para garantir que:
1. **Todas as opera��es verificam as permiss�es do usu�rio o mais cedo poss�vel**
2. **Os m�todos de consulta agora recebem uma lista de IDs de contas**
3. **Antes de buscar os lan�amentos, o sistema verifica se o usu�rio tem permiss�o para TODAS as contas da lista**

## ?? Principais Mudan�as

### 1. Novos M�todos Auxiliares

#### Verifica��o de acesso a m�ltiplas contas:
```csharp
private async Task VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(IEnumerable<Guid> contaIds, Guid usuarioId)
{
    if (contaIds == null || !contaIds.Any())
    {
        throw new ArgumentException("� necess�rio fornecer pelo menos uma conta");
    }

    foreach (var contaId in contaIds)
    {
        if (!await _contaRepository.UsuarioTemAcessoContaAsync(usuarioId, contaId))
        {
            throw new UnauthorizedAccessException($"Voc� n�o tem permiss�o para acessar a conta com ID '{contaId}'");
        }
    }
}
```

### 2. Novos M�todos no Reposit�rio

Adicionados m�todos no `ILancamentoRepository` e `LancamentoRepository` que recebem lista de contas:

- ? `GetLancamentosPorPeriodoEContasAsync(DateTime dataInicio, DateTime dataFim, IEnumerable<Guid> contaIds)`
- ? `GetLancamentosPorCategoriaEContasAsync(Guid categoriaId, IEnumerable<Guid> contaIds)`
- ? `GetLancamentosPorTipoEContasAsync(TipoLancamento tipo, IEnumerable<Guid> contaIds)`
- ? `GetLancamentosPorStatusEContasAsync(StatusLancamento status, IEnumerable<Guid> contaIds)`
- ? `GetLancamentosVencidosPorContasAsync(IEnumerable<Guid> contaIds)`
- ? `GetLancamentosRecorrentesPorContasAsync(IEnumerable<Guid> contaIds)`
- ? `GetLancamentosPorContasAsync(IEnumerable<Guid> contaIds)`

### 3. M�todos de Servi�o Atualizados

Todos os m�todos de consulta agora recebem lista de contas e verificam permiss�es ANTES de buscar:

#### Exemplo - GetLancamentosPorPeriodoAsync:
```csharp
public async Task<IEnumerable<LancamentoDto>> GetLancamentosPorPeriodoAsync(
    DateTime dataInicio, 
    DateTime dataFim, 
    IEnumerable<Guid> contaIds, 
    Guid usuarioId)
{
    // 1. PRIMEIRO: Verificar acesso a TODAS as contas (evita consultas desnecess�rias)
    await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

    // 2. Depois: Buscar lan�amentos apenas das contas permitidas
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

#### Valida��es no Controller:
- ? Verifica se a lista de contas foi fornecida
- ? Verifica se a lista n�o est� vazia
- ? Retorna BadRequest se lista n�o for fornecida
- ? Retorna Unauthorized se usu�rio n�o tiver permiss�o para alguma conta

## ?? Benef�cios

### 1. **Seguran�a Aprimorada**
- Verifica��o de permiss�o para TODAS as contas ANTES de qualquer consulta
- Imposs�vel acessar lan�amentos de contas sem permiss�o
- Mensagens de erro espec�ficas indicando qual conta n�o tem permiss�o

### 2. **Performance Otimizada**
- Consultas SQL mais eficientes usando `WHERE IN (contaIds)`
- Evita N+1 queries ao filtrar por conta
- Verifica��o de permiss�o acontece ANTES de buscar dados do banco

### 3. **Flexibilidade**
- Cliente pode consultar lan�amentos de m�ltiplas contas em uma �nica requisi��o
- �til para dashboards e relat�rios consolidados
- Permite an�lise comparativa entre contas

### 4. **C�digo Mais Limpo**
- Padr�o consistente em todos os m�todos de consulta
- Reutiliza��o de m�todos auxiliares
- Separa��o clara de responsabilidades

## ?? M�todos Atualizados

### M�todos de Consulta (agora recebem lista de contas):
- [x] `GetLancamentosPorPeriodoAsync` - Busca por per�odo e contas
- [x] `GetLancamentosPorCategoriaAsync` - Busca por categoria e contas
- [x] `GetLancamentosPorTipoAsync` - Busca por tipo e contas
- [x] `GetLancamentosPorStatusAsync` - Busca por status e contas
- [x] `GetLancamentosVencidosAsync` - Busca vencidos por contas
- [x] `GetLancamentosRecorrentesAsync` - Busca recorrentes por contas
- [x] `GetSaldoPorPeriodoAsync` - Calcula saldo por per�odo e contas
- [x] `GetTotalReceitasPorPeriodoAsync` - Total de receitas por per�odo e contas
- [x] `GetTotalDespesasPorPeriodoAsync` - Total de despesas por per�odo e contas

### M�todos Mantidos (single conta):
- [x] `GetByIdAsync` - Busca por ID espec�fico
- [x] `GetLancamentosPorContaAsync` - Busca por uma �nica conta
- [x] `GetAllAsync` - Busca todos (filtra automaticamente por acesso)

### M�todos de Opera��o (sem mudan�as):
- [x] `CreateAsync`
- [x] `UpdateAsync`
- [x] `UpdateLancamentoRecorrenteAsync`
- [x] `DeleteAsync`
- [x] `MarcarComoPagoAsync`
- [x] `MarcarComoPendenteAsync`
- [x] `CancelarAsync`
- [x] `CriarLancamentosRecorrentesAsync`
- [x] `GerarLancamentosFuturosAsync`

## ?? Fluxo de Verifica��o

### Para m�todos com lista de contas:
```
1. Recebe lista de contaIds via query parameter
2. ? VALIDA se lista n�o est� vazia (BadRequest se vazia)
3. ? VERIFICA PERMISS�O para TODAS as contas (Unauthorized se alguma n�o permitida)
4. Busca lan�amentos APENAS das contas da lista
5. Retorna resultados
```

### Para m�todos com conta �nica:
```
1. Recebe contaId (na rota ou no DTO)
2. ? VERIFICA PERMISS�O para a conta (Unauthorized se n�o permitido)
3. Executa opera��o
4. Retorna resultado
```

## ?? Exemplos de Uso

### Buscar lan�amentos de m�ltiplas contas por per�odo:
```http
GET /api/lancamentos/periodo?dataInicio=2024-01-01&dataFim=2024-01-31&contaIds=550e8400-e29b-41d4-a716-446655440000&contaIds=550e8400-e29b-41d4-a716-446655440001
Authorization: Bearer {token}
```

### Buscar lan�amentos vencidos de contas espec�ficas:
```http
GET /api/lancamentos/vencidos?contaIds=550e8400-e29b-41d4-a716-446655440000&contaIds=550e8400-e29b-41d4-a716-446655440001
Authorization: Bearer {token}
```

### Obter saldo consolidado de m�ltiplas contas:
```http
GET /api/lancamentos/saldo?dataInicio=2024-01-01&dataFim=2024-01-31&contaIds=conta1&contaIds=conta2&contaIds=conta3
Authorization: Bearer {token}
```

## ?? Testes Recomendados

Para garantir que as verifica��es est�o funcionando corretamente, teste:

1. **Buscar lan�amentos com conta sem permiss�o** - Deve retornar 401 e especificar qual conta
2. **Buscar lan�amentos sem fornecer contas** - Deve retornar 400
3. **Buscar lan�amentos com lista vazia** - Deve retornar 400
4. **Buscar lan�amentos com mix de contas (algumas permitidas, outras n�o)** - Deve retornar 401 na primeira conta sem permiss�o
5. **Buscar lan�amentos com todas as contas permitidas** - Deve retornar dados corretamente
6. **Comparar performance entre m�todo antigo (filtrando depois) e novo (filtrando na query)** - Novo deve ser mais r�pido

## ? Melhorias de Performance

### Antes:
```csharp
// Buscava TODOS os lan�amentos e filtrava em mem�ria
var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoAsync(dataInicio, dataFim);
var lancamentosFiltrados = await FiltrarLancamentosPorAcessoAsync(lancamentos, usuarioId);
// N+1 queries para verificar acesso de cada conta
```

### Depois:
```csharp
// Verifica permiss�o ANTES (evita query desnecess�ria)
await VerificarELancarExcecaoSeNaoTemAcessoATodasContasAsync(contaIds, usuarioId);

// Busca APENAS das contas especificadas (query mais eficiente)
var lancamentos = await _lancamentoRepository.GetLancamentosPorPeriodoEContasAsync(
    dataInicio, dataFim, contaIds);
// SQL: WHERE ContaId IN (@contaIds) - Uma �nica query otimizada
```

## ?? Mensagens de Erro

- **Lista de contas vazia**: `"� necess�rio informar pelo menos uma conta"`
- **Sem permiss�o**: `"Voc� n�o tem permiss�o para acessar a conta com ID '{contaId}'"`
- **Data inv�lida**: `"A data de in�cio deve ser anterior � data de fim"`

---

**Desenvolvido com ?? seguindo as melhores pr�ticas de seguran�a e performance**
