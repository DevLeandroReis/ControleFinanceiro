import type { FC } from 'react';
import { useRouteError, isRouteErrorResponse, Link } from 'react-router-dom';
import './NotFoundPage.css';

export const NotFoundPage: FC = () => {
  const error = useRouteError();
  
  let errorMessage: string;
  
  if (isRouteErrorResponse(error)) {
    errorMessage = error.statusText;
  } else if (error instanceof Error) {
    errorMessage = error.message;
  } else {
    errorMessage = 'Erro desconhecido';
  }

  return (
    <div className="not-found-page">
      <div className="error-content">
        <h1>404</h1>
        <h2>Página não encontrada</h2>
        <p>{errorMessage}</p>
        <Link to="/" className="btn btn--primary">
          ← Voltar para Home
        </Link>
      </div>
    </div>
  );
};
