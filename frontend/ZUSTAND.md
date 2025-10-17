# 🔄 Zustand - Gerenciamento de Estado

Zustand foi implementado para gerenciamento de estado global no projeto, seguindo a arquitetura Feature-Sliced Design.

## 📦 Instalação

```bash
npm install zustand
```

**Versão instalada:** `zustand@^5.0.2`

---

## 🏗️ Arquitetura

As stores Zustand foram organizadas na camada **entities**, seguindo o padrão FSD:

```
src/entities/
├── user/
│   └── model/
│       ├── types.ts     # Interfaces TypeScript
│       ├── store.ts     # Zustand store
│       └── index.ts     # Barrel export
├── transaction/
│   └── model/
│       ├── types.ts
│       ├── store.ts
│       └── index.ts
├── account/
│   └── model/
│       ├── types.ts
│       ├── store.ts
│       └── index.ts
└── category/
    └── model/
        ├── types.ts
        ├── store.ts
        └── index.ts
```

---

## 📊 Stores Implementadas

### 1. **User Store** (`entities/user`)

Gerencia o estado do usuário autenticado.

**State:**
```typescript
interface UserState {
  user: User | null;
  isLoading: boolean;
  error: string | null;
  setUser: (user: User | null) => void;
  clearUser: () => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
}
```

**Uso:**
```typescript
import { useUserStore } from '../../entities/user';

const user = useUserStore((state) => state.user);
const setUser = useUserStore((state) => state.setUser);
```

**Features:**
- ✅ Persistência no localStorage (via middleware `persist`)
- ✅ DevTools habilitado para debug
- ✅ Estado sincronizado entre tabs

---

### 2. **Transaction Store** (`entities/transaction`)

Gerencia lançamentos financeiros (receitas e despesas).

**State:**
```typescript
interface TransactionState {
  transactions: Transaction[];
  isLoading: boolean;
  error: string | null;
  addTransaction: (transaction: Omit<Transaction, 'id'>) => void;
  updateTransaction: (id: string, transaction: Partial<Transaction>) => void;
  deleteTransaction: (id: string) => void;
  setTransactions: (transactions: Transaction[]) => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearTransactions: () => void;
}
```

**Uso:**
```typescript
import { useTransactionStore } from '../../entities/transaction';

const transactions = useTransactionStore((state) => state.transactions);
const addTransaction = useTransactionStore((state) => state.addTransaction);
const deleteTransaction = useTransactionStore((state) => state.deleteTransaction);

// Adicionar transação
addTransaction({
  descricao: 'Salário',
  valor: 5000,
  data: '2025-10-17',
  tipo: 'income',
  categoriaId: '1',
  contaId: '1',
  usuarioId: '1',
});

// Deletar transação
deleteTransaction('transaction-id');
```

**Features:**
- ✅ CRUD completo de transações
- ✅ Geração automática de IDs (crypto.randomUUID())
- ✅ DevTools habilitado

---

### 3. **Account Store** (`entities/account`)

Gerencia contas bancárias e carteiras.

**State:**
```typescript
interface AccountState {
  accounts: Account[];
  isLoading: boolean;
  error: string | null;
  addAccount: (account: Omit<Account, 'id' | 'saldoAtual'>) => void;
  updateAccount: (id: string, account: Partial<Account>) => void;
  deleteAccount: (id: string) => void;
  setAccounts: (accounts: Account[]) => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearAccounts: () => void;
  getTotalBalance: () => number;
}
```

**Uso:**
```typescript
import { useAccountStore } from '../../entities/account';

const accounts = useAccountStore((state) => state.accounts);
const getTotalBalance = useAccountStore((state) => state.getTotalBalance);
const addAccount = useAccountStore((state) => state.addAccount);

// Obter saldo total
const totalBalance = getTotalBalance();

// Adicionar conta
addAccount({
  nome: 'Conta Corrente',
  tipo: 'corrente',
  saldoInicial: 1000,
  usuarioId: '1',
});
```

**Features:**
- ✅ CRUD completo de contas
- ✅ Cálculo de saldo total
- ✅ Persistência no localStorage
- ✅ DevTools habilitado

---

### 4. **Category Store** (`entities/category`)

