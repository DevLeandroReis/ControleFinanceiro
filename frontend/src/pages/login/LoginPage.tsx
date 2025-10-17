import type { FC } from 'react';
import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useUserStore } from '../../entities/user';
import { loginSchema, type LoginInput } from '../../entities/user/model';
import './LoginPage.css';

export const LoginPage: FC = () => {
  const navigate = useNavigate();
  const { login, isLoading, error } = useUserStore();

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginInput>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: '',
      senha: '',
    },
  });

  const onSubmit = async (data: LoginInput) => {
    try {
      await login(data);
      navigate('/dashboard');
    } catch (error) {
      console.error('Erro ao fazer login:', error);
    }
  };

  return (
    <div className="login-page">
      <div className="login-container">
        <div className="login-header">
          <h1>💰 Controle Financeiro</h1>
          <p>Faça login para acessar sua conta</p>
        </div>

        <form className="login-form" onSubmit={handleSubmit(onSubmit)}>
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

          <div className="form-group">
            <label htmlFor="senha">Senha</label>
            <input
              id="senha"
              type="password"
              {...register('senha')}
              placeholder="••••••••"
              disabled={isSubmitting}
              autoComplete="current-password"
            />
            {errors.senha && (
              <span className="error-message">{errors.senha.message}</span>
            )}
          </div>

          {error && (
            <div className="form-error" role="alert">
              {error}
            </div>
          )}

          <button type="submit" className="btn btn--primary btn--full" disabled={isSubmitting || isLoading}>
            {isSubmitting || isLoading ? '⏳ Entrando...' : '🔓 Entrar'}
          </button>
        </form>

        <div className="login-footer">
          <p>Ainda não tem uma conta?</p>
          <button className="btn btn--link" onClick={() => navigate('/register')}>
            Criar conta
          </button>
        </div>
      </div>
    </div>
  );
};
