# Controle Financeiro API

Uma API RESTful para controle financeiro pessoal implementada seguindo os princ�pios do Domain-Driven Design (DDD) e Clean Architecture.

## ??? Arquitetura

O projeto est� organizado seguindo os princ�pios do DDD e Clean Architecture com **4 projetos separados**:

```
ControleFinanceiro/
??? ControleFinanceiro.Domain/           # Camada de Dom�nio
?   ??? Entities/                        # Entidades do neg�cio
?   ?   ??? BaseEntity.cs
?   ?   ??? Categoria.cs
?   ?   ??? Lancamento.cs
?   ?   ??? Usuario.cs
?   ?   ??? Conta.cs
?   ??? Enums/                           # Enumera��es
?   ?   ??? StatusLancamento.cs
?   ?   ??? TipoLancamento.cs
?   ?   ??? TipoRecorrencia.cs
?   ??? Interfaces/                      # Interfaces de reposit�rio
?       ??? Repositories/
??? ControleFinanceiro.Application/      # Camada de Aplica��o
?   ??? DTOs/                            # Objetos de transfer�ncia
?   ??? Services/                        # Servi�os de aplica��o
?   ??? Interfaces/                      # Interfaces dos servi�os
?   ??? Mappings/                        # Perfis do AutoMapper
??? ControleFinanceiro.Infrastructure/   # Camada de Infraestrutura
?   ??? Data/                            # Acesso a dados
?   ?   ??? Configurations/              # Configura��es EF Core
?   ?   ??? Repositories/                # Implementa��o reposit�rios
?   ?   ??? ApplicationDbContext.cs
?   ??? Migrations/                      # Migra��es do banco
??? ControleFinanceiro.Presentation/     # Camada de Apresenta��o (API)
    ??? Controllers/                     # Controladores da API
    ??? Extensions/                      # Extens�es �teis
    ??? Program.cs                       # Ponto de entrada
```

### ?? ControleFinanceiro.Domain
**N�o possui depend�ncias externas**
- **Entities**: Entidades principais do neg�cio (`Categoria`, `Lancamento`, `Usuario`, `Conta`)
- **Enums**: Enumera��es (`TipoLancamento`, `StatusLancamento`, `TipoRecorrencia`)
- **Interfaces**: Contratos dos reposit�rios

### ?? ControleFinanceiro.Infrastructure
**Depend�ncias**: Domain, Application
- **Data**: Contexto do Entity Framework e configura��es
- **Repositories**: Implementa��o dos reposit�rios
- **Migrations**: Migra��es do Entity Framework

### ?? ControleFinanceiro.Application
**Depend�ncias**: Domain
- **DTOs**: Objetos de transfer�ncia de dados
- **Services**: Servi�os de aplica��o com regras de neg�cio
- **Mappings**: Perfis do AutoMapper
- **Interfaces**: Contratos dos servi�os

### ?? ControleFinanceiro.Presentation
**Depend�ncias**: Domain, Application, Infrastructure
- **Controllers**: Controladores da API Web
- **Extensions**: M�todos de extens�o (ex: ClaimsPrincipal)
- **Program.cs**: Configura��o e inje��o de depend�ncias

## ?? Tecnologias Utilizadas

- **.NET 8**: Framework principal
- **Entity Framework Core**: ORM para acesso a dados
- **PostgreSQL**: Banco de dados
- **AutoMapper**: Mapeamento de objetos
- **Swagger/OpenAPI**: Documenta��o da API

## ?? Funcionalidades

### Categorias
- ? CRUD completo de categorias
- ? Listagem de categorias ativas
- ? Busca por nome
- ? Valida��o de nomes �nicos

### Lan�amentos (Receitas e Despesas)
- ? CRUD completo de lan�amentos
- ? Suporte a lan�amentos recorrentes
- ? Controle de status (Pendente, Pago, Cancelado, Vencido)
- ? Relat�rios financeiros por per�odo
- ? C�lculo de saldo, receitas e despesas totais
- ? Filtros por categoria, tipo, status e per�odo

## ?? Configura��o e Instala��o

### Pr�-requisitos
- .NET 8 SDK
- PostgreSQL
- Visual Studio 2022 ou VS Code

### Passo a passo

1. **Clone o reposit�rio**
```bash
git clone <repository-url>
cd ControleFinanceiro
```

2. **Configure a string de conex�o**
Edite o arquivo `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ControleFinanceiro;Username=SEU_USUARIO;Password=SUA_SENHA;Port=5432"
  }
}
```

3. **Instale as ferramentas do EF Core** (se n�o tiver)
```bash
dotnet tool install --global dotnet-ef
```

