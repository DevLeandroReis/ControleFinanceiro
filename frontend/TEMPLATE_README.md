# Template Padrão - Controle Financeiro

## 🎨 Visão Geral

Este projeto agora possui um template padronizado com menu lateral esquerdo (sidebar) e dashboard com dados fictícios para demonstração.

## 📁 Estrutura Criada

### 1. **Sidebar (Menu Lateral)**
Localização: `src/widgets/sidebar/`

- **Componente**: `Sidebar.tsx`
- **Estilos**: `Sidebar.css`

**Características:**
- Menu lateral fixo com gradiente roxo moderno
- Navegação para:
  - 📊 Dashboard
  - 💸 Lançamentos
  - 🏦 Contas
  - 📁 Categorias
  - 👥 Usuários
- Informações do usuário logado no rodapé
- Botão de logout
- Responsivo (colapsa em telas menores)

### 2. **AuthenticatedLayout**
Localização: `src/app/layouts/AuthenticatedLayout.tsx`

Layout especializado para rotas autenticadas que inclui:
- Sidebar à esquerda
- Área de conteúdo principal
- Background neutro (#f5f7fa)

### 3. **Dashboard com Dados Fictícios**
Localização: `src/pages/dashboard/DashboardPage.tsx`

**Componentes do Dashboard:**

#### Cards de Resumo (4 cards)
- 📈 **Receitas do Mês**: R$ 15.750,00
- 📉 **Despesas do Mês**: R$ 8.420,50
- 💰 **Saldo Total**: R$ 7.329,50
- 🏦 **Contas Ativas**: 3 contas

#### Gráfico de Evolução Mensal
- Gráfico de barras com últimos 6 meses
- Comparação visual entre receitas e despesas
- Cores distintas (roxo para receitas, rosa para despesas)

#### Receitas por Categoria
- Salário: 76%
- Freelance: 16%
- Investimentos: 8%
- Barras de progresso coloridas

#### Despesas por Categoria
- Moradia: 38%
- Alimentação: 21%
- Lazer: 14%
- Transporte: 10%
- Saúde: 8%
- Educação: 8%

#### Tabela de Atividades Recentes
- Últimas 6 transações
- Ícones de tipo (entrada/saída)
- Categorização
- Valores formatados em R$

### 4. **Roteamento Atualizado**
Localização: `src/app/router.tsx`

**Rotas Públicas** (sem sidebar):
- `/` - Home
- `/login` - Login
- `/register` - Cadastro
- `/forgot-password` - Esqueci minha senha
- `/reset-password` - Redefinir senha

**Rotas Autenticadas** (com sidebar):
- `/dashboard` - Dashboard
- `/lancamentos` - Lançamentos
- `/contas` - Contas
- `/categorias` - Categorias
- `/usuarios` - Usuários

### 5. **Página de Usuários**
Localização: `src/pages/users/UsersPage.tsx`

Página básica criada para manutenção de usuários (em desenvolvimento).

## 🎨 Design e Estilo

### Paleta de Cores
- **Sidebar**: Gradiente roxo (#667eea → #764ba2)
- **Background**: Cinza claro (#f5f7fa)
- **Cards**: Branco com sombras suaves
- **Receitas**: Gradiente roxo/azul
- **Despesas**: Gradiente rosa/vermelho
- **Saldo**: Gradiente azul/ciano
- **Contas**: Gradiente verde/ciano

### Componentes Visuais
- Cards com hover animado (elevação)
- Gráficos de barra interativos
- Barras de progresso animadas
- Tabela responsiva com hover
- Ícones emoji para melhor UX

## 📱 Responsividade

### Desktop (> 768px)
- Sidebar com 260px de largura
- Conteúdo com margem esquerda
- Layout em grid para cards

### Mobile (≤ 768px)
- Sidebar colapsa para 70px (apenas ícones)
- Textos do menu ocultados
- Cards em coluna única
- Tabela em formato vertical

## 🚀 Como Usar

### Executar em Desenvolvimento
```bash
cd frontend
npm run dev
```

### Build para Produção
```bash
cd frontend
npm run build
```

## 📊 Dados Fictícios

Os dados do dashboard são fictícios e estão hardcoded no componente para demonstração. Eles incluem:

- Resumo financeiro de Outubro/2025
- Histórico de 6 meses (Mai-Out 2025)
- Categorias de receitas e despesas realistas
- Transações de exemplo com datas recentes

## 🔄 Próximos Passos

Para conectar com o backend real, você precisará:

1. Substituir os dados fictícios por chamadas à API
2. Implementar os endpoints de relatórios no backend
3. Adicionar filtros de período (mês/ano)
4. Criar gráficos mais avançados (pizza, linhas, etc.)
5. Implementar a página de usuários completamente
6. Adicionar exportação de relatórios (PDF, Excel)

## 🎯 Endpoints do Backend Disponíveis

Baseado nos controllers encontrados:

- `api/Contas` - Gerenciamento de contas
- `api/Categorias` - Gerenciamento de categorias  
- `api/Lancamentos` - Gerenciamento de lançamentos
- `api/Usuarios` - Gerenciamento de usuários

## ✨ Funcionalidades Implementadas

✅ Menu lateral com navegação
✅ Layout autenticado com sidebar
✅ Dashboard com dados fictícios realistas
✅ Cards de resumo financeiro
✅ Gráfico de evolução mensal
✅ Gráficos de categorias (receitas e despesas)
✅ Tabela de atividades recentes
✅ Design responsivo
✅ Integração com sistema de autenticação
✅ Roteamento protegido

## 🎨 Capturas de Tela

O dashboard agora apresenta:
- Interface moderna e limpa
- Visualização clara de métricas financeiras
- Gráficos intuitivos e coloridos
- Navegação simplificada via sidebar

---

**Desenvolvido com React + TypeScript + Vite + Zustand**
