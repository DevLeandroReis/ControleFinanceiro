# Como Usar os Stores com API - Guia Prático

## 🎯 Objetivo

Este guia mostra exemplos práticos de como usar os stores atualizados nos seus componentes React.

---

## 📝 1. Página de Login

```tsx
import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useUserStore, loginSchema, type LoginInput } from '@/entities/user/model';

export function LoginPage() {
  const navigate = useNavigate();
  const { login, isLoading, error } = useUserStore();
  
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginInput>({
    resolver: zodResolver(loginSchema),
  });

  const onSubmit = async (data: LoginInput) => {
    try {
      await login(data);
      navigate('/dashboard');
    } catch (error) {
      // Erro já está no store.error
      console.error('Falha no login:', error);
    }
  };

  return (
    <div className="login-page">
      <h1>Login</h1>
      
      {error && (
        <div className="error-message" role="alert">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label htmlFor="email">Email</label>
          <input
            id="email"
            type="email"
            {...register('email')}
            disabled={isLoading}
          />
          {errors.email && <span>{errors.email.message}</span>}
        </div>

        <div>
          <label htmlFor="senha">Senha</label>
          <input
            id="senha"
            type="password"
            {...register('senha')}
            disabled={isLoading}
          />
          {errors.senha && <span>{errors.senha.message}</span>}
        </div>

        <button type="submit" disabled={isLoading}>
          {isLoading ? 'Entrando...' : 'Entrar'}
        </button>
      </form>
    </div>
  );
}
```

---

## 💰 2. Página de Transações (Lista + Formulário)

```tsx
import { useEffect, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import {
  useTransactionStore,
  createTransactionSchema,
  type CreateTransactionInput,
} from '@/entities/transaction/model';
import { useCategoryStore } from '@/entities/category/model';
import { useAccountStore } from '@/entities/account/model';

export function TransactionsPage() {
  const [showForm, setShowForm] = useState(false);
  
  // Stores
  const {
    transactions,
    isLoading: loadingTransactions,
    error: transactionError,
    fetchTransactions,
    addTransaction,
    deleteTransaction,
  } = useTransactionStore();
  
  const { categories, fetchCategories } = useCategoryStore();
  const { accounts, fetchAccounts } = useAccountStore();

  // Form
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<CreateTransactionInput>({
    resolver: zodResolver(createTransactionSchema),
    defaultValues: {
      data: new Date().toISOString().split('T')[0],
      tipo: 'expense',
    },
  });

  // Carregar dados ao montar
  useEffect(() => {
    fetchTransactions();
    fetchCategories();
    fetchAccounts();
  }, []);

  const onSubmit = async (data: CreateTransactionInput) => {
    try {
      await addTransaction(data);
      reset();
      setShowForm(false);
      alert('Transação adicionada com sucesso!');
    } catch (error) {
      alert('Erro ao adicionar transação');
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Deseja deletar esta transação?')) return;
    
    try {
      await deleteTransaction(id);
      alert('Transação deletada!');
    } catch (error) {
      alert('Erro ao deletar transação');
    }
  };

  if (loadingTransactions) {
    return <div>Carregando transações...</div>;
  }

  if (transactionError) {
    return (
      <div className="error">
        <p>Erro: {transactionError}</p>
        <button onClick={fetchTransactions}>Tentar Novamente</button>
      </div>
    );
  }

  return (
    <div className="transactions-page">
      <div className="header">
        <h1>Transações</h1>
        <button onClick={() => setShowForm(!showForm)}>
          {showForm ? 'Cancelar' : 'Nova Transação'}
        </button>
      </div>

      {showForm && (
        <form onSubmit={handleSubmit(onSubmit)} className="transaction-form">
          <input
            type="text"
            placeholder="Descrição"
            {...register('descricao')}
          />
          {errors.descricao && <span>{errors.descricao.message}</span>}

          <input
            type="number"
            step="0.01"
            placeholder="Valor"
            {...register('valor', { valueAsNumber: true })}
          />
          {errors.valor && <span>{errors.valor.message}</span>}

          <input type="date" {...register('data')} />
          {errors.data && <span>{errors.data.message}</span>}

          <select {...register('tipo')}>
            <option value="income">Receita</option>
            <option value="expense">Despesa</option>
          </select>

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

          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Salvando...' : 'Salvar'}
          </button>
        </form>
      )}

      <div className="transactions-list">
        {transactions.length === 0 ? (
          <p>Nenhuma transação encontrada</p>
        ) : (
          <ul>
            {transactions.map((transaction) => (
              <li key={transaction.id}>
                <div>
                  <strong>{transaction.descricao}</strong>
                  <span
                    className={transaction.tipo === 'income' ? 'income' : 'expense'}
                  >
                    {transaction.tipo === 'income' ? '+' : '-'} R${' '}
                    {transaction.valor.toFixed(2)}
                  </span>
                </div>
                <div>
                  <small>{new Date(transaction.data).toLocaleDateString()}</small>
                  <button onClick={() => handleDelete(transaction.id)}>
                    Deletar
                  </button>
                </div>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  );
}
```

