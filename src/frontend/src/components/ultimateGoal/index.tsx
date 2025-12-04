import React from 'react';
import { useGetTeamUltimateGoalsWithGoalsAndActionsQuery } from 'utils/redux/api/ultimateGoalApiSlice';
import { FiEdit2 } from 'react-icons/fi';
import { ApiResponse } from 'utils/redux/apiResponse';
import UltimateGoalAccordion from 'components/ultimateGoal/accordion';
import { useNavigate } from 'react-router-dom';
import { UltimateGoalsWithGoalsAndActionsResponse as UltimateGoal } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsWithGoalsAndActionsResponse';

type Props = {
  teamId: string;
}

const UltimateGoals = (props: Props) => {
  const { teamId } = props;

  const navigate = useNavigate();

  const {
    data: getTeamUltimateGoalsResponseData,
    isLoading: getTeamUltimateGoalsResponseIsLoading,
    isError: getTeamUltimateGoalsResponseIsError,
    isSuccess: getTeamUltimateGoalsResponseIsSuccess
  }: {
    data?: ApiResponse<UltimateGoal[]>,
    isLoading: boolean,
    isError: boolean,
    isSuccess: boolean,
  } = useGetTeamUltimateGoalsWithGoalsAndActionsQuery(teamId);

  const handleSettingsClick = () => {
    navigate('/settings/ultimate-goals');
  };

  if (getTeamUltimateGoalsResponseIsLoading) return <p>Loading Ultimate Goals...</p>;

  if (getTeamUltimateGoalsResponseIsError || getTeamUltimateGoalsResponseData?.isFailure) return <p>An error occurred while trying to load your team's Ultimate Goals.</p>;

  if (getTeamUltimateGoalsResponseIsSuccess && getTeamUltimateGoalsResponseData?.isSuccess) {
    const ultimateGoals = getTeamUltimateGoalsResponseData.value;

    if (ultimateGoals === null) return (<p>No Ultimate Goals associated with your Team. Create one now!</p>);

    const ultimateGoalAccordions = ultimateGoals.map(function(ultimateGoal) {
      return (
        <UltimateGoalAccordion ultimateGoal={ ultimateGoal } key={ ultimateGoal.id } />
      );
    });

    return (
      <>
        <div className='border-2 border-gray-100 p-4 rounded-lg'>
          <div className='flex flex-row justify-between'>
            <h2 className='text-lg font-bold pb-2'>My Ultimate Goals</h2>
            <FiEdit2 className='cursor-pointer w-6 h-6 mr-1' onClick={ handleSettingsClick } />
          </div>
          { ultimateGoalAccordions }
        </div>
      </>
    );
  }

  return (
    <></>
  );
};

export default UltimateGoals;