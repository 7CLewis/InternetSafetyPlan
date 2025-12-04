import React from 'react';
import TeamSetupForm from 'pages/initial-setup/team-setup/form';
import { User } from 'library/userAggregate/User';

type Props = {
  user: User;
}

const TeamSetup = (props: Props) => {
  const { user } = props;

  return (
    <>
      <div className='w-full flex-col align-middle justify-center max-w-md'>
        <p>Let's set up your team.</p>
        <TeamSetupForm user={ user } />
      </div>
    </>
  );
};

export default TeamSetup;