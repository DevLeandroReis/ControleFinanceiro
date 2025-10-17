import { StrictMode } from 'react';
import { AppProvider } from './providers';
import './styles/index.css';

export const App = () => {
  return (
    <StrictMode>
      <AppProvider />
    </StrictMode>
  );
};
