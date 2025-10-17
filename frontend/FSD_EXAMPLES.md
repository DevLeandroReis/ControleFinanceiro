# 📖 Guia de Exemplos - Feature-Sliced Design

Este documento contém exemplos práticos de como criar novos elementos seguindo o padrão FSD.

---

## 📦 1. Criando uma Nova Entidade

Exemplo: Entidade `Transaction`

### Estrutura:
```
src/entities/transaction/
├── model/
│   ├── types.ts           # TypeScript types
│   └── index.ts
├── ui/
│   ├── TransactionCard.tsx
│   ├── TransactionCard.css
│   └── index.ts
├── api/
│   ├── transactionApi.ts
│   └── index.ts
└── index.ts
```

### `model/types.ts`:
```typescript
export interface Transaction {
  id: string;
  description: string;
  amount: number;
  date: string;
  categoryId: string;
  type: 'income' | 'expense';
}

export type TransactionType = 'income' | 'expense';
```

### `ui/TransactionCard.tsx`:
```typescript
import type { FC } from 'react';
import type { Transaction } from '../model/types';
import './TransactionCard.css';

interface TransactionCardProps {
  transaction: Transaction;
}

export const TransactionCard: FC<TransactionCardProps> = ({ transaction }) => {
  return (
    <div className={`transaction-card transaction-card--${transaction.type}`}>
      <h3>{transaction.description}</h3>
      <p className="amount">R$ {transaction.amount.toFixed(2)}</p>
      <p className="date">{new Date(transaction.date).toLocaleDateString()}</p>
    </div>
  );
};
```

### `api/transactionApi.ts`:
```typescript
import { API_BASE_URL } from '@shared/config';
import type { Transaction } from '../model/types';

export const transactionApi = {
  getAll: async (): Promise<Transaction[]> => {
    const response = await fetch(`${API_BASE_URL}/lancamentos`);
    return response.json();
  },

  getById: async (id: string): Promise<Transaction> => {
    const response = await fetch(`${API_BASE_URL}/lancamentos/${id}`);
    return response.json();
  },

  create: async (transaction: Omit<Transaction, 'id'>): Promise<Transaction> => {
    const response = await fetch(`${API_BASE_URL}/lancamentos`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(transaction),
    });
    return response.json();
  },
};
```

### `index.ts` (barrel export):
```typescript
export { TransactionCard } from './ui/TransactionCard';
export { transactionApi } from './api/transactionApi';
export type { Transaction, TransactionType } from './model/types';
```

---

## ⚡ 2. Criando uma Nova Feature

Exemplo: Feature `add-transaction`

### Estrutura:
```
src/features/add-transaction/
├── model/
│   ├── useAddTransaction.ts    # Hook personalizado
│   └── index.ts
├── ui/
│   ├── AddTransactionForm.tsx
│   ├── AddTransactionForm.css
│   └── index.ts
└── index.ts
```

### `model/useAddTransaction.ts`:
```typescript
import { useState } from 'react';
import { transactionApi } from '@entities/transaction';
import type { Transaction } from '@entities/transaction';

export const useAddTransaction = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const addTransaction = async (data: Omit<Transaction, 'id'>) => {
    setIsLoading(true);
    setError(null);
    
    try {
      const newTransaction = await transactionApi.create(data);
      return newTransaction;
    } catch (err) {
      setError('Erro ao adicionar transação');
      throw err;
    } finally {
      setIsLoading(false);
    }
  };

  return { addTransaction, isLoading, error };
};
```

### `ui/AddTransactionForm.tsx`:
```typescript
import type { FC } from 'react';
import { useState } from 'react';
import { Button } from '@shared/ui';
import { useAddTransaction } from '../model/useAddTransaction';
import './AddTransactionForm.css';

export const AddTransactionForm: FC = () => {
  const { addTransaction, isLoading, error } = useAddTransaction();
  const [description, setDescription] = useState('');
  const [amount, setAmount] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    await addTransaction({
      description,
      amount: parseFloat(amount),
      date: new Date().toISOString(),
      categoryId: '1',
      type: 'expense',
    });
    
    // Reset form
    setDescription('');
    setAmount('');
  };

  return (
    <form className="add-transaction-form" onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Descrição"
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        required
      />
      <input
        type="number"
        placeholder="Valor"
        value={amount}
        onChange={(e) => setAmount(e.target.value)}
        required
      />
      <Button type="submit" disabled={isLoading}>
        {isLoading ? 'Adicionando...' : 'Adicionar Transação'}
      </Button>
      {error && <p className="error">{error}</p>}
    </form>
  );
};
```

### `index.ts`:
```typescript
export { AddTransactionForm } from './ui/AddTransactionForm';
export { useAddTransaction } from './model/useAddTransaction';
```

---

## 🧩 3. Criando um Widget

Exemplo: Widget `transactions-list`

### Estrutura:
```
src/widgets/transactions-list/
├── ui/
│   ├── TransactionsList.tsx
│   ├── TransactionsList.css
│   └── index.ts
└── index.ts
```

