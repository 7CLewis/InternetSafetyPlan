import React from 'react';
import { Outlet } from 'react-router-dom';
import { Header } from 'components/base';
import { useAuth } from 'react-oidc-context';
import Sidebar from 'components/base/sidebar';
import CircleLoader from 'components/shared/loading/circleLoader';

const Layout = () => {
  const auth = useAuth();

  let mainContent = <Outlet />;

  switch (auth.activeNavigator) {
    case 'signinSilent':
      mainContent = <CircleLoader text='Signing you in...' />;
    break;
    case 'signoutRedirect':
      mainContent = <CircleLoader text='Signing you out...' />;
    break;
  }

  if (auth.error) {
    mainContent = <div>An authentication error occurred: { auth.error.message }</div>;
  }

  return (
    <>
      <div className='flex flex-col h-screen max-h-screen'>
        <div>
          <Header />
        </div>
        <div className='flex flex-row flex-1' style={ { height: 'calc(100vh - var(--header-height))' } }>
          <Sidebar></Sidebar>
          <div className='p-8 max-h-full h-full w-full overflow-y-auto'>
            { mainContent }
          </div>
        </div>
      </div>
    </>
  );
};

export default Layout;