Gerencia categorias de transações.

**State:**
```typescript
interface CategoryState {
  categories: Category[];
  isLoading: boolean;
  error: string | null;
  addCategory: (category: Omit<Category, 'id'>) => void;
  updateCategory: (id: string, category: Partial<Category>) => void;
  deleteCategory: (id: string) => void;
  setCategories: (categories: Category[]) => void;
  getCategoriesByType: (type: CategoryType) => Category[];
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearCategories: () => void;
}
```

**Uso:**
```typescript
import { useCategoryStore } from '../../entities/category';

const categories = useCategoryStore((state) => state.categories);
const getCategoriesByType = useCategoryStore((state) => state.getCategoriesByType);

// Filtrar categorias por tipo
const incomeCategories = getCategoriesByType('income');
const expenseCategories = getCategoriesByType('expense');
```

**Features:**
- ✅ CRUD completo de categorias
- ✅ Filtro por tipo (income/expense)
- ✅ Persistência no localStorage
- ✅ DevTools habilitado

---

## 🎯 Padrões de Uso

### 1. **Selectors (Recomendado)**

Use selectors para obter apenas o estado necessário:

```typescript
// ✅ Correto - componente re-renderiza apenas quando transactions muda
const transactions = useTransactionStore((state) => state.transactions);

// ❌ Evitar - componente re-renderiza em qualquer mudança na store
const store = useTransactionStore();
```

### 2. **Múltiplos Selectors**

```typescript
const transactions = useTransactionStore((state) => state.transactions);
const addTransaction = useTransactionStore((state) => state.addTransaction);
const isLoading = useTransactionStore((state) => state.isLoading);
```

### 3. **Actions em Event Handlers**

```typescript
const addTransaction = useTransactionStore((state) => state.addTransaction);

const handleSubmit = () => {
  addTransaction({
    descricao: 'Nova transação',
    valor: 100,
    // ...
  });
};
```

### 4. **Computed Values**

```typescript
// Calcular valores derivados
const monthlyTransactions = transactions.filter((t) => {
  const date = new Date(t.data);
  return date.getMonth() === currentMonth;
});

const totalIncome = monthlyTransactions
  .filter((t) => t.tipo === 'income')
  .reduce((sum, t) => sum + t.valor, 0);
```

---

## 🔧 Middlewares Utilizados

### 1. **devtools**

Habilita a integração com Redux DevTools para debug:

```typescript
import { devtools } from 'zustand/middleware';

export const useTransactionStore = create<TransactionState>()(
  devtools((set) => ({
    // state e actions
  }))
);
```

