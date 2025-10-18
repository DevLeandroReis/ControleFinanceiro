// Transaction entity types
export type TransactionType = 'income' | 'expense';

// Backend enums mapping
export const TipoLancamento = {
  Receita: 1,
  Despesa: 2,
} as const;

export type TipoLancamento = (typeof TipoLancamento)[keyof typeof TipoLancamento];

export const StatusLancamento = {
  Pendente: 1,
  Pago: 2,
  Cancelado: 3,
  Vencido: 4,
} as const;

export type StatusLancamento = (typeof StatusLancamento)[keyof typeof StatusLancamento];

export const TipoRecorrencia = {
  Nenhuma: 0,
  Diaria: 1,
  Semanal: 2,
  Mensal: 3,
  Anual: 4,
} as const;

export type TipoRecorrencia = (typeof TipoRecorrencia)[keyof typeof TipoRecorrencia];

export interface CreateTransactionInput {
  descricao: string;
  valor: number;
  dataVencimento: string;
  tipo: TipoLancamento;
  observacoes?: string;
  ehRecorrente?: boolean;
  tipoRecorrencia?: TipoRecorrencia;
  quantidadeParcelas?: number;
  categoriaId: string;
  contaId: string;
}

export interface UpdateTransactionInput {
  descricao: string;
  valor: number;
  dataVencimento: string;
  tipo: TipoLancamento;
  observacoes?: string;
  ehRecorrente?: boolean;
  tipoRecorrencia?: TipoRecorrencia;
  quantidadeParcelas?: number;
  categoriaId: string;
  contaId: string;
}

export interface Transaction {
  id: string;
  descricao: string;
  valor: number;
  dataVencimento: string;
  dataPagamento?: string | null;
  tipo: TipoLancamento;
  status: StatusLancamento;
  observacoes?: string | null;
  ehRecorrente: boolean;
  tipoRecorrencia: TipoRecorrencia;
  quantidadeParcelas?: number | null;
  parcelaAtual?: number | null;
  lancamentoPaiId?: string | null;
  categoriaId: string;
  categoriaNome?: string | null;
  contaId: string;
  contaNome?: string | null;
  usuarioId?: string;
  createdAt?: string;
  updatedAt?: string;
}

export interface TransactionState {
  transactions: Transaction[];
  isLoading: boolean;
  error: string | null;
  fetchTransactions: () => Promise<void>;
  addTransaction: (data: CreateTransactionInput) => Promise<void>;
  updateTransaction: (id: string, data: UpdateTransactionInput) => Promise<void>;
  deleteTransaction: (id: string) => Promise<void>;
  marcarComoPago: (id: string, dataPagamento?: string) => Promise<void>;
  marcarComoPendente: (id: string) => Promise<void>;
  cancelar: (id: string) => Promise<void>;
  setTransactions: (transactions: Transaction[]) => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearTransactions: () => void;
}
