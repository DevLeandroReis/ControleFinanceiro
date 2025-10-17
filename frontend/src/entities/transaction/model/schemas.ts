import { z } from 'zod';

// Schema para criar transação
export const createTransactionSchema = z.object({
  descricao: z
    .string()
    .min(3, 'A descrição deve ter no mínimo 3 caracteres')
    .max(200, 'A descrição deve ter no máximo 200 caracteres'),
  valor: z
    .number()
    .positive('O valor deve ser maior que zero'),
  data: z
    .string()
    .regex(/^\d{4}-\d{2}-\d{2}$/, 'Data inválida'),
  tipo: z.enum(['income', 'expense'], {
    message: 'Tipo deve ser receita ou despesa',
  }),
  categoriaId: z.string().min(1, 'Selecione uma categoria'),
  contaId: z.string().min(1, 'Selecione uma conta'),
  usuarioId: z.string().optional(),
});

// Schema para atualizar transação
export const updateTransactionSchema = createTransactionSchema.partial();

// Tipos inferidos
export type CreateTransactionInput = z.infer<typeof createTransactionSchema>;
export type UpdateTransactionInput = z.infer<typeof updateTransactionSchema>;
