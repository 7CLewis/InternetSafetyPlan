import React from 'react';
import UserSetup from 'pages/initial-setup/user-setup';
import TeamSetup from 'pages/initial-setup/team-setup';
import { Navigate, useLocation } from 'react-router-dom';
import { User } from 'library/userAggregate/User';

export enum InitialSetupOption {
  User = 1,
  Team = 2
}

export interface InitialSetupState {
  user: User | null;
  type: InitialSetupOption;
}

const InitialSetup = () => {
  const location = useLocation();
  const state = (location.state as InitialSetupState);
  const type = state?.type;
  const user = state?.user;

  switch (type) {
    case InitialSetupOption.User:
      return <UserSetup />;
    case InitialSetupOption.Team:
      if (user) return <TeamSetup user={ user } />;
      else return <p>Error: User setup must be completed before Team Setup can occur.</p>;
    default:
      return <Navigate to='/login' />;
  }
};

export default InitialSetup;