import type { FC } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useUserStore, forgotPasswordSchema, type ForgotPasswordInput } from '../../entities/user';
import './ForgotPasswordPage.css';

export const ForgotPasswordPage: FC = () => {
  const navigate = useNavigate();
  const { forgotPassword, isLoading, error, successMessage } = useUserStore();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<ForgotPasswordInput>({
    resolver: zodResolver(forgotPasswordSchema),
    defaultValues: {
      email: '',
    },
  });

  const onSubmit = async (data: ForgotPasswordInput) => {
    try {
      await forgotPassword(data);
    } catch (error) {
      console.error('Erro ao solicitar recuperação de senha:', error);
    }
  };

  return (
    <div className="forgot-password-page">
      <div className="forgot-password-container">
        <div className="forgot-password-header">
          <h1>🔑 Recuperar Senha</h1>
          <p>Digite seu email para receber as instruções de recuperação</p>
        </div>

        <form className="forgot-password-form" onSubmit={handleSubmit(onSubmit)}>
          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              id="email"
              type="email"
              {...register('email')}
              placeholder="seu@email.com"
              disabled={isSubmitting}
              autoComplete="email"
            />
            {errors.email && (
              <span className="error-message">{errors.email.message}</span>
            )}
          </div>

          {error && (
            <div className="form-error" role="alert">
              {error}
            </div>
          )}

          {successMessage && (
            <div className="form-success" role="status">
              {successMessage}
            </div>
          )}

          <button 
            type="submit" 
            className="btn btn--primary btn--full" 
            disabled={isSubmitting || isLoading}
          >
            {isSubmitting || isLoading ? '⏳ Enviando...' : '📧 Enviar instruções'}
          </button>
        </form>

        <div className="forgot-password-footer">
          <button className="btn btn--link" onClick={() => navigate('/login')}>
            ← Voltar para o login
          </button>
        </div>
      </div>
    </div>
  );
};
