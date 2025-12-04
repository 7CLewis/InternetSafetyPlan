import React, { useState } from 'react';
import {
  Button,
  Dialog,
  DialogBody,
  DialogHeader
} from '@material-tailwind/react';
import {  updateFormData } from 'utils/redux/forms/devices/deviceFormSlice';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from 'utils/redux/store';
import { RxCross1 } from 'react-icons/rx';
import DeviceType from 'library/deviceAggregate/DeviceType';
import TagInput from 'components/device/tagInput';
import TeammateDropdown from 'components/device/teammateDropdown';
import CircleLoader from 'components/shared/loading/circleLoader';
import { TeamByIdResponse } from 'library/teamAggregate/queries/TeamByIdResponse';
import { useGetTeamByIdQuery } from 'utils/redux/api/teamApiSlice';
import { ApiResponse } from 'utils/redux/apiResponse';
import { Teammate } from 'library/teamAggregate/Teammate';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import DeviceDataFetcher from 'components/device/deviceDataFetcher';
import { useCreateDeviceMutation, useEditDeviceMutation } from 'utils/redux/api/deviceApiSlice';
import { Device } from 'library/deviceAggregate/Device';

type Props = {
  deviceId: string;
  open: boolean;
  handleClose: (isSubmit?: boolean) => void;
  onDeviceAdd?: (device: Device) => void;
  onDeviceEdit?: (device: Device) => void;
};

const DeviceForm = (props: Props) => {
  const { deviceId, open, handleClose, onDeviceAdd, onDeviceEdit } = props;
  const dispatch = useDispatch();
  const formData = useSelector((state: RootState) => state.deviceForm.formData);
  const [deviceDataReceived, setDeviceDataReceived] = useState<boolean>(false);
  let teammates: Teammate[] = [];

  const [createDevice] = useCreateDeviceMutation();
  const [editDevice] = useEditDeviceMutation();

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value, type } = event.currentTarget;
    const updatedValue = type === 'checkbox' ? (event.currentTarget as HTMLInputElement).checked : value;

    dispatch(updateFormData({ [name]: updatedValue }));
  };

  const handleSubmit = () => {
    if ( deviceId == 'new') {
      createDevice({ id: formData.id, teamId: getTeamIdFromLocalStorage(), name: formData.name, nickname: formData.nickname, type: formData.type, teammateIds: formData.teammateIds, tags: formData.tags })
        .unwrap()
        .then((response) => {
          console.log('Device created successfully. ' + JSON.stringify(response));

          const newDevice = new Device(response, getTeamIdFromLocalStorage(), formData.name, formData.nickname, formData.type, formData.teammateIds, formData.tags);
          if (onDeviceAdd) onDeviceAdd(newDevice);

          handleClose(true);
          return response;
        })
        .catch((error) => {
          console.log(error);
        });
    } else {
      editDevice({ id: formData.id, teamId: getTeamIdFromLocalStorage(), name: formData.name, nickname: formData.nickname, type: formData.type, teammateIds: formData.teammateIds, tags: formData.tags })
        .unwrap()
        .then((response) => {
          console.log('Device updated successfully. ' + JSON.stringify(response));
          const editedDevice = new Device(deviceId, getTeamIdFromLocalStorage(), formData.name, formData.nickname, formData.type, formData.teammateIds, formData.tags);
          if (onDeviceEdit) onDeviceEdit(editedDevice);

          handleClose(true);
          return response;
        })
        .catch((error) => {
          console.log(error);
        });
    }

    handleClose(true);
  };

  const handleTagChange = (tags: string[]) => {
    dispatch(updateFormData({ ['tags']: tags }));
  };

  const handleTeammateChange = (teammateIds: string[]) => {
    dispatch(updateFormData({ ['teammateIds']: teammateIds }));
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

  const handleDeviceDataReceived = () => {
    setDeviceDataReceived(true);
  };

  if (getTeamByIdIsLoading) return <CircleLoader />;

  if (getTeamByIdIsError) return <p>An error occurred while trying to load teammates</p>;

  if (getTeamByIdIsSuccess) {
    if (getTeamByIdResponseData?.value != null) teammates = getTeamByIdResponseData?.value?.teammates;

    if (deviceId != 'new' && !deviceDataReceived) {
      return (
        <>
          <DeviceDataFetcher deviceId={ deviceId } onDeviceDataReceived= { handleDeviceDataReceived } />
          <CircleLoader />;
        </>
      );
    } else {
      return (
        <>
          <div>
            <Dialog
              open={ open }
              handler={ handleClose }
              className=' bg-white shadow-xl rounded-md text-center'
            >
              <DialogHeader className='pb-0'>
                <div className='flex w-full flex-row justify-between items-center'>
                  <p className='outline-0 outline rounded-[7px] border mr-4 px-4 py-1 text-xl border-transparent'>{ deviceId == 'new' ? 'Add' : 'Edit' } Device</p>
                  <RxCross1 className='cursor-pointer m-2 text-red-600 stroke-1 hover:stroke-2' onClick={ () => handleClose() }>X</RxCross1>
                </div>
              </DialogHeader>
              <DialogBody className='pt-0'>
                <div className='w-full'>
                  <div className='mt-4 mb-4 flex flex-col items-start'>
                    <label className='px-4 text-sm'>Name</label>
                    <input
                      name='name'
                      type='text'
                      value={ formData.name }
                      placeholder='Enter Device Name'
                      onChange={ handleChange }
                      className='outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-black text-md border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                    />
                  </div>
                  <div className='mt-4 mb-4 flex flex-col items-start'>
                    <label className='px-4 text-sm'>Nickname</label>
                    <input
                      name='nickname'
                      type='text'
                      value={ formData.nickname }
                      placeholder='Enter Device Nickname'
                      onChange={ handleChange }
                      className='outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-black text-md border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                    />
                  </div>
                  <div className='flex flex-col justify-start items-start mb-5'>
                    <label className='px-4 text-sm'>Device Type</label>
                    <select
                      name='type'
                      className='mb-2 outline-0 outline rounded-[7px] border mr-4 px-3 py-1 text-md text-black border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                      value={ formData.type }
                      onChange={ handleChange }
                    >
                      <option value={ 0 } disabled>
                        Select a device type
                      </option>
                      { Object.keys(DeviceType).map((id) => (
                        <option key={ id } value={ id } >
                          { DeviceType[parseInt(id, 10)] }
                        </option>
                      )) }
                    </select>
                  </div>
                  <div className='flex flex-col justify-start items-start mb-5'>
                    <label className='px-4 text-sm'>Device Users</label>
                    <TeammateDropdown teammates={ teammates } associatedTeammateIds={ formData.teammateIds } onTeammateChange={ handleTeammateChange } />
                  </div>
                  <div className='flex flex-col justify-start items-start mb-5'>
                    <TagInput tagList={ formData.tags } onTagChange={ handleTagChange } />
                  </div>
                  <div className='flex flex-row justify-start p-4'>
                    <div className='mr-2'>
                      <Button
                        variant='outlined'
                        color='gray'
                        onClick={ () => handleClose() }
                        className='mr-2'
                      >
                        Cancel
                      </Button>
                    </div>
                    <div className='mr-2'>
                      <Button
                        variant='outlined'
                        onClick={ handleSubmit }
                        className='mr-2'
                        color='green'
                      >
                        Save
                      </Button>
                    </div>
                  </div>
                </div>
              </DialogBody>
            </Dialog>
          </div>
        </>
      );
    }
  }
};

export default DeviceForm;