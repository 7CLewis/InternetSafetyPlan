import React from 'react';
import GoalBar from 'components/goal/goalBar';
import { ApiResponse } from 'utils/redux/apiResponse';
import { useGetTeamGoalsQuery } from 'utils/redux/api/goalApiSlice';
import { TeamGoalsResponse } from 'library/goalAggregate/queries/TeamGoalsResponse';
import CircleLoader from 'components/shared/loading/circleLoader';
import { Button } from '@material-tailwind/react';
import { Link } from 'react-router-dom';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';

const Goals = () => {
  const {
    data: getTeamGoalsResponseData,
    isLoading: getTeamGoalsResponseIsLoading,
    isError: getTeamGoalsResponseIsError,
    isSuccess: getTeamGoalsResponseIsSuccess
  }: {
    data?: ApiResponse<TeamGoalsResponse[]>,
    isLoading: boolean,
    isError: boolean,
    isSuccess: boolean,
  } = useGetTeamGoalsQuery(getTeamIdFromLocalStorage());

  if (getTeamGoalsResponseIsLoading) return <CircleLoader />;

  if (getTeamGoalsResponseIsError) return <p>An error occurred while loading your goals</p>;

  if (getTeamGoalsResponseIsSuccess) {
    if (getTeamGoalsResponseData?.value != null) {
      const goals: JSX.Element[] = [];
      getTeamGoalsResponseData.value.forEach((goal) => {
        goals.push(
          <GoalBar goal={ goal } key={ goal.id } />
        );
      });

      return (
        <>
          <div className='flex flex-col items-center'>
            <div className='font-bold pb-5 text-4xl' >Goals</div>
            <Link to='/goals/new'>
              <Button color='blue-gray' className='p-3'>+ Add New Goal</Button>
            </Link>
            { goals }
          </div>
        </>
      );
    } else {
      return (
        <>
          <div className='flex flex-col'>
            <p>No goals found.</p>
          </div>
        </>
      );
    }
  }
};

export default Goals;