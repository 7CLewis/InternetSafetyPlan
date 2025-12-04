import DeviceCardItem from 'components/device/deviceCardItem';
import CircleLoader from 'components/shared/loading/circleLoader';
import { TeamDevicesResponse } from 'library/deviceAggregate/queries/TeamDevicesResponse';
import React, { useEffect, useState } from 'react';
import { useGetTeamDevicesQuery } from 'utils/redux/api/deviceApiSlice';
import { ApiResponse } from 'utils/redux/apiResponse';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { Button } from '@material-tailwind/react';
import DeviceForm from 'components/device/deviceForm';
import { Device } from 'library/deviceAggregate/Device';

const Devices = () => {
  const [open, setOpen] = React.useState(false);
  const [teamDevices, setTeamDevices] = useState<TeamDevicesResponse[]>([]);

  const handleClose = (isSubmit = false) => {
    if (isSubmit) {
      setOpen(false);
      return;
    }

    const result = window.confirm('Leave without saving changes to this Device?');
    if (result) {
      setOpen(false);
    }
  };

  const handleOpen = () => {
      setOpen(true);
  };

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
  } = useGetTeamDevicesQuery(getTeamIdFromLocalStorage());

  useEffect(() => {
    if (getTeamDevicesResponseData?.value != null) {
      const devices = getTeamDevicesResponseData.value;
      setTeamDevices(devices);
    }
  }, [getTeamDevicesResponseData]);

  const handleDeleteDevice = (deviceId: string) => {
    const updatedDevices = teamDevices.filter(device => device.id !== deviceId);
    setTeamDevices(updatedDevices);
  };

  const handleAddedDevice = (device: Device) => {
    const updatedDevices = teamDevices.concat(device);
    setTeamDevices(updatedDevices);
  };

  const handleDeviceEdit = (editedDevice: Device) => {
    const updatedDevices = teamDevices.filter(device => device.id !== editedDevice.id);
    setTeamDevices(updatedDevices.concat(editedDevice));
  };

  if (getTeamDevicesResponseIsLoading) return <CircleLoader />;

  if (getTeamDevicesResponseIsError) return <p>An error occurred while loading your devices</p>;

  if (getTeamDevicesResponseIsSuccess) {
    if (getTeamDevicesResponseData?.value != null) {
      const devices: JSX.Element[] = [];
      teamDevices.forEach((device) => {
        devices.push(
          <DeviceCardItem device={ device } key={ device.id } onDelete={ () => handleDeleteDevice(device.id) } onDeviceEdit={ handleDeviceEdit } />
        );
      });

      return (
        <>
          <div className='flex flex-col items-center pb-28'>
            <div className='font-bold pb-5 text-4xl' >Devices</div>
            <Button color='blue-gray' className='p-3 mb-3' onClick={ handleOpen }>+ Add Device</Button>
            { open && <DeviceForm deviceId='new' handleClose={ handleClose } open={ open } key='new' onDeviceAdd={ handleAddedDevice } /> }
            { devices }
          </div>
        </>
      );
    } else {
      return (
        <>
          <div className='flex flex-col'>
            <p>No devices found.</p>
          </div>
        </>
      );
    }
  }
};

export default Devices;