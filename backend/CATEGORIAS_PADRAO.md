# Categorias Padr�o Inseridas no Banco de Dados

## Resumo
Foi criada uma migration que insere automaticamente categorias padr�o de receita e despesa para todas as contas existentes e futuras no sistema.

## Migration Criada
- **Nome**: `20251016201723_SeedCategoriasPadrao`
- **Localiza��o**: `ControleFinanceiro.Infrastructure\Migrations\20251016201723_SeedCategoriasPadrao.cs`

## Categorias de RECEITA (Tipo = 1)

### Destacadas (Destacada = true) - Mais Utilizadas
1. **Sal�rio** - Recebimento de sal�rio mensal (Verde #28a745)
2. **Freelance** - Trabalhos e projetos freelance (Verde-�gua #20c997)
3. **Investimentos** - Rendimentos de investimentos (Azul-claro #17a2b8)

### N�o Destacadas (Destacada = false) - Menos Utilizadas
4. **Vendas** - Receita de vendas de produtos ou servi�os (Cinza #6c757d)
5. **Bonifica��o** - B�nus e gratifica��es (Roxo #6610f2)
6. **Aluguel Recebido** - Recebimento de aluguel de im�veis (Laranja #fd7e14)
7. **Outras Receitas** - Receitas diversas (Rosa #e83e8c)

## Categorias de DESPESA (Tipo = 2)

### Destacadas (Destacada = true) - Mais Utilizadas
1. **Alimenta��o** - Supermercado e alimenta��o em geral (Amarelo #ffc107)
2. **Transporte** - Combust�vel, transporte p�blico, uber (Azul #007bff)
3. **Moradia** - Aluguel, condom�nio, IPTU (Vermelho #dc3545)
4. **Sa�de** - Plano de sa�de, medicamentos, consultas (Vermelho #dc3545)
5. **Contas de Consumo** - Luz, �gua, g�s, internet, telefone (Laranja #fd7e14)
6. **Educa��o** - Mensalidade escolar, cursos, livros (Azul-claro #17a2b8)

### N�o Destacadas (Destacada = false) - Menos Utilizadas
7. **Lazer** - Cinema, shows, viagens, hobbies (Rosa #e83e8c)
8. **Vestu�rio** - Roupas, cal�ados, acess�rios (Roxo #6610f2)
9. **Cuidados Pessoais** - Sal�o, barbearia, cosm�ticos (Laranja #fd7e14)
10. **Seguros** - Seguro de vida, carro, residencial (Cinza #6c757d)
11. **Impostos e Taxas** - Impostos diversos e taxas governamentais (Vermelho #dc3545)
12. **Presentes e Doa��es** - Presentes e contribui��es beneficentes (Rosa #e83e8c)
13. **Pets** - Veterin�rio, ra��o, acess�rios para pets (Amarelo #ffc107)
14. **Manuten��o Ve�culo** - Manuten��o e reparos do ve�culo (Azul #007bff)
15. **Assinaturas** - Netflix, Spotify, revistas e outros servi�os (Roxo #6610f2)
16. **Outras Despesas** - Despesas diversas (Cinza #6c757d)

## Total de Categorias
- **7 categorias de RECEITA** (3 destacadas + 4 n�o destacadas)
- **16 categorias de DESPESA** (6 destacadas + 10 n�o destacadas)
- **Total: 23 categorias**

## Caracter�sticas da Implementa��o

### Seguran�a
- As categorias s�o criadas apenas se ainda n�o existirem para a conta
- Usa `NOT EXISTS` para evitar duplica��es
- Utiliza `gen_random_uuid()` para gerar IDs �nicos

### Funcionalidade
- Todas as categorias s�o criadas como **ativas** (`Ativo = true`)
- As categorias incluem descri��es �teis
- Cores hexadecimais s�o aplicadas para facilitar a visualiza��o
- Suporta tanto contas existentes quanto futuras (atrav�s do SELECT em Contas)

### Reversibilidade
- A migration inclui um m�todo `Down()` que remove todas as categorias padr�o criadas
- Pode ser revertida com: `dotnet ef database update <migration-anterior>`

## Como Usar

### Aplicar a Migration (J� aplicada)
```bash
cd ControleFinanceiro.Infrastructure
dotnet ef database update --startup-project ..\ControleFinanceiro.Presentation\ControleFinanceiro.Presentation.csproj
```

### Reverter a Migration (se necess�rio)
```bash
cd ControleFinanceiro.Infrastructure
dotnet ef database update AdicionarTipoCategoriaEmCategoria --startup-project ..\ControleFinanceiro.Presentation\ControleFinanceiro.Presentation.csproj
```

## Observa��es
- Toda vez que uma nova conta � criada no sistema, ser� necess�rio executar a mesma l�gica para adicionar as categorias padr�o a ela
- Considere criar um servi�o ou m�todo no `ContaService` para adicionar automaticamente essas categorias quando uma nova conta for criada
- As categorias podem ser editadas, desativadas ou exclu�das pelos usu�rios conforme necess�rio
