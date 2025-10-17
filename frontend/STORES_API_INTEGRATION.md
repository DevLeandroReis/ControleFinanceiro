# Stores Atualizados - Integração com API

## 📋 Resumo das Mudanças

Todos os 4 stores do Zustand foram refatorados para usar os serviços de API do Axios em vez de dados locais mockados. Agora os stores fazem requisições HTTP reais para o backend.

---

## 🔄 Stores Atualizados

### 1. **TransactionStore** (`entities/transaction/model/store.ts`)

#### ✅ Actions Atualizadas
- `fetchTransactions()` - Busca todas transações da API
- `addTransaction(data)` - Cria nova transação via POST
- `updateTransaction(id, data)` - Atualiza transação via PUT
- `deleteTransaction(id)` - Remove transação via DELETE

#### 🔧 Mudanças Técnicas
- Removido `crypto.randomUUID()` - ID agora vem do backend
- Adicionado tratamento de erro com `try/catch`
- Estados de loading durante requisições
- Throws de erro para propagação ao componente

#### 📝 Exemplo de Uso
```typescript
const { transactions, isLoading, error, fetchTransactions, addTransaction } = 
  useTransactionStore();

// Carregar transações ao montar
useEffect(() => {
  fetchTransactions();
}, []);

// Adicionar nova transação
const handleSubmit = async (data: CreateTransactionDTO) => {
  try {
    await addTransaction(data);
    alert('Transação criada!');
  } catch (error) {
    alert('Erro ao criar transação');
  }
};
```

---

### 2. **AccountStore** (`entities/account/model/store.ts`)

#### ✅ Actions Atualizadas
- `fetchAccounts()` - Busca todas contas da API
- `addAccount(data)` - Cria nova conta via POST
- `updateAccount(id, data)` - Atualiza conta via PUT
- `deleteAccount(id)` - Remove conta via DELETE

#### 🔧 Mudanças Técnicas
- Removido `crypto.randomUUID()` e `saldoAtual` calculado localmente
- Backend agora gerencia saldo da conta
- Mantido `getTotalBalance()` para cálculo no frontend
- Persistência com `persist` middleware mantida

#### 📝 Exemplo de Uso
```typescript
const { accounts, fetchAccounts, addAccount, getTotalBalance } = 
  useAccountStore();

useEffect(() => {
  fetchAccounts();
}, []);

const totalBalance = getTotalBalance(); // Soma de saldos localmente
```

---

### 3. **CategoryStore** (`entities/category/model/store.ts`)

#### ✅ Actions Atualizadas
- `fetchCategories()` - Busca todas categorias da API
- `addCategory(data)` - Cria nova categoria via POST
- `updateCategory(id, data)` - Atualiza categoria via PUT
- `deleteCategory(id)` - Remove categoria via DELETE
- `createDefaultCategories()` - Cria categorias padrão (novo!)

#### 🔧 Mudanças Técnicas
- Adicionada action `createDefaultCategories()` para setup inicial
- `getCategoriesByType()` mantido como helper local
- Filtros por tipo agora disponíveis via API também

#### 📝 Exemplo de Uso
```typescript
const { categories, fetchCategories, createDefaultCategories, getCategoriesByType } = 
  useCategoryStore();

// Setup inicial
const handleSetup = async () => {
  await createDefaultCategories();
  await fetchCategories();
};

// Filtrar por tipo (local)
const incomeCategories = getCategoriesByType('income');
```

---

### 4. **UserStore** (`entities/user/model/store.ts`)

#### ✅ Actions Atualizadas
- `login(data)` - Autentica usuário e salva token
- `register(data)` - Registra novo usuário e salva token
- `logout()` - Faz logout e limpa token
- `fetchProfile()` - Busca dados do usuário logado
- `updateProfile(data)` - Atualiza dados do usuário

#### 🔧 Mudanças Técnicas
- Adicionado estado `isAuthenticated`
- Token JWT salvo em `localStorage` como `auth_token`
- Logout limpa token e storage do Zustand
- Login/Register retornam token + dados do usuário

#### 📝 Exemplo de Uso
```typescript
const { user, isAuthenticated, login, logout, fetchProfile } = 
  useUserStore();

// Login
const handleLogin = async (data: LoginDTO) => {
  try {
    await login(data);
    navigate('/dashboard');
  } catch (error) {
    alert('Credenciais inválidas');
  }
};

// Logout
const handleLogout = async () => {
  await logout();
  navigate('/');
};

// Verificar autenticação
useEffect(() => {
  if (!isAuthenticated) {
    navigate('/login');
  }
}, [isAuthenticated]);
```

