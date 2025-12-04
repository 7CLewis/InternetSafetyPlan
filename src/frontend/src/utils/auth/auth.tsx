import { User, WebStorageStateStore } from 'oidc-client-ts';
import React, { Fragment, useEffect } from 'react';
import { AuthProviderProps, hasAuthParams, useAuth } from 'react-oidc-context';
import { Outlet } from 'react-router-dom';

export const oidcConfiguration: AuthProviderProps = {
  authority: 'http://localhost:8080/realms/internet-safety',
  client_id: 'plan',
  redirect_uri: window.location.origin + '/login',
  userStore: new WebStorageStateStore({ store: window.localStorage }),

  // eslint-disable-next-line @typescript-eslint/no-unused-vars
  onSigninCallback: (_user: User | void): void => {
    window.history.replaceState(
      {},
      document.title,
      window.location.pathname
    );
  }
};

// Adapted from https://github.com/authts/react-oidc-context/issues/685#issuecomment-1483822510
export function RequireAuth(): JSX.Element {
  const auth = useAuth();

  useEffect(() => {
    if (!hasAuthParams() &&
        !auth.isAuthenticated &&
        !auth.activeNavigator &&
        !auth.isLoading
        ) {
          void auth.signinRedirect();
        }
  }, [
    auth.isAuthenticated,
    auth.activeNavigator,
    auth.isLoading,
    void auth.signinRedirect]);

  return (<Fragment>{ auth.isAuthenticated ? <Outlet /> : (null) }</Fragment>);
}