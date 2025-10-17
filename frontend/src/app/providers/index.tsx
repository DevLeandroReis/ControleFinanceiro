import { RouterProvider } from 'react-router-dom';
import { router } from '../router';

export const AppProvider = () => {
  return <RouterProvider router={router} />;
};
