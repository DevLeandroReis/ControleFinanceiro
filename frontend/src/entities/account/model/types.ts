// Account entity types
export type AccountType = 'corrente' | 'poupanca' | 'investimento' | 'carteira';

export interface Account {
  id: string;
  nome: string;
  tipo: AccountType;
  saldoInicial: number;
  saldoAtual: number;
  cor?: string;
  usuarioId: string;
}

export interface AccountState {
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
