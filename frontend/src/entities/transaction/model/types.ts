// Transaction entity types
export type TransactionType = 'income' | 'expense';

export interface Transaction {
  id: string;
  descricao: string;
  valor: number;
  data: string;
  tipo: TransactionType;
  categoriaId: string;
  contaId: string;
  usuarioId: string;
}

export interface TransactionState {
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
