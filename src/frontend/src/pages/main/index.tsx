import UltimateGoals from 'components/ultimateGoal';
import React from 'react';
import { ApiResponse } from 'utils/redux/apiResponse';
import TeamList from 'components/team/teamList';
import DeviceList from 'components/device/deviceList';
import CircleLoader from 'components/shared/loading/circleLoader';
import { useGetTeamByIdQuery } from 'utils/redux/api/teamApiSlice';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { TeamByIdResponse } from 'library/teamAggregate/queries/TeamByIdResponse';

const Main = () => {
  const {
    data: getTeamByIdResponseData,
    isLoading: getTeamByIdResponseIsLoading,
    isError: getTeamByIdResponseIsError,
    isSuccess: getTeamByIdResponseIsSuccess
  }: {
    data?: ApiResponse<TeamByIdResponse>,
    isLoading: boolean,
    isError: boolean,
    isSuccess: boolean,
  } = useGetTeamByIdQuery(getTeamIdFromLocalStorage());

  if (getTeamByIdResponseIsLoading) return (<CircleLoader />);

  if (getTeamByIdResponseIsError || getTeamByIdResponseData?.isFailure) return (<p>An error occurred while attempting to load your team's data.</p>);

  if (getTeamByIdResponseIsSuccess && getTeamByIdResponseData?.value) {
    const teamId = getTeamByIdResponseData.value.id;

    return (
      <>
        <div className='flex flex-col pb-4 ml-4 max-h-full h-full'>
          <div className='font-bold self-center pb-5 text-4xl' >Your Internet Safety Center</div>
          <div className='flex flex-row'>
            <div className='w-2/3 m-4'>
              <UltimateGoals teamId={ teamId } />
            </div>
            <div className='flex flex-col m-4 w-1/3'>
              <div className='pb-4'>
                <TeamList team={ getTeamByIdResponseData.value } />
              </div>
              <div>
                <DeviceList teamId={ teamId } />
              </div>
            </div>
          </div>
        </div>
      </>
    );
  }

  return (<></>);
};

export default Main;