**Como usar:**
1. Instale a extensão [Redux DevTools](https://github.com/reduxjs/redux-devtools)
2. Abra o DevTools no navegador
3. Visualize o estado e as actions em tempo real

### 2. **persist**

Persiste o estado no localStorage:

```typescript
import { persist } from 'zustand/middleware';

export const useAccountStore = create<AccountState>()(
  devtools(
    persist(
      (set) => ({
        // state e actions
      }),
      {
        name: 'accounts-storage', // Nome da chave no localStorage
      }
    )
  )
);
```

**Features:**
- ✅ Estado preservado após recarregar a página
- ✅ Sincronização entre tabs
- ✅ Configurável por store

---

## 📄 Integração nas Páginas

### Dashboard Page

A página Dashboard usa as stores para exibir métricas:

```typescript
import { useTransactionStore } from '../../entities/transaction';
import { useAccountStore } from '../../entities/account';

export const DashboardPage: FC = () => {
  const transactions = useTransactionStore((state) => state.transactions);
  const getTotalBalance = useAccountStore((state) => state.getTotalBalance);
  
  // Calcular receitas e despesas
  const income = transactions
    .filter((t) => t.tipo === 'income')
    .reduce((sum, t) => sum + t.valor, 0);
  
  const expense = transactions
    .filter((t) => t.tipo === 'expense')
    .reduce((sum, t) => sum + t.valor, 0);
  
  const totalBalance = getTotalBalance();
  
  // ...
};
```

### Transactions Page

A página Transactions usa a store para CRUD de lançamentos:

```typescript
import { useTransactionStore } from '../../entities/transaction';

export const TransactionsPage: FC = () => {
  const transactions = useTransactionStore((state) => state.transactions);
  const addTransaction = useTransactionStore((state) => state.addTransaction);
  const deleteTransaction = useTransactionStore((state) => state.deleteTransaction);
  
  const handleSubmit = (formData) => {
    addTransaction(formData);
  };
  
  const handleDelete = (id) => {
    deleteTransaction(id);
  };
  
  // ...
};
```

---

## 🎨 Boas Práticas

### 1. **Separação de Concerns**

```typescript
// ✅ Correto - Store na camada entities
src/entities/transaction/model/store.ts

// ❌ Evitar - Store fora da estrutura FSD
src/store/transactions.ts
```

### 2. **Tipagem Forte**

```typescript
// ✅ Correto - Tipos explícitos
const transactions = useTransactionStore(
  (state: { transactions: Transaction[] }) => state.transactions
);

// Ou melhor ainda, criar um hook customizado
const useTransactions = () => 
  useTransactionStore((state) => state.transactions);
```

### 3. **Imutabilidade**

```typescript
// ✅ Correto - Criar novo array
addTransaction: (transaction) =>
  set((state) => ({
    transactions: [...state.transactions, { ...transaction, id: crypto.randomUUID() }],
  })),

// ❌ Evitar - Mutar estado diretamente
addTransaction: (transaction) => {
  state.transactions.push(transaction); // NUNCA FAZER ISSO
}
```

### 4. **Actions com Side Effects**

Para operações assíncronas (API calls), crie actions separadas:

```typescript
addTransaction: async (transaction) => {
  set({ isLoading: true });
  try {
    const response = await api.post('/transactions', transaction);
    set((state) => ({
      transactions: [...state.transactions, response.data],
      isLoading: false,
    }));
  } catch (error) {
    set({ error: error.message, isLoading: false });
  }
},
```

---

## 🔄 Próximas Melhorias

### 1. **Custom Hooks**

Criar hooks customizados para facilitar o uso:

```typescript
// src/entities/transaction/model/hooks.ts
export const useTransactions = () => {
  const transactions = useTransactionStore((state) => state.transactions);
  const addTransaction = useTransactionStore((state) => state.addTransaction);
  const deleteTransaction = useTransactionStore((state) => state.deleteTransaction);
  
  return { transactions, addTransaction, deleteTransaction };
};
```

### 2. **Integração com API**

Criar actions que se comunicam com o backend:

```typescript
fetchTransactions: async () => {
  set({ isLoading: true });
  const data = await api.get('/transactions');
  set({ transactions: data, isLoading: false });
},
```

### 3. **Optimistic Updates**

Atualizar UI antes da confirmação do backend:

```typescript
addTransaction: async (transaction) => {
  const tempId = crypto.randomUUID();
  const tempTransaction = { ...transaction, id: tempId };
  
  // Adiciona imediatamente na UI
  set((state) => ({
    transactions: [...state.transactions, tempTransaction],
  }));
  
  try {
    // Confirma no backend
    const response = await api.post('/transactions', transaction);
    // Substitui o temporário pelo real
    set((state) => ({
      transactions: state.transactions.map((t) =>
        t.id === tempId ? response.data : t
      ),
    }));
  } catch (error) {
    // Remove em caso de erro
    set((state) => ({
      transactions: state.transactions.filter((t) => t.id !== tempId),
      error: error.message,
    }));
  }
},
```

---

## 📚 Recursos

- [Zustand Documentation](https://zustand-demo.pmnd.rs/)
- [Zustand GitHub](https://github.com/pmndrs/zustand)
- [Zustand Best Practices](https://github.com/pmndrs/zustand/wiki/Best-Practices)
- [TypeScript Guide](https://github.com/pmndrs/zustand/blob/main/docs/guides/typescript.md)

---

## ✅ Status

**Implementado:**
- ✅ 4 stores completas (User, Transaction, Account, Category)
- ✅ Persistência com localStorage
- ✅ DevTools habilitado
- ✅ Tipagem TypeScript completa
- ✅ Integração com Dashboard e Transactions pages
- ✅ CRUD funcional de transações

**Gerenciamento de estado totalmente operacional!** 🎉
