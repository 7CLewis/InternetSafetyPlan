import { Team } from 'library/teamAggregate/Team';
import React from 'react';
import { Link } from 'react-router-dom';
import TeammateBar from 'components/team/teammateBar';

type Props = {
  team: Team
}

const TeamList = (props: Props) => {
  const { team } = props;
  const teammates = team.teammates;

  return (
    <>
      <div className='p-4 border rounded-lg'>
        <div className='flex justify-between items-center mb-4'>
          <h2 className='font-bold text-xl'>{ team.name }</h2>
          <Link to='/team' className='underline'>
            Manage Team
          </Link>
        </div>
        <ul>
          {
            teammates.map((teammate) => (
              <div key={ teammate.id }>
                <TeammateBar teammate={ teammate } />
              </div>
            ))
          }
        </ul>
      </div>
    </>
  );
};

export default TeamList;