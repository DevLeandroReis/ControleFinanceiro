# 🚀 Quick Reference - Features

## Import das Features

```typescript
import {
  // Forms
  AddTransactionForm,
  EditTransactionForm,
  EditCategoryForm,
  EditAccountForm,
  TransactionFilterPanel,
  
  // Hooks
  useAddTransaction,
  useEditTransaction,
  useEditCategory,
  useEditAccount,
  useFilterTransactions,
  
  // Types
  type TransactionFilters
} from '@/features';
```

---

## 🎯 Uso Rápido

### 1. Adicionar Transação

```tsx
import { AddTransactionForm } from '@/features';

<AddTransactionForm
  onSuccess={() => console.log('Sucesso!')}
  onCancel={() => setShowForm(false)}
/>
```

### 2. Editar Transação

```tsx
import { EditTransactionForm } from '@/features';

<EditTransactionForm
  transaction={selectedTransaction}
  onSuccess={() => handleSuccess()}
  onCancel={() => handleCancel()}
/>
```

### 3. Editar Categoria

```tsx
import { EditCategoryForm } from '@/features';

<EditCategoryForm
  category={selectedCategory}
  onSuccess={() => refresh()}
  onCancel={() => close()}
/>
```

### 4. Editar Conta

```tsx
import { EditAccountForm } from '@/features';

<EditAccountForm
  account={selectedAccount}
  onSuccess={() => refresh()}
  onCancel={() => close()}
/>
```

### 5. Filtrar Transações

```tsx
import { useState } from 'react';
import { 
  TransactionFilterPanel,
  useFilterTransactions,
  type TransactionFilters 
} from '@/features';

const [filters, setFilters] = useState<TransactionFilters>({});

const { filteredTransactions, stats } = useFilterTransactions(
  transactions,
  filters
);

<TransactionFilterPanel
  filters={filters}
  onFiltersChange={setFilters}
  onClearFilters={() => setFilters({})}
/>
```

---

## 📝 Tipos Importantes

```typescript
interface TransactionFilters {
  tipo?: 'income' | 'expense' | '';
  categoriaId?: string;
  contaId?: string;
  dataInicio?: string;
  dataFim?: string;
  valorMin?: number;
  valorMax?: number;
  descricao?: string;
}

interface FilterStats {
  total: number;
  totalReceitas: number;
  totalDespesas: number;
  saldo: number;
}
```

---

## 🔥 Exemplo Completo

```tsx
import { useState, useEffect } from 'react';
import { useTransactionStore } from '@/entities/transaction';
import {
  AddTransactionForm,
  EditTransactionForm,
  TransactionFilterPanel,
  useFilterTransactions,
  type TransactionFilters
} from '@/features';

export const TransactionsPage = () => {
  const { transactions, fetchTransactions } = useTransactionStore();
  const [showAddForm, setShowAddForm] = useState(false);
  const [editingTransaction, setEditingTransaction] = useState(null);
  const [filters, setFilters] = useState<TransactionFilters>({});

  const { filteredTransactions, stats } = useFilterTransactions(
    transactions,
    filters
  );

  useEffect(() => {
    fetchTransactions();
  }, [fetchTransactions]);

  return (
    <div className="page">
      <h1>Transações</h1>

      {/* Filtros */}
      <TransactionFilterPanel
        filters={filters}
        onFiltersChange={setFilters}
        onClearFilters={() => setFilters({})}
      />

      {/* Stats */}
      <div className="stats">
        <div>Total: {stats.total}</div>
        <div>Receitas: R$ {stats.totalReceitas.toFixed(2)}</div>
        <div>Despesas: R$ {stats.totalDespesas.toFixed(2)}</div>
        <div>Saldo: R$ {stats.saldo.toFixed(2)}</div>
      </div>

      {/* Add Button */}
      <button onClick={() => setShowAddForm(true)}>
        ➕ Nova Transação
      </button>

      {/* Add Form */}
      {showAddForm && (
        <AddTransactionForm
          onSuccess={() => {
            setShowAddForm(false);
            fetchTransactions();
          }}
          onCancel={() => setShowAddForm(false)}
        />
      )}

      {/* Edit Form */}
      {editingTransaction && (
        <EditTransactionForm
          transaction={editingTransaction}
          onSuccess={() => {
            setEditingTransaction(null);
            fetchTransactions();
          }}
          onCancel={() => setEditingTransaction(null)}
        />
      )}

      {/* List */}
      <div className="list">
        {filteredTransactions.map((t) => (
          <div key={t.id} className="card">
            <h3>{t.descricao}</h3>
            <p>R$ {t.valor}</p>
            <button onClick={() => setEditingTransaction(t)}>
              ✏️ Editar
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};
```

---

## ✅ Checklist de Uso

### Antes de usar uma feature:

- [ ] Importar a feature de `@/features`
- [ ] Verificar se os stores necessários estão configurados
- [ ] Garantir que as entidades relacionadas existem
- [ ] Adicionar handlers de `onSuccess` e `onCancel`

### Componentes de Formulário:

Todos os formulários aceitam as mesmas props:

```typescript
interface FormProps {
  entity?: EntityType;        // Para edição (opcional para add)
  onSuccess?: () => void;     // Callback após sucesso
  onCancel?: () => void;      // Callback para cancelar
}
```

### Hooks:

Todos os hooks retornam o mesmo padrão:

```typescript
const { 
  featureAction,    // Função principal
  isLoading,        // Estado de carregamento
  error             // Mensagem de erro
} = useFeatureName();
```

---

## 🎨 Estilos CSS

Todos os componentes têm classes CSS consistentes:

```css
/* Container do formulário */
.add-transaction-form
.edit-transaction-form
.edit-category-form
.edit-account-form
.transaction-filter-panel

/* Grupos de campos */
.form-row
.form-group

/* Mensagens */
.error-message
.form-error

/* Ações */
.form-actions
.btn
.btn--primary
.btn--secondary
```

---

## 🔗 Links Úteis

- [Documentação Completa](./FEATURES_DOCUMENTATION.md)
- [Exemplos de Stores](./STORES_USAGE_EXAMPLES.md)
- [Guia FSD](./FSD_EXAMPLES.md)
- [Forms Refactoring](./FORMS_REFACTORING.md)

---

**Última Atualização:** 17 de outubro de 2025
