import type { FC } from 'react';
import { useNavigate, useSearchParams } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useUserStore, resetPasswordSchema, type ResetPasswordInput } from '../../entities/user';
import { AlertTriangle } from 'lucide-react';
import './ResetPasswordPage.css';

export const ResetPasswordPage: FC = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();
  const token = searchParams.get('token') || '';
  
  const { resetPassword, isLoading, error, successMessage } = useUserStore();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<ResetPasswordInput>({
    resolver: zodResolver(resetPasswordSchema),
    defaultValues: {
      token,
      novaSenha: '',
      confirmarNovaSenha: '',
    },
  });

  const onSubmit = async (data: ResetPasswordInput) => {
    try {
      await resetPassword(data);
      // Redirect to login after successful password reset
      setTimeout(() => {
        navigate('/login');
      }, 3000);
    } catch (error) {
      console.error('Erro ao redefinir senha:', error);
    }
  };

  if (!token) {
    return (
      <div className="reset-password-page">
        <div className="reset-password-container">
          <div className="reset-password-header">
            <h1><AlertTriangle size={32} style={{ display: 'inline', marginRight: '10px', verticalAlign: 'middle' }} /> Token Inválido</h1>
            <p>O link de redefinição de senha é inválido ou expirou.</p>
          </div>
          <div className="reset-password-footer">
            <button className="btn btn--link" onClick={() => navigate('/forgot-password')}>
              Solicitar novo link
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="reset-password-page">
      <div className="reset-password-container">
        <div className="reset-password-header">
          <h1>🔒 Nova Senha</h1>
          <p>Digite sua nova senha</p>
        </div>

        <form className="reset-password-form" onSubmit={handleSubmit(onSubmit)}>
          <input type="hidden" {...register('token')} />

          <div className="form-group">
            <label htmlFor="novaSenha">Nova senha</label>
            <input
              id="novaSenha"
              type="password"
              {...register('novaSenha')}
              placeholder="Mínimo 6 caracteres"
              disabled={isSubmitting}
              autoComplete="new-password"
            />
            {errors.novaSenha && (
              <span className="error-message">{errors.novaSenha.message}</span>
            )}
          </div>

          <div className="form-group">
            <label htmlFor="confirmarNovaSenha">Confirmar nova senha</label>
            <input
              id="confirmarNovaSenha"
              type="password"
              {...register('confirmarNovaSenha')}
              placeholder="Repita sua nova senha"
              disabled={isSubmitting}
              autoComplete="new-password"
            />
            {errors.confirmarNovaSenha && (
              <span className="error-message">{errors.confirmarNovaSenha.message}</span>
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
            {isSubmitting || isLoading ? '⏳ Redefinindo...' : '✅ Redefinir senha'}
          </button>
        </form>

        <div className="reset-password-footer">
          <button className="btn btn--link" onClick={() => navigate('/login')}>
            ← Voltar para o login
          </button>
        </div>
      </div>
    </div>
  );
};
