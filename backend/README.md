# Controle Financeiro API

Uma API RESTful para controle financeiro pessoal implementada seguindo os princípios do Domain-Driven Design (DDD) e Clean Architecture.

## ??? Arquitetura

O projeto está organizado seguindo os princípios do DDD e Clean Architecture com **4 projetos separados**:

```
ControleFinanceiro/
??? ControleFinanceiro.Domain/           # Camada de Domínio
?   ??? Entities/                        # Entidades do negócio
?   ?   ??? BaseEntity.cs
?   ?   ??? Categoria.cs
?   ?   ??? Lancamento.cs
?   ?   ??? Usuario.cs
?   ?   ??? Conta.cs
?   ??? Enums/                           # Enumerações
?   ?   ??? StatusLancamento.cs
?   ?   ??? TipoLancamento.cs
?   ?   ??? TipoRecorrencia.cs
?   ??? Interfaces/                      # Interfaces de repositório
?       ??? Repositories/
??? ControleFinanceiro.Application/      # Camada de Aplicação
?   ??? DTOs/                            # Objetos de transferência
?   ??? Services/                        # Serviços de aplicação
?   ??? Interfaces/                      # Interfaces dos serviços
?   ??? Mappings/                        # Perfis do AutoMapper
??? ControleFinanceiro.Infrastructure/   # Camada de Infraestrutura
?   ??? Data/                            # Acesso a dados
?   ?   ??? Configurations/              # Configurações EF Core
?   ?   ??? Repositories/                # Implementação repositórios
?   ?   ??? ApplicationDbContext.cs
?   ??? Migrations/                      # Migrações do banco
??? ControleFinanceiro.Presentation/     # Camada de Apresentação (API)
    ??? Controllers/                     # Controladores da API
    ??? Extensions/                      # Extensões úteis
    ??? Program.cs                       # Ponto de entrada
```

### ?? ControleFinanceiro.Domain
**Não possui dependências externas**
- **Entities**: Entidades principais do negócio (`Categoria`, `Lancamento`, `Usuario`, `Conta`)
- **Enums**: Enumerações (`TipoLancamento`, `StatusLancamento`, `TipoRecorrencia`)
- **Interfaces**: Contratos dos repositórios

### ?? ControleFinanceiro.Infrastructure
**Dependências**: Domain, Application
- **Data**: Contexto do Entity Framework e configurações
- **Repositories**: Implementação dos repositórios
- **Migrations**: Migrações do Entity Framework

### ?? ControleFinanceiro.Application
**Dependências**: Domain
- **DTOs**: Objetos de transferência de dados
- **Services**: Serviços de aplicação com regras de negócio
- **Mappings**: Perfis do AutoMapper
- **Interfaces**: Contratos dos serviços

### ?? ControleFinanceiro.Presentation
**Dependências**: Domain, Application, Infrastructure
- **Controllers**: Controladores da API Web
- **Extensions**: Métodos de extensão (ex: ClaimsPrincipal)
- **Program.cs**: Configuração e injeção de dependências

## ?? Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **PostgreSQL**: Banco de dados
- **AutoMapper**: Mapeamento de objetos
- **Swagger/OpenAPI**: Documentação da API

## ?? Funcionalidades

### Categorias
- ? CRUD completo de categorias
- ? Listagem de categorias ativas
- ? Busca por nome
- ? Validação de nomes únicos

### Lançamentos (Receitas e Despesas)
- ? CRUD completo de lançamentos
- ? Suporte a lançamentos recorrentes
- ? Controle de status (Pendente, Pago, Cancelado, Vencido)
- ? Relatórios financeiros por período
- ? Cálculo de saldo, receitas e despesas totais
- ? Filtros por categoria, tipo, status e período

## ?? Configuração e Instalação

### Pré-requisitos
- .NET 8 SDK
- PostgreSQL
- Visual Studio 2022 ou VS Code

### Passo a passo

1. **Clone o repositório**
```bash
git clone <repository-url>
cd ControleFinanceiro
```