---

## 📊 3. Dashboard com Dados Agregados

```tsx
import { useEffect, useMemo } from 'react';
import { useTransactionStore } from '@/entities/transaction/model';
import { useAccountStore } from '@/entities/account/model';

export function DashboardPage() {
  const {
    transactions,
    isLoading: loadingTransactions,
    fetchTransactions,
  } = useTransactionStore();
  
  const {
    accounts,
    isLoading: loadingAccounts,
    getTotalBalance,
    fetchAccounts,
  } = useAccountStore();

  useEffect(() => {
    fetchTransactions();
    fetchAccounts();
  }, []);

  // Calcular métricas
  const metrics = useMemo(() => {
    const currentMonth = new Date().getMonth();
    const currentYear = new Date().getFullYear();

    const monthlyTransactions = transactions.filter((t) => {
      const date = new Date(t.data);
      return date.getMonth() === currentMonth && date.getFullYear() === currentYear;
    });

    const income = monthlyTransactions
      .filter((t) => t.tipo === 'income')
      .reduce((sum, t) => sum + t.valor, 0);

    const expense = monthlyTransactions
      .filter((t) => t.tipo === 'expense')
      .reduce((sum, t) => sum + t.valor, 0);

    return {
      totalBalance: getTotalBalance(),
      monthlyIncome: income,
      monthlyExpense: expense,
      monthlyBalance: income - expense,
    };
  }, [transactions, accounts]);

  if (loadingTransactions || loadingAccounts) {
    return <div>Carregando dashboard...</div>;
  }

  return (
    <div className="dashboard">
      <h1>Dashboard Financeiro</h1>

      <div className="metrics-grid">
        <div className="metric-card">
          <h3>Saldo Total</h3>
          <p className="value">R$ {metrics.totalBalance.toFixed(2)}</p>
        </div>

        <div className="metric-card income">
          <h3>Receitas do Mês</h3>
          <p className="value">R$ {metrics.monthlyIncome.toFixed(2)}</p>
        </div>

        <div className="metric-card expense">
          <h3>Despesas do Mês</h3>
          <p className="value">R$ {metrics.monthlyExpense.toFixed(2)}</p>
        </div>

        <div className="metric-card">
          <h3>Balanço do Mês</h3>
          <p
            className={`value ${
              metrics.monthlyBalance >= 0 ? 'positive' : 'negative'
            }`}
          >
            R$ {metrics.monthlyBalance.toFixed(2)}
          </p>
        </div>
      </div>

      <div className="accounts-summary">
        <h2>Minhas Contas</h2>
        {accounts.map((account) => (
          <div key={account.id} className="account-item">
            <span>{account.nome}</span>
            <span>R$ {account.saldoAtual.toFixed(2)}</span>
          </div>
        ))}
      </div>
    </div>
  );
}
```

---

## 🔐 4. Proteção de Rotas

```tsx
import { Navigate, Outlet } from 'react-router-dom';
import { useUserStore } from '@/entities/user/model';

export function ProtectedRoute() {
  const { isAuthenticated } = useUserStore();

  if (!isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return <Outlet />;
}

// No router
<Route element={<ProtectedRoute />}>
  <Route path="/dashboard" element={<DashboardPage />} />
  <Route path="/transactions" element={<TransactionsPage />} />
  <Route path="/accounts" element={<AccountsPage />} />
</Route>
```

---

## 🏷️ 5. Gerenciamento de Categorias

