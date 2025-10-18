import { z } from 'zod';
import { TipoLancamento, TipoRecorrencia } from './types';

// Schema para criar transação
export const createTransactionSchema = z.object({
  descricao: z
    .string()
    .min(3, 'A descrição deve ter no mínimo 3 caracteres')
    .max(200, 'A descrição deve ter no máximo 200 caracteres'),
  valor: z
    .number()
    .positive('O valor deve ser maior que zero'),
  dataVencimento: z
    .string()
    .regex(/^\d{4}-\d{2}-\d{2}$/, 'Data inválida'),
  tipo: z.union([
    z.literal(TipoLancamento.Receita),
    z.literal(TipoLancamento.Despesa)
  ], {
    message: 'Tipo deve ser receita ou despesa',
  }),
  observacoes: z.string().max(1000, 'As observações devem ter no máximo 1000 caracteres').optional(),
  ehRecorrente: z.boolean().optional().default(false),
  tipoRecorrencia: z.union([
    z.literal(TipoRecorrencia.Nenhuma),
    z.literal(TipoRecorrencia.Diaria),
    z.literal(TipoRecorrencia.Semanal),
    z.literal(TipoRecorrencia.Mensal),
    z.literal(TipoRecorrencia.Anual)
  ]).optional().default(TipoRecorrencia.Nenhuma),
  quantidadeParcelas: z.number().int().positive().optional(),
  categoriaId: z.string().min(1, 'Selecione uma categoria'),
  contaId: z.string().min(1, 'Selecione uma conta'),
});

// Schema para atualizar transação
export const updateTransactionSchema = z.object({
  descricao: z
    .string()
    .min(3, 'A descrição deve ter no mínimo 3 caracteres')
    .max(200, 'A descrição deve ter no máximo 200 caracteres'),
  valor: z
    .number()
    .positive('O valor deve ser maior que zero'),
  dataVencimento: z
    .string()
    .regex(/^\d{4}-\d{2}-\d{2}$/, 'Data inválida'),
  tipo: z.union([
    z.literal(TipoLancamento.Receita),
    z.literal(TipoLancamento.Despesa)
  ]),
  observacoes: z.string().max(1000).optional(),
  ehRecorrente: z.boolean().optional().default(false),
  tipoRecorrencia: z.union([
    z.literal(TipoRecorrencia.Nenhuma),
    z.literal(TipoRecorrencia.Diaria),
    z.literal(TipoRecorrencia.Semanal),
    z.literal(TipoRecorrencia.Mensal),
    z.literal(TipoRecorrencia.Anual)
  ]).optional().default(TipoRecorrencia.Nenhuma),
  quantidadeParcelas: z.number().int().positive().optional(),
  categoriaId: z.string().min(1, 'Selecione uma categoria'),
  contaId: z.string().min(1, 'Selecione uma conta'),
});

// Tipos inferidos (re-exportados dos types.ts para consistência)
export type { CreateTransactionInput, UpdateTransactionInput } from './types';