4. **Execute as migra��es**
```bash
dotnet ef database update
```

5. **Execute a aplica��o**
```bash
dotnet run
```

6. **Acesse a documenta��o**
- Swagger UI: `https://localhost:5001` ou `http://localhost:5000`

## ?? Endpoints da API

### Categorias

| M�todo | Endpoint | Descri��o |
|--------|----------|-----------|
| GET | `/api/categorias` | Lista todas as categorias |
| GET | `/api/categorias/ativas` | Lista apenas categorias ativas |
| GET | `/api/categorias/{id}` | Obter categoria por ID |
| GET | `/api/categorias/nome/{nome}` | Obter categoria por nome |
| POST | `/api/categorias` | Criar nova categoria |
| PUT | `/api/categorias/{id}` | Atualizar categoria |
| DELETE | `/api/categorias/{id}` | Excluir categoria |

### Lan�amentos

| M�todo | Endpoint | Descri��o |
|--------|----------|-----------|
| GET | `/api/lancamentos` | Lista todos os lan�amentos |
| GET | `/api/lancamentos/{id}` | Obter lan�amento por ID |
| GET | `/api/lancamentos/periodo?dataInicio={data}&dataFim={data}` | Lan�amentos por per�odo |
| GET | `/api/lancamentos/categoria/{id}` | Lan�amentos por categoria |
| GET | `/api/lancamentos/tipo/{tipo}` | Lan�amentos por tipo (Receita/Despesa) |
| GET | `/api/lancamentos/status/{status}` | Lan�amentos por status |
| GET | `/api/lancamentos/vencidos` | Lan�amentos vencidos |
| GET | `/api/lancamentos/recorrentes` | Lan�amentos recorrentes |
| GET | `/api/lancamentos/saldo?dataInicio={data}&dataFim={data}` | Saldo por per�odo |
| GET | `/api/lancamentos/receitas/total?dataInicio={data}&dataFim={data}` | Total de receitas |
| GET | `/api/lancamentos/despesas/total?dataInicio={data}&dataFim={data}` | Total de despesas |
| POST | `/api/lancamentos` | Criar novo lan�amento |
| POST | `/api/lancamentos/recorrentes` | Criar lan�amentos recorrentes |
| PUT | `/api/lancamentos/{id}` | Atualizar lan�amento |
| DELETE | `/api/lancamentos/{id}` | Excluir lan�amento |
| PATCH | `/api/lancamentos/{id}/pagar` | Marcar como pago |
| PATCH | `/api/lancamentos/{id}/pendente` | Marcar como pendente |
| PATCH | `/api/lancamentos/{id}/cancelar` | Cancelar lan�amento |

## ?? Estrutura dos DTOs

### CreateCategoriaDto
```json
{
  "nome": "string (obrigat�rio, max: 100)",
  "descricao": "string (opcional, max: 500)",
  "cor": "string (opcional, formato hex: #FFFFFF)"
}
```

### CreateLancamentoDto
```json
{
  "descricao": "string (obrigat�rio, max: 200)",
  "valor": "decimal (obrigat�rio, > 0)",
  "dataVencimento": "datetime (obrigat�rio)",
  "tipo": "int (1=Receita, 2=Despesa)",
  "observacoes": "string (opcional, max: 1000)",
  "ehRecorrente": "boolean",
  "tipoRecorrencia": "int (0=Nenhuma, 1=Di�ria, 2=Semanal, 3=Mensal, 4=Anual)",
  "quantidadeParcelas": "int (opcional)",
  "categoriaId": "guid (obrigat�rio)"
}
```

## ?? Principais Padr�es Implementados

- **Domain-Driven Design (DDD)**: Organiza��o focada no dom�nio do neg�cio
- **Clean Architecture**: Separa��o clara de responsabilidades
- **Repository Pattern**: Abstra��o do acesso a dados
- **Service Layer**: L�gica de neg�cio centralizada
- **DTO Pattern**: Transfer�ncia segura de dados
- **AutoMapper**: Mapeamento autom�tico de objetos
- **Soft Delete**: Exclus�o l�gica de registros

## ?? Observa��es Importantes

- A API implementa **Soft Delete**, ou seja, os registros n�o s�o removidos fisicamente do banco
- Lan�amentos recorrentes criam m�ltiplos registros baseados no tipo de recorr�ncia
- O sistema calcula automaticamente se um lan�amento est� vencido
- Todas as datas s�o armazenadas em UTC
- A valida��o � feita tanto no n�vel de DTO quanto no dom�nio

## ?? Contribui��o

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudan�as (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request