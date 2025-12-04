import React from 'react';
import { TeamDevicesResponse } from 'library/deviceAggregate/queries/TeamDevicesResponse';
import { useGetTeamDevicesQuery } from 'utils/redux/api/deviceApiSlice';
import { ApiResponse } from 'utils/redux/apiResponse';
import { Link } from 'react-router-dom';
import DeviceBar from 'components/device/deviceBar';

type Props = {
  teamId: string;
}

const DeviceList = (props: Props) => {
  const { teamId } = props;

  const {
    data: getTeamDevicesResponseData,
    isLoading: getTeamDevicesResponseIsLoading,
    isError: getTeamDevicesResponseIsError,
    isSuccess: getTeamDevicesResponseIsSuccess
  }: {
    data?: ApiResponse<TeamDevicesResponse[]>,
    isLoading: boolean,
    isError: boolean,
    isSuccess: boolean,
  } = useGetTeamDevicesQuery(teamId);

  if (getTeamDevicesResponseIsLoading) return <p>Loading...</p>;

  if (getTeamDevicesResponseIsError) return <p>An error occurred while loading your devices</p>;

  if (getTeamDevicesResponseIsSuccess) {
    if (getTeamDevicesResponseData?.value != null) {
      const devices: JSX.Element[] = [];
      getTeamDevicesResponseData.value.forEach((device) => {
        devices.push(
          <div key={ device.id }>
            <DeviceBar device={ device } />
          </div>
        );
      });

      return (
        <>
          <div className='p-4 border rounded-lg'>
            <div className='flex justify-between items-center mb-4'>
              <h2 className='font-bold text-xl'>Devices</h2>
              <Link to='/devices' className='underline'>
                Manage Devices
              </Link>
            </div>
            <ul key={ '2' }>
              { devices }
            </ul>
          </div>
        </>
      );
    }
  }
};

export default DeviceList;