import { useMemo } from 'react';
import type { Transaction, TransactionType } from '@/entities/transaction';

export interface TransactionFilters {
  tipo?: TransactionType | '';
  categoriaId?: string;
  contaId?: string;
  dataInicio?: string;
  dataFim?: string;
  valorMin?: number;
  valorMax?: number;
  descricao?: string;
}

export const useFilterTransactions = (
  transactions: Transaction[],
  filters: TransactionFilters
) => {
  const filteredTransactions = useMemo(() => {
    let result = [...transactions];

    // Filtrar por tipo
    if (filters.tipo) {
      result = result.filter((t) => t.tipo === filters.tipo);
    }

    // Filtrar por categoria
    if (filters.categoriaId) {
      result = result.filter((t) => t.categoriaId === filters.categoriaId);
    }

    // Filtrar por conta
    if (filters.contaId) {
      result = result.filter((t) => t.contaId === filters.contaId);
    }

    // Filtrar por data de início
    if (filters.dataInicio) {
      result = result.filter((t) => t.data >= filters.dataInicio!);
    }

    // Filtrar por data de fim
    if (filters.dataFim) {
      result = result.filter((t) => t.data <= filters.dataFim!);
    }

    // Filtrar por valor mínimo
    if (filters.valorMin !== undefined && filters.valorMin > 0) {
      result = result.filter((t) => Math.abs(t.valor) >= filters.valorMin!);
    }

    // Filtrar por valor máximo
    if (filters.valorMax !== undefined && filters.valorMax > 0) {
      result = result.filter((t) => Math.abs(t.valor) <= filters.valorMax!);
    }

    // Filtrar por descrição (busca parcial, case-insensitive)
    if (filters.descricao) {
      const searchTerm = filters.descricao.toLowerCase();
      result = result.filter((t) => 
        t.descricao.toLowerCase().includes(searchTerm)
      );
    }

    return result;
  }, [transactions, filters]);

  const stats = useMemo(() => {
    const totalReceitas = filteredTransactions
      .filter((t) => t.tipo === 'income')
      .reduce((sum, t) => sum + Math.abs(t.valor), 0);

    const totalDespesas = filteredTransactions
      .filter((t) => t.tipo === 'expense')
      .reduce((sum, t) => sum + Math.abs(t.valor), 0);

    const saldo = totalReceitas - totalDespesas;

    return {
      total: filteredTransactions.length,
      totalReceitas,
      totalDespesas,
      saldo,
    };
  }, [filteredTransactions]);

  return {
    filteredTransactions,
    stats,
  };
};
