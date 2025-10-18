import { apiClient } from '@/shared/api';
import type { Transaction, TipoLancamento, StatusLancamento, TipoRecorrencia } from './types';

export interface CreateTransactionDTO {
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

export interface UpdateTransactionDTO {
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

export const transactionApi = {
  /**
   * Busca todas as transações do usuário
   */
  async getAll(): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>('/api/Lancamentos');
    return response.data;
  },

  /**
   * Busca uma transação por ID
   */
  async getById(id: string): Promise<Transaction> {
    const response = await apiClient.get<Transaction>(`/api/Lancamentos/${id}`);
    return response.data;
  },

  /**
   * Busca transações por período e contas
   */
  async getByPeriod(dataInicio: string, dataFim: string, contaIds: string[]): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>('/api/Lancamentos/periodo', {
      params: { dataInicio, dataFim, contaIds }
    });
    return response.data;
  },

  /**
   * Busca transações por categoria e contas
   */
  async getByCategory(categoriaId: string, contaIds: string[]): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>(`/api/Lancamentos/categoria/${categoriaId}`, {
      params: { contaIds }
    });
    return response.data;
  },

  /**
   * Busca transações por conta
   */
  async getByAccount(contaId: string): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>(`/api/Lancamentos/conta/${contaId}`);
    return response.data;
  },

  /**
   * Busca transações por tipo e contas
   */
  async getByType(tipo: TipoLancamento, contaIds: string[]): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>(`/api/Lancamentos/tipo/${tipo}`, {
      params: { contaIds }
    });
    return response.data;
  },

  /**
   * Busca transações por status e contas
   */
  async getByStatus(status: StatusLancamento, contaIds: string[]): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>(`/api/Lancamentos/status/${status}`, {
      params: { contaIds }
    });
    return response.data;
  },

  /**
   * Busca transações vencidas
   */
  async getVencidos(contaIds: string[]): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>('/api/Lancamentos/vencidos', {
      params: { contaIds }
    });
    return response.data;
  },

  /**
   * Busca transações recorrentes
   */
  async getRecorrentes(contaIds: string[]): Promise<Transaction[]> {
    const response = await apiClient.get<Transaction[]>('/api/Lancamentos/recorrentes', {
      params: { contaIds }
    });
    return response.data;
  },

  /**
   * Obter saldo por período
   */
  async getSaldoPorPeriodo(dataInicio: string, dataFim: string, contaIds: string[]): Promise<number> {
    const response = await apiClient.get<number>('/api/Lancamentos/saldo', {
      params: { dataInicio, dataFim, contaIds }
    });
    return response.data;
  },

  /**
   * Obter total de receitas por período
   */
  async getTotalReceitas(dataInicio: string, dataFim: string, contaIds: string[]): Promise<number> {
    const response = await apiClient.get<number>('/api/Lancamentos/receitas/total', {
      params: { dataInicio, dataFim, contaIds }
    });
    return response.data;
  },

  /**
   * Obter total de despesas por período
   */
  async getTotalDespesas(dataInicio: string, dataFim: string, contaIds: string[]): Promise<number> {
    const response = await apiClient.get<number>('/api/Lancamentos/despesas/total', {
      params: { dataInicio, dataFim, contaIds }
    });
    return response.data;
  },

  /**
   * Cria uma nova transação
   */
  async create(data: CreateTransactionDTO): Promise<Transaction> {
    const response = await apiClient.post<Transaction>('/api/Lancamentos', data);
    return response.data;
  },

  /**
   * Cria transações recorrentes
   */
  async createRecorrentes(data: CreateTransactionDTO): Promise<Transaction[]> {
    const response = await apiClient.post<Transaction[]>('/api/Lancamentos/recorrentes', data);
    return response.data;
  },

  /**
   * Atualiza uma transação existente
   */
  async update(id: string, data: UpdateTransactionDTO): Promise<Transaction> {
    const response = await apiClient.put<Transaction>(`/api/Lancamentos/${id}`, data);
    return response.data;
  },

  /**
   * Atualiza transação recorrente (pai e filhos futuros)
   */
  async updateRecorrente(id: string, data: UpdateTransactionDTO): Promise<Transaction[]> {
    const response = await apiClient.put<Transaction[]>(`/api/Lancamentos/${id}/recorrente`, data);
    return response.data;
  },

  /**
   * Gera lançamentos futuros para um lançamento pai
   */
  async gerarFuturos(id: string): Promise<Transaction[]> {
    const response = await apiClient.post<Transaction[]>(`/api/Lancamentos/${id}/gerar-futuros`);
    return response.data;
  },

  /**
   * Remove uma transação
   */
  async delete(id: string): Promise<void> {
    await apiClient.delete<void>(`/api/Lancamentos/${id}`);
  },

  /**
   * Marca transação como paga
   */
  async marcarComoPago(id: string, dataPagamento?: string): Promise<Transaction> {
    const response = await apiClient.patch<Transaction>(`/api/Lancamentos/${id}/pagar`, dataPagamento);
    return response.data;
  },

  /**
   * Marca transação como pendente
   */
  async marcarComoPendente(id: string): Promise<Transaction> {
    const response = await apiClient.patch<Transaction>(`/api/Lancamentos/${id}/pendente`);
    return response.data;
  },

  /**
   * Cancela transação
   */
  async cancelar(id: string): Promise<Transaction> {
    const response = await apiClient.patch<Transaction>(`/api/Lancamentos/${id}/cancelar`);
    return response.data;
  }
};