### `ui/TransactionsList.tsx`:
```typescript
import type { FC } from 'react';
import { useState, useEffect } from 'react';
import { TransactionCard, transactionApi } from '@entities/transaction';
import type { Transaction } from '@entities/transaction';
import './TransactionsList.css';

export const TransactionsList: FC = () => {
  const [transactions, setTransactions] = useState<Transaction[]>([]);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const loadTransactions = async () => {
      try {
        const data = await transactionApi.getAll();
        setTransactions(data);
      } finally {
        setIsLoading(false);
      }
    };

    loadTransactions();
  }, []);

  if (isLoading) {
    return <div>Carregando...</div>;
  }

  return (
    <div className="transactions-list">
      <h2>Últimas Transações</h2>
      <div className="transactions-grid">
        {transactions.map((transaction) => (
          <TransactionCard key={transaction.id} transaction={transaction} />
        ))}
      </div>
    </div>
  );
};
```

### `index.ts`:
```typescript
export { TransactionsList } from './ui/TransactionsList';
```

---

## 📄 4. Criando uma Nova Página

Exemplo: Página `TransactionsPage`

### Estrutura:
```
src/pages/transactions/
├── TransactionsPage.tsx
├── TransactionsPage.css
└── index.ts
```

### `TransactionsPage.tsx`:
```typescript
import type { FC } from 'react';
import { TransactionsList } from '@widgets/transactions-list';
import { AddTransactionForm } from '@features/add-transaction';
import './TransactionsPage.css';

export const TransactionsPage: FC = () => {
  return (
    <div className="transactions-page">
      <header>
        <h1>Gerenciar Transações</h1>
      </header>

      <section className="add-section">
        <h2>Nova Transação</h2>
        <AddTransactionForm />
      </section>

      <section className="list-section">
        <TransactionsList />
      </section>
    </div>
  );
};
```

### `index.ts`:
```typescript
export { TransactionsPage } from './TransactionsPage';
```

---

## 🔧 5. Criando Componentes Shared UI

Exemplo: Componente `Input`

### Estrutura:
```
src/shared/ui/input/
├── Input.tsx
├── Input.css
└── index.ts
```

### `Input.tsx`:
```typescript
import type { FC, InputHTMLAttributes } from 'react';
import './Input.css';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
  error?: string;
}

export const Input: FC<InputProps> = ({ 
  label, 
  error, 
  className = '', 
  ...props 
}) => {
  return (
    <div className="input-wrapper">
      {label && <label className="input-label">{label}</label>}
      <input 
        className={`input ${error ? 'input--error' : ''} ${className}`}
        {...props}
      />
      {error && <span className="input-error">{error}</span>}
    </div>
  );
};
```

### Atualize `src/shared/ui/index.ts`:
```typescript
export { Button } from './button/Button';
export { Input } from './input/Input';
```

---

## 🛠️ 6. Criando Utilitários Shared

Exemplo: Formatador de moeda

### `src/shared/lib/formatters/currency.ts`:
```typescript
export const formatCurrency = (value: number): string => {
  return new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
  }).format(value);
};

export const parseCurrency = (value: string): number => {
  return parseFloat(value.replace(/[^\d,]/g, '').replace(',', '.'));
};
```

### `src/shared/lib/index.ts`:
```typescript
export { formatCurrency, parseCurrency } from './formatters/currency';
```

---

## 📝 Boas Práticas

### 1. **Sempre use barrel exports (index.ts)**
```typescript
// ✅ Correto
export { Component } from './Component';
export type { ComponentProps } from './Component';

// ❌ Evitar
export * from './Component';
```

### 2. **Use imports absolutos com aliases**
```typescript
// ✅ Correto
import { Button } from '@shared/ui';

// ❌ Evitar
import { Button } from '../../../../shared/ui/button/Button';
```

### 3. **Respeite a hierarquia de camadas**
```typescript
// ✅ Correto (superior → inferior)
import { Transaction } from '@entities/transaction'; // em features
import { AddTransactionForm } from '@features/add-transaction'; // em pages

// ❌ Incorreto (inferior → superior)
import { TransactionsPage } from '@pages/transactions'; // em entities
```

### 4. **Use tipos explícitos**
```typescript
// ✅ Correto
import type { FC } from 'react';
import type { Transaction } from '@entities/transaction';

// ❌ Evitar
import { FC } from 'react';
import { Transaction } from '@entities/transaction';
```

### 5. **Organize por domínio, não por tipo de arquivo**
```
// ✅ Correto
entities/
  transaction/
    model/
    ui/
    api/

// ❌ Evitar
models/
  transaction.ts
components/
  TransactionCard.tsx
api/
  transactionApi.ts
```

---

## 🎯 Checklist para Novos Elementos

- [ ] Determinar a camada correta (entities, features, widgets, pages)
- [ ] Criar estrutura de pastas seguindo o padrão
- [ ] Implementar a lógica/componente
- [ ] Criar barrel export (index.ts)
- [ ] Usar path aliases nos imports
- [ ] Adicionar tipos TypeScript
- [ ] Seguir as regras de dependência entre camadas
- [ ] Documentar se necessário

---

**Lembre-se:** A arquitetura FSD promove baixo acoplamento e alta coesão. Cada slice deve ser independente e focado em um único domínio!
