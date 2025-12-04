import React, { useEffect, useState } from 'react';
import TeammateCard from 'pages/team/teammateCard';
import { useGetTeamByIdQuery } from 'utils/redux/api/teamApiSlice';
import { TeamByIdResponse, TeamByIdResponse_Teammate } from 'library/teamAggregate/queries/TeamByIdResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import CircleLoader from 'components/shared/loading/circleLoader';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { Button } from '@material-tailwind/react';
import TeammateForm from 'components/team/teammateForm';
import { Teammate } from 'library/teamAggregate/Teammate';
import { FaPen } from 'react-icons/fa';
import TeamForm from 'components/team/teamForm';

const Team = () => {
  const [teamFormOpen, setTeamFormOpen] = React.useState(false);
  const handleTeamFormOpen = () => {
    setTeamFormOpen(true);
  };
  const handleTeamFormClose = (teamName: string | undefined = undefined, isSubmit: boolean | undefined = false) => {
    if (teamName !== undefined) setTeamName(teamName);
    if (isSubmit) {
      setTeamFormOpen(false);
      return;
    }
    const result = window.confirm('Leave without saving changes to this Team?');
    if (result) {
      setTeamFormOpen(false);
    }
  };

  const [teammateFormOpen, setTeammateFormOpen] = React.useState(false);
  const handleTeammateFormOpen = () => {
    setTeammateFormOpen(true);
  };
  const handleTeammateFormClose = (isSubmit = false) => {
    if (isSubmit) {
      setTeammateFormOpen(false);
      return;
    }
    const result = window.confirm('Leave without saving changes to this Teammate?');
    if (result) {
      setTeammateFormOpen(false);
    }
  };

  const [teamName, setTeamName] = useState('');
  const [teammates, setTeammates] = useState<TeamByIdResponse_Teammate[]>([]);
  const [isHovered, setIsHovered] = useState(false);

  const handleDeleteTeammate = (teammateId: string) => {
    const updatedTeammates = teammates.filter(teammate => teammate.id !== teammateId);
    setTeammates(updatedTeammates);
  };

  const handleAddedTeammate = (teammate: Teammate) => {
    const updatedTeammates = teammates.concat(teammate);
    setTeammates(updatedTeammates);
  };

  const handleTeammateEdit = (editedTeammate: Teammate) => {
    const updatedTeammates = teammates.filter(teammate => teammate.id !== editedTeammate.id);
    setTeammates(updatedTeammates.concat(editedTeammate));
  };

  const {
    data: getTeamByIdResponseData,
    isLoading: getTeamByIdIsLoading,
    isError: getTeamByIdIsError,
    isSuccess: getTeamByIdIsSuccess
  }: {
    data?: ApiResponse<TeamByIdResponse>,
    isLoading: boolean,
    isError: boolean,
    isSuccess: boolean,
  } = useGetTeamByIdQuery(getTeamIdFromLocalStorage());

  useEffect(() => {
    if (getTeamByIdResponseData?.value != null) {
      const team = getTeamByIdResponseData.value;
      setTeamName(team.name);
      setTeammates(team.teammates);
    }
  }, [getTeamByIdResponseData]);

  if (getTeamByIdIsLoading) return <CircleLoader />;

  if (getTeamByIdIsError) return <p>An error occurred while loading your goals</p>;

  if (getTeamByIdIsSuccess) {
    if (getTeamByIdResponseData?.value != null) {

      const teammateCards: JSX.Element[] = [];

      teammates.forEach((teammate) => {
        teammateCards.push(
          <TeammateCard key={ teammate.id } teammate={ teammate } deviceCount={ Math.floor(Math.random() * 20) } goalCount={ Math.floor(Math.random() * 20) } onDelete={ () => handleDeleteTeammate(teammate.id) } onTeammateEdit={ handleTeammateEdit }/>
        );
      });

      return (
        <>
          <div className='flex flex-col items-center'>
              <div className='font-bold self-center pb-5 text-4xl flex flex-row align-middle'
                onMouseEnter={ () => setIsHovered(true) }
                onMouseLeave={ () => setIsHovered(false) }
              >
                <h1>{ teamName }</h1>
                { isHovered && <FaPen className='ml-2 cursor-pointer' size={ 20 } onClick={ handleTeamFormOpen } /> }
                { teamFormOpen && <TeamForm teamId={ getTeamIdFromLocalStorage() } open={ teamFormOpen } handleClose={ handleTeamFormClose } /> }
              </div>
              <Button color='blue-gray' className='p-3 mb-3' onClick={ handleTeammateFormOpen }>+ Add Teammate</Button>
              { teammateFormOpen && <TeammateForm teammateId='new' handleClose={ handleTeammateFormClose } open={ teammateFormOpen } key='new' onTeammateAdd={ handleAddedTeammate } /> }
              <div className='flex p-0 overflow-x-auto justify-center'>
                { teammateCards }
              </div>
          </div>
        </>
      );
    }
  }
};

export default Team;
