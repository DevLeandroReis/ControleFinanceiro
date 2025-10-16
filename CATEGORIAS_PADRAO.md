# Categorias Padrão Inseridas no Banco de Dados

## Resumo
Foi criada uma migration que insere automaticamente categorias padrão de receita e despesa para todas as contas existentes e futuras no sistema.

## Migration Criada
- **Nome**: `20251016201723_SeedCategoriasPadrao`
- **Localização**: `ControleFinanceiro.Infrastructure\Migrations\20251016201723_SeedCategoriasPadrao.cs`

## Categorias de RECEITA (Tipo = 1)

### Destacadas (Destacada = true) - Mais Utilizadas
1. **Salário** - Recebimento de salário mensal (Verde #28a745)
2. **Freelance** - Trabalhos e projetos freelance (Verde-água #20c997)
3. **Investimentos** - Rendimentos de investimentos (Azul-claro #17a2b8)

### Não Destacadas (Destacada = false) - Menos Utilizadas
4. **Vendas** - Receita de vendas de produtos ou serviços (Cinza #6c757d)
5. **Bonificação** - Bônus e gratificações (Roxo #6610f2)
6. **Aluguel Recebido** - Recebimento de aluguel de imóveis (Laranja #fd7e14)
7. **Outras Receitas** - Receitas diversas (Rosa #e83e8c)

## Categorias de DESPESA (Tipo = 2)

### Destacadas (Destacada = true) - Mais Utilizadas
1. **Alimentação** - Supermercado e alimentação em geral (Amarelo #ffc107)
2. **Transporte** - Combustível, transporte público, uber (Azul #007bff)
3. **Moradia** - Aluguel, condomínio, IPTU (Vermelho #dc3545)
4. **Saúde** - Plano de saúde, medicamentos, consultas (Vermelho #dc3545)
5. **Contas de Consumo** - Luz, água, gás, internet, telefone (Laranja #fd7e14)
6. **Educação** - Mensalidade escolar, cursos, livros (Azul-claro #17a2b8)

### Não Destacadas (Destacada = false) - Menos Utilizadas
7. **Lazer** - Cinema, shows, viagens, hobbies (Rosa #e83e8c)
8. **Vestuário** - Roupas, calçados, acessórios (Roxo #6610f2)
9. **Cuidados Pessoais** - Salão, barbearia, cosméticos (Laranja #fd7e14)
10. **Seguros** - Seguro de vida, carro, residencial (Cinza #6c757d)
11. **Impostos e Taxas** - Impostos diversos e taxas governamentais (Vermelho #dc3545)
12. **Presentes e Doações** - Presentes e contribuições beneficentes (Rosa #e83e8c)
13. **Pets** - Veterinário, ração, acessórios para pets (Amarelo #ffc107)
14. **Manutenção Veículo** - Manutenção e reparos do veículo (Azul #007bff)
15. **Assinaturas** - Netflix, Spotify, revistas e outros serviços (Roxo #6610f2)
16. **Outras Despesas** - Despesas diversas (Cinza #6c757d)

## Total de Categorias
- **7 categorias de RECEITA** (3 destacadas + 4 não destacadas)
- **16 categorias de DESPESA** (6 destacadas + 10 não destacadas)
- **Total: 23 categorias**

## Características da Implementação

### Segurança
- As categorias são criadas apenas se ainda não existirem para a conta
- Usa `NOT EXISTS` para evitar duplicações
- Utiliza `gen_random_uuid()` para gerar IDs únicos

### Funcionalidade
- Todas as categorias são criadas como **ativas** (`Ativo = true`)
- As categorias incluem descrições úteis
- Cores hexadecimais são aplicadas para facilitar a visualização
- Suporta tanto contas existentes quanto futuras (através do SELECT em Contas)

### Reversibilidade
- A migration inclui um método `Down()` que remove todas as categorias padrão criadas
- Pode ser revertida com: `dotnet ef database update <migration-anterior>`

## Como Usar

### Aplicar a Migration (Já aplicada)
```bash
cd ControleFinanceiro.Infrastructure
dotnet ef database update --startup-project ..\ControleFinanceiro.Presentation\ControleFinanceiro.Presentation.csproj
```

### Reverter a Migration (se necessário)
```bash
cd ControleFinanceiro.Infrastructure
dotnet ef database update AdicionarTipoCategoriaEmCategoria --startup-project ..\ControleFinanceiro.Presentation\ControleFinanceiro.Presentation.csproj
```

## Observações
- Toda vez que uma nova conta é criada no sistema, será necessário executar a mesma lógica para adicionar as categorias padrão a ela
- Considere criar um serviço ou método no `ContaService` para adicionar automaticamente essas categorias quando uma nova conta for criada
- As categorias podem ser editadas, desativadas ou excluídas pelos usuários conforme necessário
