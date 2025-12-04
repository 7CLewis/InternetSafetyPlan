import React from 'react';
import { createRoot } from 'react-dom/client';
import 'index.css';
import { AuthProvider } from 'react-oidc-context';
import { oidcConfiguration } from 'utils/auth/auth';
import App from 'App';
import { BrowserRouter } from 'react-router-dom';
import { store } from 'utils/redux/store';
import { Provider as ReduxProvider }  from 'react-redux';
import { worker } from 'msw/worker';
import { ThemeProvider } from '@material-tailwind/react';

if (process.env.NODE_ENV === 'development') {
  void worker.start();
}

// eslint-disable-next-line @typescript-eslint/no-non-null-assertion
createRoot(document.getElementById('root')!).render(
  <>
    <ThemeProvider>
      <ReduxProvider store={ store } >
        <AuthProvider { ...oidcConfiguration } >
          <BrowserRouter>
            <App />
          </BrowserRouter>
        </AuthProvider>
      </ReduxProvider>
    </ThemeProvider>
  </>
);