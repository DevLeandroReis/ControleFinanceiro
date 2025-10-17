# 🧭 React Router - Documentação

React Router foi adicionado ao projeto para gerenciar a navegação entre páginas.

## 📦 Instalação

```bash
npm install react-router-dom
```

**Versão instalada:** `react-router-dom@^7.1.3`

---

## 🗺️ Estrutura de Rotas

### Arquivo de Configuração: `src/app/router.tsx`

```typescript
import { createBrowserRouter } from 'react-router-dom';

export const router = createBrowserRouter([
  {
    path: '/',
    element: <RootLayout />,
    errorElement: <NotFoundPage />,
    children: [
      { index: true, element: <HomePage /> },
      { path: 'dashboard', element: <DashboardPage /> },
      { path: 'transacoes', element: <TransactionsPage /> },
      { path: 'contas', element: <AccountsPage /> },
      { path: 'categorias', element: <CategoriesPage /> },
    ],
  },
]);
```

### Rotas Disponíveis

| Rota | Componente | Descrição |
|------|-----------|-----------|
| `/` | `HomePage` | Página inicial com cards de features |
| `/dashboard` | `DashboardPage` | Dashboard com visão geral financeira |
| `/transacoes` | `TransactionsPage` | Gerenciamento de lançamentos |
| `/contas` | `AccountsPage` | Gerenciamento de contas bancárias |
| `/categorias` | `CategoriesPage` | Gerenciamento de categorias |
| `*` (404) | `NotFoundPage` | Página de erro 404 |

---

## 📐 Layout Principal

### `src/app/layouts/RootLayout.tsx`

Layout raiz que envolve todas as páginas, contendo:
- **Navigation** - Barra de navegação superior
- **Outlet** - Renderiza a página ativa

```typescript
export const RootLayout: FC = () => {
  return (
    <div className="root-layout">
      <Navigation />
      <main className="main-content">
        <Outlet />
      </main>
    </div>
  );
};
```

---

## 🧭 Componente de Navegação

### `src/widgets/navigation/Navigation.tsx`

Menu de navegação responsivo com links para todas as páginas.

**Features:**
- ✅ Links ativos destacados automaticamente
- ✅ Responsivo (mobile-friendly)
- ✅ Usa `NavLink` para navegação com classe `active`
- ✅ Design moderno com ícones

```typescript
<NavLink 
  to="/dashboard" 
  className={({ isActive }) => isActive ? 'nav-link active' : 'nav-link'}
>
  📊 Dashboard
</NavLink>
```

---

## 📄 Páginas Criadas

### 1. **HomePage** (`/`)
- Página inicial de boas-vindas
- Cards clicáveis com links para outras seções
- Layout grid responsivo

### 2. **DashboardPage** (`/dashboard`)
- Visão geral das finanças
- Cards de métricas (Receitas, Despesas, Saldo, Contas)
- Seção de atividades recentes

### 3. **TransactionsPage** (`/transacoes`)
- Lista de lançamentos financeiros
- Botão para adicionar nova transação
- Filtros por categoria e tipo

### 4. **AccountsPage** (`/contas`)
- Gerenciamento de contas bancárias
- Botão para adicionar nova conta
- Grid de contas

### 5. **CategoriesPage** (`/categorias`)
- Gerenciamento de categorias
- Botão para adicionar nova categoria
- Grid de categorias

### 6. **NotFoundPage** (404)
- Página de erro 404
- Tratamento de rotas não encontradas
- Link para voltar à home

---

## 🎨 Navegação Programática

### Usando `useNavigate` hook

```typescript
import { useNavigate } from 'react-router-dom';

const MyComponent = () => {
  const navigate = useNavigate();

  const handleClick = () => {
    navigate('/dashboard');
  };

  return <button onClick={handleClick}>Ir para Dashboard</button>;
};
```

### Usando componente `Link`

```typescript
import { Link } from 'react-router-dom';

<Link to="/transacoes" className="btn">
  Ver Transações
</Link>
```

### Usando componente `NavLink` (com active state)

```typescript
import { NavLink } from 'react-router-dom';

<NavLink 
  to="/dashboard" 
  className={({ isActive }) => isActive ? 'active' : ''}
>
  Dashboard
</NavLink>
```

---

## 🔄 Hooks do React Router

### `useNavigate`
Navega programaticamente para outras rotas.

```typescript
const navigate = useNavigate();
navigate('/dashboard');
navigate(-1); // Voltar
```

### `useLocation`
Obtém informações sobre a rota atual.

```typescript
const location = useLocation();
console.log(location.pathname); // "/dashboard"
```

### `useParams`
Obtém parâmetros da URL.

```typescript
// Rota: /transacoes/:id
const { id } = useParams();
```

### `useSearchParams`
Gerencia query parameters.

```typescript
const [searchParams, setSearchParams] = useSearchParams();
const filter = searchParams.get('filter');
```

---

## 🎯 Próximas Melhorias Sugeridas

### 1. **Rotas Protegidas**
Adicionar autenticação e proteção de rotas:

```typescript
const ProtectedRoute = ({ children }: { children: ReactNode }) => {
  const isAuthenticated = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" />;
};
```

### 2. **Rotas com Parâmetros**
Implementar rotas dinâmicas:

```typescript
{
  path: 'transacoes/:id',
  element: <TransactionDetailPage />,
}
```

### 3. **Lazy Loading**
Carregar páginas sob demanda:

```typescript
const DashboardPage = lazy(() => import('@pages/dashboard'));

<Suspense fallback={<Loading />}>
  <DashboardPage />
</Suspense>
```

### 4. **Breadcrumbs**
Adicionar navegação breadcrumb:

```typescript
<nav aria-label="breadcrumb">
  <Link to="/">Home</Link> / 
  <Link to="/dashboard">Dashboard</Link>
</nav>
```

### 5. **Scroll to Top**
Rolar para o topo ao mudar de página:

```typescript
const ScrollToTop = () => {
  const { pathname } = useLocation();
  
  useEffect(() => {
    window.scrollTo(0, 0);
  }, [pathname]);
  
  return null;
};
```

---

## 📚 Documentação Oficial

- [React Router Docs](https://reactrouter.com/)
- [React Router Tutorial](https://reactrouter.com/en/main/start/tutorial)
- [API Reference](https://reactrouter.com/en/main/routers/create-browser-router)

---

## ✅ Checklist de Implementação

- [x] Instalar `react-router-dom`
- [x] Criar estrutura de rotas
- [x] Implementar `RootLayout` com `Outlet`
- [x] Criar componente `Navigation`
- [x] Criar páginas (Home, Dashboard, Transactions, Accounts, Categories)
- [x] Implementar página 404 (NotFound)
- [x] Adicionar navegação com `NavLink` e estados ativos
- [x] Integrar router no `AppProvider`
- [ ] Adicionar rotas protegidas (autenticação)
- [ ] Implementar lazy loading
- [ ] Adicionar parâmetros de rota
- [ ] Criar breadcrumbs

---

**Status:** ✅ React Router configurado e funcional!

O projeto agora possui navegação completa entre páginas com menu responsivo e rotas organizadas seguindo o padrão FSD.