---

## 🎨 Padrão Comum em Todos os Stores

### Estados
```typescript
{
  items: Entity[];           // Dados da entidade
  isLoading: boolean;        // Estado de carregamento
  error: string | null;      // Mensagem de erro
}
```

### Fluxo de Ação
```typescript
async action() {
  set({ isLoading: true, error: null });
  try {
    const result = await api.method();
    set({ items: result, isLoading: false });
  } catch (error) {
    const errorMessage = error instanceof Error 
      ? error.message 
      : 'Erro genérico';
    set({ error: errorMessage, isLoading: false });
    throw error; // Propaga para componente
  }
}
```

---

## ⚠️ Breaking Changes

### Antes (Dados Locais)
```typescript
// Transaction Store
addTransaction: (transaction) =>
  set((state) => ({
    transactions: [
      ...state.transactions,
      { ...transaction, id: crypto.randomUUID() }
    ]
  }))
```

### Depois (API)
```typescript
// Transaction Store
addTransaction: async (data: CreateTransactionDTO) => {
  set({ isLoading: true, error: null });
  try {
    const newTransaction = await transactionApi.create(data);
    set((state) => ({
      transactions: [...state.transactions, newTransaction],
      isLoading: false,
    }));
  } catch (error) {
    const errorMessage = error instanceof Error 
      ? error.message 
      : 'Erro ao adicionar transação';
    set({ error: errorMessage, isLoading: false });
    throw error;
  }
}
```

### Impacto nos Componentes
- Actions agora são **assíncronas** (retornam `Promise<void>`)
- Componentes devem usar `async/await` ou `.then()`
- Tratamento de erro necessário em cada chamada
- Estados `isLoading` e `error` disponíveis para UI

---

## 🔐 Autenticação

### Token JWT
- Salvo em `localStorage` como `auth_token`
- Adicionado automaticamente no header `Authorization` pelo Axios
- Removido automaticamente no logout

### Interceptor do Axios
```typescript
// shared/api/client.ts
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('auth_token');
  if (token && config.headers) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

### Proteção de Rotas
```typescript
// Exemplo de proteção
function ProtectedRoute({ children }) {
  const { isAuthenticated } = useUserStore();
  
  if (!isAuthenticated) {
    return <Navigate to="/login" />;
  }
  
  return children;
}
```

---

## 🧪 Como Testar

### 1. Testar TransactionStore
```typescript
// pages/transactions/TransactionsPage.tsx
const { fetchTransactions, addTransaction } = useTransactionStore();

useEffect(() => {
  fetchTransactions(); // Deve fazer GET /lancamentos
}, []);

// Ao submeter formulário
const handleSubmit = async (data) => {
  await addTransaction(data); // Deve fazer POST /lancamentos
};
```

### 2. Testar UserStore (Login)
```typescript
// pages/login/LoginPage.tsx
const { login } = useUserStore();

const handleLogin = async (credentials) => {
  await login(credentials); // POST /usuarios/login
  // Token salvo automaticamente
  // Redirecionamento após sucesso
};
```

### 3. Verificar Estados de Loading
```typescript
const { isLoading, error, transactions } = useTransactionStore();

if (isLoading) return <LoadingSpinner />;
if (error) return <ErrorMessage message={error} />;
return <TransactionList items={transactions} />;
```

---

## 📦 Dependências

```json
{
  "zustand": "^5.0.2",
  "axios": "^1.7.0"
}
```

---

## ✅ Checklist de Migração

- [x] TransactionStore refatorado
- [x] AccountStore refatorado
- [x] CategoryStore refatorado
- [x] UserStore refatorado
- [x] Tratamento de erro em todas actions
- [x] Estados de loading implementados
- [x] Tokens JWT gerenciados corretamente
- [x] Build TypeScript sem erros
- [ ] Testar com backend rodando
- [ ] Atualizar componentes para usar actions assíncronas
- [ ] Adicionar componentes de Loading/Error UI

---

## 🚀 Próximos Passos

1. **Atualizar Páginas** - Refatorar páginas para usar actions assíncronas
2. **UI de Loading** - Criar componentes visuais para estados de loading
3. **UI de Erro** - Criar componentes para exibir mensagens de erro
4. **Testes E2E** - Testar fluxo completo com backend
5. **Validação** - Integrar React Hook Form + Zod nos formulários
6. **Otimização** - Adicionar debounce e cache quando necessário

---

**Build Status:** ✅ Passou sem erros
**TypeScript:** ✅ Compilação bem-sucedida
**Último Update:** 17 de outubro de 2025
