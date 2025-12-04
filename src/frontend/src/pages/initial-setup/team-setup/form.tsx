import React, { useState } from 'react';
import { setTeamName, setTeamDescription, selectTeamName, selectTeamDescription } from 'pages/initial-setup/team-setup/slice';
import { useCreateTeamMutation } from 'utils/redux/api/teamApiSlice';
import { useSelector, useDispatch } from 'react-redux';
import { User } from 'library/userAggregate/User';
import { setTeamIdInLocalStorage } from 'utils/local-storage/localStorage';
import { Navigate } from 'react-router-dom';

type Props = {
  user: User;
}

const TeamSetupForm = (props: Props) => {
  const { user } = props;

  const [resultStatus, setResultStatus] = useState<string>();

  const [createTeam] = useCreateTeamMutation();

  const name = useSelector(selectTeamName);
  const description = useSelector(selectTeamDescription);

  const dispatch = useDispatch();

  const handleSubmit = (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setResultStatus('loading');

    createTeam({ userEmail: user.email.value, name: name, description: description != '' ? description : null })
      .unwrap()
      .then((response) => {
        console.log('Team created successfully. ' + JSON.stringify(response));
        setTeamIdInLocalStorage(response);
        setResultStatus('success');
      })
      .catch((error) => {
        console.log(error);
        setResultStatus('error');
      });
  };

  if (resultStatus == 'error') return (<p>An error occurred while creating the team.</p>);
  if (resultStatus == 'success') return <Navigate to='/' />;

  return (
    <>
      <form onSubmit={ handleSubmit } className='bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4'>
        <div className='mb-4'>
          <label className='block text-gray-700 text-sm font-bold mb-2' htmlFor='teamName'>
            Team Name
          </label>
          <input
            className='shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline'
            id='teamName'
            type='text'
            placeholder='Vivacious Pandas!'
            value={ name }
            onChange={ (e) => dispatch(setTeamName(e.target.value)) }
          />
        </div>
        <div className='mb-4'>
          <label className='block text-gray-700 text-sm font-bold mb-2' htmlFor='teamDescription'>
            Description
          </label>
          <small className='text-sm'>Additional information you want to include about your team.</small>
          <input
            className='shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline'
            id='teamDescription'
            type='text'
            placeholder='We are the best!'
            value={ description }
            onChange={ (e) => dispatch(setTeamDescription(e.target.value)) }
          />
        </div>
        <div className='flex items-center justify-between'>
          <button
            className='bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline'
            type='submit'
          >
            { resultStatus != 'loading' ? 'Save and Continue' : 'Saving information...' }
          </button>
        </div>
      </form>
    </>
  );
};

export default TeamSetupForm;