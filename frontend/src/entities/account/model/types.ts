// Account entity types
export type AccountType = 'corrente' | 'poupanca' | 'investimento' | 'carteira';

export type RequestStatus = 0 | 1 | 2 | 3;

export const RequestStatusEnum = {
  Pendente: 0 as RequestStatus,
  Aprovada: 1 as RequestStatus,
  Rejeitada: 2 as RequestStatus,
  Cancelada: 3 as RequestStatus,
} as const;

export interface Account {
  id: string;
  nome: string;
  descricao?: string;
  tipo?: AccountType;
  saldoInicial?: number;
  saldoAtual?: number;
  cor?: string;
  usuarioId?: string;
  ativa: boolean;
  proprietarioId: string;
  proprietarioNome: string;
  usuarios: UserAccount[];
  createdAt: string;
  updatedAt: string;
}

export interface UserAccount {
  id: string;
  usuarioId: string;
  usuarioNome: string;
  usuarioEmail: string;
  contaId: string;
  contaNome: string;
  podeAdicionarUsuarios: boolean;
  ativo: boolean;
  dataAdesao: string;
}

export interface AccessRequest {
  id: string;
  solicitanteId: string;
  solicitanteNome: string;
  solicitanteEmail: string;
  proprietarioId: string;
  proprietarioNome: string;
  contaId: string;
  contaNome: string;
  status: RequestStatus;
  mensagem?: string;
  dataResposta?: string;
  createdAt: string;
}

export interface CreateAccountInput {
  nome: string;
  descricao?: string;
}

export interface UpdateAccountInput {
  nome?: string;
  descricao?: string;
}

export interface AccountState {
  accounts: Account[];
  receivedRequests: AccessRequest[];
  sentRequests: AccessRequest[];
  isLoading: boolean;
  error: string | null;
  
  // Account CRUD operations
  fetchAccounts: () => Promise<void>;
  addAccount: (account: CreateAccountInput) => Promise<void>;
  updateAccount: (id: string, account: UpdateAccountInput) => Promise<void>;
  deleteAccount: (id: string) => Promise<void>;
  activateAccount: (id: string) => Promise<void>;
  deactivateAccount: (id: string) => Promise<void>;
  
  // Access request operations
  requestAccess: (contaId: string, mensagem?: string) => Promise<void>;
  approveRequest: (solicitacaoId: string) => Promise<void>;
  rejectRequest: (solicitacaoId: string) => Promise<void>;
  cancelRequest: (solicitacaoId: string) => Promise<void>;
  fetchReceivedRequests: () => Promise<void>;
  fetchSentRequests: () => Promise<void>;
  
  // User management operations
  getAccountUsers: (contaId: string) => Promise<UserAccount[]>;
  removeUserFromAccount: (contaId: string, usuarioId: string) => Promise<void>;
  grantPermission: (contaId: string, usuarioId: string) => Promise<void>;
  revokePermission: (contaId: string, usuarioId: string) => Promise<void>;
  
  // State management
  setAccounts: (accounts: Account[]) => void;
  setLoading: (isLoading: boolean) => void;
  setError: (error: string | null) => void;
  clearAccounts: () => void;
  getTotalBalance: () => number;
}