```tsx
import { useEffect } from 'react';
import { useCategoryStore } from '@/entities/category/model';

export function CategoriesPage() {
  const {
    categories,
    isLoading,
    error,
    fetchCategories,
    createDefaultCategories,
    deleteCategory,
  } = useCategoryStore();

  useEffect(() => {
    fetchCategories();
  }, []);

  const handleSetupDefaults = async () => {
    try {
      await createDefaultCategories();
      alert('Categorias padrão criadas!');
    } catch (error) {
      alert('Erro ao criar categorias padrão');
    }
  };

  const handleDelete = async (id: string) => {
    if (!confirm('Deseja deletar esta categoria?')) return;
    
    try {
      await deleteCategory(id);
    } catch (error) {
      alert('Erro ao deletar categoria');
    }
  };

  if (isLoading) return <div>Carregando...</div>;
  if (error) return <div>Erro: {error}</div>;

  return (
    <div className="categories-page">
      <div className="header">
        <h1>Categorias</h1>
        <button onClick={handleSetupDefaults}>
          Criar Categorias Padrão
        </button>
      </div>

      <div className="categories-list">
        <h2>Receitas</h2>
        {categories
          .filter((c) => c.tipo === 'income')
          .map((category) => (
            <div key={category.id} className="category-item">
              <span>{category.nome}</span>
              <button onClick={() => handleDelete(category.id)}>
                Deletar
              </button>
            </div>
          ))}

        <h2>Despesas</h2>
        {categories
          .filter((c) => c.tipo === 'expense')
          .map((category) => (
            <div key={category.id} className="category-item">
              <span>{category.nome}</span>
              <button onClick={() => handleDelete(category.id)}>
                Deletar
              </button>
            </div>
          ))}
      </div>
    </div>
  );
}
```

---

## 👤 6. Perfil do Usuário

```tsx
import { useEffect } from 'react';
import { useForm } from 'react-hook-form';
import { useUserStore } from '@/entities/user/model';

export function ProfilePage() {
  const { user, isLoading, updateProfile, fetchProfile } = useUserStore();

  const { register, handleSubmit, reset } = useForm();

  useEffect(() => {
    fetchProfile();
  }, []);

  useEffect(() => {
    if (user) {
      reset({
        nome: user.nome,
        email: user.email,
      });
    }
  }, [user, reset]);

  const onSubmit = async (data: any) => {
    try {
      await updateProfile(data);
      alert('Perfil atualizado!');
    } catch (error) {
      alert('Erro ao atualizar perfil');
    }
  };

  if (isLoading) return <div>Carregando perfil...</div>;

  return (
    <div className="profile-page">
      <h1>Meu Perfil</h1>
      
      <form onSubmit={handleSubmit(onSubmit)}>
        <div>
          <label>Nome</label>
          <input type="text" {...register('nome')} />
        </div>

        <div>
          <label>Email</label>
          <input type="email" {...register('email')} />
        </div>

        <button type="submit">Salvar Alterações</button>
      </form>
    </div>
  );
}
```

---

## 🚦 7. Componente de Loading Reutilizável

```tsx
interface LoadingWrapperProps {
  isLoading: boolean;
  error: string | null;
  onRetry?: () => void;
  children: React.ReactNode;
}

export function LoadingWrapper({
  isLoading,
  error,
  onRetry,
  children,
}: LoadingWrapperProps) {
  if (isLoading) {
    return (
      <div className="loading-wrapper">
        <div className="spinner"></div>
        <p>Carregando...</p>
      </div>
    );
  }

  if (error) {
    return (
      <div className="error-wrapper">
        <p className="error-message">{error}</p>
        {onRetry && (
          <button onClick={onRetry}>Tentar Novamente</button>
        )}
      </div>
    );
  }

  return <>{children}</>;
}

// Uso
export function TransactionsPage() {
  const { transactions, isLoading, error, fetchTransactions } = 
    useTransactionStore();

  return (
    <LoadingWrapper
      isLoading={isLoading}
      error={error}
      onRetry={fetchTransactions}
    >
      <TransactionsList transactions={transactions} />
    </LoadingWrapper>
  );
}
```

---

## 🎣 8. Custom Hook para Fetch ao Montar

```tsx
import { useEffect, useRef } from 'react';

export function useFetchOnMount(fetchFn: () => Promise<void>) {
  const hasFetched = useRef(false);

  useEffect(() => {
    if (!hasFetched.current) {
      fetchFn();
      hasFetched.current = true;
    }
  }, [fetchFn]);
}

// Uso
export function TransactionsPage() {
  const { fetchTransactions } = useTransactionStore();
  
  useFetchOnMount(fetchTransactions);
  
  // Resto do componente...
}
```

---

## ✅ Checklist de Implementação

Ao usar os stores nos componentes:

- [ ] Importar o store correto
- [ ] Desestruturar apenas o que precisa (otimização)
- [ ] Chamar `fetch*` no `useEffect` ao montar
- [ ] Usar `isLoading` para mostrar loading
- [ ] Usar `error` para mostrar mensagens de erro
- [ ] Usar `try/catch` em actions assíncronas
- [ ] Tratar erros com feedback ao usuário
- [ ] Resetar formulários após sucesso
- [ ] Confirmar ações destrutivas (delete)
- [ ] Desabilitar botões durante loading

---

**Última Atualização:** 17 de outubro de 2025