2. **Configure a string de conexão**
Edite o arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ControleFinanceiro;Username=SEU_USUARIO;Password=SUA_SENHA;Port=5432"
  }
}
```

3. **Instale as ferramentas do EF Core** (se não tiver)
```bash
dotnet tool install --global dotnet-ef
```

4. **Execute as migrações**
```bash
dotnet ef database update
```

5. **Execute a aplicação**
```bash
dotnet run
```

6. **Acesse a documentação**
- Swagger UI: `https://localhost:5001` ou `http://localhost:5000`

## ?? Endpoints da API

### Categorias

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/categorias` | Lista todas as categorias |
| GET | `/api/categorias/ativas` | Lista apenas categorias ativas |
| GET | `/api/categorias/{id}` | Obter categoria por ID |
| GET | `/api/categorias/nome/{nome}` | Obter categoria por nome |
| POST | `/api/categorias` | Criar nova categoria |
| PUT | `/api/categorias/{id}` | Atualizar categoria |
| DELETE | `/api/categorias/{id}` | Excluir categoria |

### Lançamentos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/lancamentos` | Lista todos os lançamentos |
| GET | `/api/lancamentos/{id}` | Obter lançamento por ID |
| GET | `/api/lancamentos/periodo?dataInicio={data}&dataFim={data}` | Lançamentos por período |
| GET | `/api/lancamentos/categoria/{id}` | Lançamentos por categoria |
| GET | `/api/lancamentos/tipo/{tipo}` | Lançamentos por tipo (Receita/Despesa) |
| GET | `/api/lancamentos/status/{status}` | Lançamentos por status |
| GET | `/api/lancamentos/vencidos` | Lançamentos vencidos |
| GET | `/api/lancamentos/recorrentes` | Lançamentos recorrentes |
| GET | `/api/lancamentos/saldo?dataInicio={data}&dataFim={data}` | Saldo por período |
| GET | `/api/lancamentos/receitas/total?dataInicio={data}&dataFim={data}` | Total de receitas |
| GET | `/api/lancamentos/despesas/total?dataInicio={data}&dataFim={data}` | Total de despesas |
| POST | `/api/lancamentos` | Criar novo lançamento |
| POST | `/api/lancamentos/recorrentes` | Criar lançamentos recorrentes |
| PUT | `/api/lancamentos/{id}` | Atualizar lançamento |
| DELETE | `/api/lancamentos/{id}` | Excluir lançamento |
| PATCH | `/api/lancamentos/{id}/pagar` | Marcar como pago |
| PATCH | `/api/lancamentos/{id}/pendente` | Marcar como pendente |
| PATCH | `/api/lancamentos/{id}/cancelar` | Cancelar lançamento |

## ?? Estrutura dos DTOs

### CreateCategoriaDto
```json
{
  "nome": "string (obrigatório, max: 100)",
  "descricao": "string (opcional, max: 500)",
  "cor": "string (opcional, formato hex: #FFFFFF)"
}
```

### CreateLancamentoDto
```json
{
  "descricao": "string (obrigatório, max: 200)",
  "valor": "decimal (obrigatório, > 0)",
  "dataVencimento": "datetime (obrigatório)",
  "tipo": "int (1=Receita, 2=Despesa)",
  "observacoes": "string (opcional, max: 1000)",
  "ehRecorrente": "boolean",
  "tipoRecorrencia": "int (0=Nenhuma, 1=Diária, 2=Semanal, 3=Mensal, 4=Anual)",
  "quantidadeParcelas": "int (opcional)",
  "categoriaId": "guid (obrigatório)"
}
```

## ?? Principais Padrões Implementados

- **Domain-Driven Design (DDD)**: Organização focada no domínio do negócio
- **Clean Architecture**: Separação clara de responsabilidades
- **Repository Pattern**: Abstração do acesso a dados
- **Service Layer**: Lógica de negócio centralizada
- **DTO Pattern**: Transferência segura de dados
- **AutoMapper**: Mapeamento automático de objetos
- **Soft Delete**: Exclusão lógica de registros

## ?? Observações Importantes

- A API implementa **Soft Delete**, ou seja, os registros não são removidos fisicamente do banco
- Lançamentos recorrentes criam múltiplos registros baseados no tipo de recorrência
- O sistema calcula automaticamente se um lançamento está vencido
- Todas as datas são armazenadas em UTC
- A validação é feita tanto no nível de DTO quanto no domínio

## ?? Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request