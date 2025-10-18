# Integração do Frontend com Backend - Guia Completo

## 📚 Índice

1. [Visão Geral](#visão-geral)
2. [Tecnologias Utilizadas](#tecnologias-utilizadas)
3. [Estrutura de Arquivos](#estrutura-de-arquivos)
4. [Configuração do Axios](#configuração-do-axios)
5. [Validação com Zod](#validação-com-zod)
6. [React Hook Form](#react-hook-form)
7. [Serviços de API](#serviços-de-api)
8. [Integração com Stores](#integração-com-stores)
9. [Exemplos Práticos](#exemplos-práticos)
10. [Boas Práticas](#boas-práticas)

---

## 🎯 Visão Geral

Este documento descreve a integração completa do frontend com o backend da aplicação de Controle Financeiro, utilizando:

- **Axios** para requisições HTTP
- **Zod** para validação de schemas
- **React Hook Form** para gerenciamento de formulários
- **Zustand** para gerenciamento de estado

---

## 🛠 Tecnologias Utilizadas

```json
{
  "axios": "^1.7.0",
  "react-hook-form": "^7.53.0",
  "zod": "^3.24.0",
  "@hookform/resolvers": "^3.10.0",
  "zustand": "^5.0.2"
}
```

---

## 📁 Estrutura de Arquivos

```
src/
├── entities/
│   ├── user/
│   │   └── model/
│   │       ├── api.ts          # Serviço de API
│   │       ├── schemas.ts      # Schemas Zod
│   │       ├── types.ts        # TypeScript types
│   │       ├── store.ts        # Zustand store
│   │       └── index.ts        # Exports
│   ├── transaction/
│   │   └── model/
│   │       ├── api.ts
│   │       ├── schemas.ts
│   │       ├── types.ts
│   │       └── store.ts
│   ├── account/
│   │   └── model/
│   │       ├── api.ts
│   │       ├── schemas.ts
│   │       ├── types.ts
│   │       └── store.ts
│   └── category/
│       └── model/
│           ├── api.ts
│           ├── schemas.ts
│           ├── types.ts
│           └── store.ts
└── shared/
    ├── api/
    │   ├── client.ts           # Configuração do Axios
    │   └── index.ts
    └── config/
        └── index.ts            # Variáveis de ambiente
```

---

## ⚙️ Configuração do Axios

### `shared/api/client.ts`

```typescript
import axios, { AxiosInstance } from 'axios';
import { API_BASE_URL } from '../config';

// Criar instância do Axios
export const apiClient: AxiosInstance = axios.create({
  baseURL: API_BASE_URL,
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
});

// Interceptor de Request - Adiciona token de autenticação
apiClient.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('auth_token');
    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  },
  (error) => Promise.reject(error)
);

// Interceptor de Response - Tratamento de erros
apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      // Logout automático em caso de token inválido
      localStorage.removeItem('auth_token');
      localStorage.removeItem('user-storage');
      window.location.href = '/';
    }
    return Promise.reject(error);
  }
);
```

### Variáveis de Ambiente

Crie um arquivo `.env` na raiz do projeto frontend:

```env
VITE_API_BASE_URL=http://localhost:5001/api
```

---

## ✅ Validação com Zod

### Exemplo: Transaction Schema

```typescript
import { z } from 'zod';

export const createTransactionSchema = z.object({
  descricao: z.string()
    .min(3, 'Descrição deve ter no mínimo 3 caracteres')
    .max(200, 'Descrição deve ter no máximo 200 caracteres'),
  
  valor: z.number()
    .positive('Valor deve ser positivo'),
  
  data: z.string()
    .regex(/^\d{4}-\d{2}-\d{2}$/, 'Data deve estar no formato YYYY-MM-DD'),
  
  tipo: z.enum(['income', 'expense'], {
    message: 'Tipo deve ser receita ou despesa',
  }),
  
  categoriaId: z.string()
    .uuid('ID da categoria inválido'),
  
  contaId: z.string()
    .uuid('ID da conta inválido'),
  
  observacoes: z.string().optional(),
});

// Inferir tipo TypeScript do schema
export type CreateTransactionInput = z.infer<typeof createTransactionSchema>;
```

### Benefícios do Zod

- ✅ Validação em tempo de execução
- ✅ Inferência automática de tipos TypeScript
- ✅ Mensagens de erro customizáveis
- ✅ Integração perfeita com React Hook Form

---

## 📝 React Hook Form

### Configuração Básica

```typescript
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { createTransactionSchema, CreateTransactionInput } from '@/entities/transaction/model';

function TransactionForm() {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<CreateTransactionInput>({
    resolver: zodResolver(createTransactionSchema),
    defaultValues: {
      descricao: '',
      valor: 0,
      data: new Date().toISOString().split('T')[0],
      tipo: 'expense',
    },
  });

  const onSubmit = async (data: CreateTransactionInput) => {
    try {
      // Chamar API ou store
      console.log('Dados válidos:', data);
      reset(); // Limpa o formulário
    } catch (error) {
      console.error('Erro ao salvar:', error);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input
        type="text"
        {...register('descricao')}
        placeholder="Descrição"
      />
      {errors.descricao && (
        <span className="error">{errors.descricao.message}</span>
      )}

      <input
        type="number"
        step="0.01"
        {...register('valor', { valueAsNumber: true })}
        placeholder="Valor"
      />
      {errors.valor && (
        <span className="error">{errors.valor.message}</span>
      )}

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Salvando...' : 'Salvar'}
      </button>
    </form>
  );
}
```

### Recursos do React Hook Form

- ✅ Performance otimizada (menos re-renders)
- ✅ Validação integrada com Zod
- ✅ Estados de formulário (errors, isSubmitting, isDirty)
- ✅ Reset e setValue para manipular valores
- ✅ Watch para observar mudanças em campos

---

## 🌐 Serviços de API

### Estrutura dos Serviços

Cada entidade possui um arquivo `api.ts` com todos os endpoints:

```typescript
import { apiClient } from '@/shared/api';
import type { Transaction } from './types';

export interface CreateTransactionDTO {
  descricao: string;
  valor: number;
  data: string;
  tipo: 'income' | 'expense';
  categoriaId: string;
  contaId: string;
  observacoes?: string;
}

export const transactionApi = {
  async getAll(): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>('/lancamentos');
    return response.data;
  },

  async create(data: CreateTransactionDTO): Promise<Transaction> {
    const response = await apiClient.post<Transaction>('/lancamentos', data);
    return response.data;
  },

  async update(id: string, data: Partial<CreateTransactionDTO>): Promise<Transaction> {
    const response = await apiClient.put<Transaction>(`/lancamentos/${id}`, data);
    return response.data;
  },

  async delete(id: string): Promise<void> {
    await apiClient.delete(`/lancamentos/${id}`);
  },
};
```

### Endpoints Disponíveis

#### **Transactions** (`/lancamentos`)
- `GET /lancamentos` - Lista todas transações
- `GET /lancamentos/:id` - Busca por ID
- `POST /lancamentos` - Cria nova transação
- `PUT /lancamentos/:id` - Atualiza transação
- `DELETE /lancamentos/:id` - Remove transação

#### **Accounts** (`/contas`)
- `GET /contas` - Lista todas contas
- `GET /contas/:id` - Busca por ID
- `POST /contas` - Cria nova conta
- `PUT /contas/:id` - Atualiza conta
- `DELETE /contas/:id` - Remove conta
- `GET /contas/saldo-total` - Total de saldo

#### **Categories** (`/categorias`)
- `GET /categorias` - Lista todas categorias
- `GET /categorias/:id` - Busca por ID
- `POST /categorias` - Cria nova categoria
- `PUT /categorias/:id` - Atualiza categoria
- `DELETE /categorias/:id` - Remove categoria
- `POST /categorias/padrao` - Cria categorias padrão

#### **Users** (`/usuarios`)
- `POST /usuarios/login` - Login
- `POST /usuarios/registro` - Registro
- `GET /usuarios/perfil` - Perfil do usuário
- `PUT /usuarios/perfil` - Atualiza perfil
- `POST /usuarios/esqueci-senha` - Recuperação de senha

---

## 🏪 Integração com Stores

### Atualizar Store para Usar API

```typescript
import { create } from 'zustand';
import { devtools, persist } from 'zustand/middleware';
import { transactionApi } from './api';
import type { Transaction } from './types';

interface TransactionState {
  transactions: Transaction[];
  loading: boolean;
  error: string | null;
  
  // Actions
  fetchTransactions: () => Promise<void>;
  addTransaction: (data: CreateTransactionDTO) => Promise<void>;
  updateTransaction: (id: string, data: Partial<CreateTransactionDTO>) => Promise<void>;
  deleteTransaction: (id: string) => Promise<void>;
}

export const useTransactionStore = create<TransactionState>()(
  devtools(
    persist(
      (set, get) => ({
        transactions: [],
        loading: false,
        error: null,

        fetchTransactions: async () => {
          set({ loading: true, error: null });
          try {
            const transactions = await transactionApi.getAll();
            set({ transactions, loading: false });
          } catch (error) {
            set({ error: 'Erro ao carregar transações', loading: false });
          }
        },

        addTransaction: async (data) => {
          set({ loading: true, error: null });
          try {
            const newTransaction = await transactionApi.create(data);
            set((state) => ({
              transactions: [...state.transactions, newTransaction],
              loading: false,
            }));
          } catch (error) {
            set({ error: 'Erro ao adicionar transação', loading: false });
            throw error;
          }
        },

        updateTransaction: async (id, data) => {
          set({ loading: true, error: null });
          try {
            const updatedTransaction = await transactionApi.update(id, data);
            set((state) => ({
              transactions: state.transactions.map((t) =>
                t.id === id ? updatedTransaction : t
              ),
              loading: false,
            }));
          } catch (error) {
            set({ error: 'Erro ao atualizar transação', loading: false });
            throw error;
          }
        },

        deleteTransaction: async (id) => {
          set({ loading: true, error: null });
          try {
            await transactionApi.delete(id);
            set((state) => ({
              transactions: state.transactions.filter((t) => t.id !== id),
              loading: false,
            }));
          } catch (error) {
            set({ error: 'Erro ao deletar transação', loading: false });
            throw error;
          }
        },
      }),
      {
        name: 'transaction-storage',
      }
    )
  )
);
```

---

## 💡 Exemplos Práticos

### 1. Formulário de Login com Validação

```typescript
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { loginSchema, type LoginInput, userApi } from '@/entities/user/model';
import { useUserStore } from '@/entities/user';

function LoginPage() {
  const setUser = useUserStore((state) => state.setUser);
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginInput>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginInput) => {
    try {
      const response = await userApi.login(data);
      
      // Salvar token
      localStorage.setItem('auth_token', response.token);
      
      // Atualizar store
      setUser(response.usuario);
      
      // Redirecionar
      window.location.href = '/dashboard';
    } catch (error) {
      alert('Erro ao fazer login');
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input type="email" {...register('email')} placeholder="Email" />
      {errors.email && <span>{errors.email.message}</span>}

      <input type="password" {...register('senha')} placeholder="Senha" />
      {errors.senha && <span>{errors.senha.message}</span>}

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Entrando...' : 'Entrar'}
      </button>
    </form>
  );
}
```

### 2. Lista com Fetch ao Montar

```typescript
import { useEffect } from 'react';
import { useTransactionStore } from '@/entities/transaction';

function TransactionsList() {
  const { transactions, loading, error, fetchTransactions, deleteTransaction } =
    useTransactionStore();

  useEffect(() => {
    fetchTransactions();
  }, [fetchTransactions]);

  if (loading) return <div>Carregando...</div>;
  if (error) return <div>Erro: {error}</div>;

  return (
    <ul>
      {transactions.map((transaction) => (
        <li key={transaction.id}>
          {transaction.descricao} - R$ {transaction.valor}
          <button onClick={() => deleteTransaction(transaction.id)}>
            Deletar
          </button>
        </li>
      ))}
    </ul>
  );
}
```

### 3. Formulário Completo de Transação

```typescript
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  createTransactionSchema,
  type CreateTransactionInput,
} from '@/entities/transaction/model';
import { useTransactionStore } from '@/entities/transaction';
import { useCategoryStore } from '@/entities/category';
import { useAccountStore } from '@/entities/account';

function TransactionForm() {
  const addTransaction = useTransactionStore((state) => state.addTransaction);
  const categories = useCategoryStore((state) => state.categories);
  const accounts = useAccountStore((state) => state.accounts);

  const {
    register,
    handleSubmit,
    control,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<CreateTransactionInput>({
    resolver: zodResolver(createTransactionSchema),
  });

  const onSubmit = async (data: CreateTransactionInput) => {
    try {
      await addTransaction(data);
      reset();
      alert('Transação adicionada com sucesso!');
    } catch (error) {
      alert('Erro ao adicionar transação');
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <input {...register('descricao')} placeholder="Descrição" />
      {errors.descricao && <span>{errors.descricao.message}</span>}

      <input
        type="number"
        step="0.01"
        {...register('valor', { valueAsNumber: true })}
        placeholder="Valor"
      />
      {errors.valor && <span>{errors.valor.message}</span>}

      <input type="date" {...register('data')} />
      {errors.data && <span>{errors.data.message}</span>}

      <select {...register('tipo')}>
        <option value="income">Receita</option>
        <option value="expense">Despesa</option>
      </select>
      {errors.tipo && <span>{errors.tipo.message}</span>}

      <select {...register('categoriaId')}>
        <option value="">Selecione uma categoria</option>
        {categories.map((cat) => (
          <option key={cat.id} value={cat.id}>
            {cat.nome}
          </option>
        ))}
      </select>
      {errors.categoriaId && <span>{errors.categoriaId.message}</span>}

      <select {...register('contaId')}>
        <option value="">Selecione uma conta</option>
        {accounts.map((acc) => (
          <option key={acc.id} value={acc.id}>
            {acc.nome}
          </option>
        ))}
      </select>
      {errors.contaId && <span>{errors.contaId.message}</span>}

      <textarea {...register('observacoes')} placeholder="Observações" />

      <button type="submit" disabled={isSubmitting}>
        {isSubmitting ? 'Salvando...' : 'Salvar Transação'}
      </button>
    </form>
  );
}
```

---

## ✨ Boas Práticas

### 1. Tratamento de Erros

```typescript
const addTransaction = async (data: CreateTransactionInput) => {
  try {
    await transactionApi.create(data);
    // Sucesso
  } catch (error) {
    if (axios.isAxiosError(error)) {
      const message = error.response?.data?.message || 'Erro desconhecido';
      alert(message);
    }
  }
};
```

### 2. Loading States

```typescript
function Component() {
  const { loading, error } = useTransactionStore();

  if (loading) return <LoadingSpinner />;
  if (error) return <ErrorMessage message={error} />;
  
  return <Content />;
}
```

### 3. Validação Condicional

```typescript
const schema = z.object({
  tipo: z.enum(['income', 'expense']),
  valor: z.number(),
}).refine(
  (data) => {
    if (data.tipo === 'expense') {
      return data.valor > 0;
    }
    return true;
  },
  {
    message: 'Despesas devem ter valor positivo',
    path: ['valor'],
  }
);
```

### 4. Reset de Formulário

```typescript
const { reset } = useForm();

// Reset para valores padrão
reset();

// Reset com novos valores
reset({
  descricao: '',
  valor: 100,
});
```

### 5. Watch de Campos

```typescript
const { watch } = useForm();
const tipo = watch('tipo');

// Renderização condicional
{tipo === 'expense' && <TaxaField />}
```

---

## 🔒 Segurança

1. **Nunca expor secrets no código**
2. **Usar variáveis de ambiente** (`.env`)
3. **Token JWT** no header Authorization
4. **Logout automático** em 401
5. **Validação no frontend E backend**

---

## 📌 Próximos Passos

- [ ] Implementar refresh token
- [ ] Adicionar React Query para cache
- [ ] Criar componentes reutilizáveis de formulário
- [ ] Adicionar testes unitários
- [ ] Implementar modo offline com Service Workers

---

## 📚 Referências

- [Axios Documentation](https://axios-http.com/)
- [React Hook Form](https://react-hook-form.com/)
- [Zod Documentation](https://zod.dev/)
- [Zustand](https://github.com/pmndrs/zustand)
- [Feature-Sliced Design](https://feature-sliced.design/)

---

**Documentação criada em:** 2025
**Última atualização:** Janeiro 2025
