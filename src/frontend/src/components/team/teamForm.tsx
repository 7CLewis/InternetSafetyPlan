import React, { useState } from 'react';
import {
  Button,
  Dialog,
  DialogBody,
  DialogHeader
} from '@material-tailwind/react';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from 'utils/redux/store';
import { RxCross1 } from 'react-icons/rx';
import CircleLoader from 'components/shared/loading/circleLoader';
import { TeamByIdResponse } from 'library/teamAggregate/queries/TeamByIdResponse';
import { useGetTeamByIdQuery } from 'utils/redux/api/teamApiSlice';
import { ApiResponse } from 'utils/redux/apiResponse';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { useUpdateTeamInformationMutation } from 'utils/redux/api/teamApiSlice';
import { updateFormData } from 'utils/redux/forms/teams/teamFormSlice';
import TeamDataFetcher from 'components/team/teamDataFetcher';

type Props = {
  teamId: string;
  open: boolean;
  handleClose: (teamName?: string, isSubmit?: boolean) => void;
};

const TeamForm = (props: Props) => {
  const { teamId, open, handleClose } = props;
  const dispatch = useDispatch();
  const formData = useSelector((state: RootState) => state.teamForm.formData);
  const [teamDataReceived, setTeamDataReceived] = useState<boolean>(false);

  const [updateTeamInformation] = useUpdateTeamInformationMutation();

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement >) => {
    const { name, value } = event.currentTarget;

    dispatch(updateFormData({ [name]: value }));
  };

  const handleSubmit = () => {
    updateTeamInformation({ teamId: getTeamIdFromLocalStorage(), name: formData.name, description: formData.description })
    .unwrap()
    .then((response) => {
      console.log('Team updated successfully. ' + JSON.stringify(response));

      handleClose(formData.name, true);
      return response;
    })
    .catch((error) => {
      console.log(error);
    });

    handleClose(undefined, true);
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

  const handleTeamDataReceived = () => {
    setTeamDataReceived(true);
  };

  if (getTeamByIdIsLoading) return <CircleLoader />;

  if (getTeamByIdIsError) return <p>An error occurred while trying to load teammates</p>;

  if (getTeamByIdIsSuccess && getTeamByIdResponseData?.value != null) {
    if (!teamDataReceived) {
      return (
        <>
          <TeamDataFetcher teamId={ teamId } onTeamDataReceived={ handleTeamDataReceived } />
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
                  <p className='outline-0 outline rounded-[7px] border mr-4 px-4 py-1 text-xl border-transparent'>Edit Team</p>
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
                      placeholder='Enter Team Name'
                      onChange={ handleChange }
                      className='outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-black text-md border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                    />
                  </div>
                  <div className='mt-4 mb-4 flex flex-col items-start'>
                    <label className='px-4 text-sm'>Description</label>
                    <textarea
                      name='description'
                      className='mb-2 outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-md border-blue-gray-200 text-black hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                      rows={ 5 }
                      value={ formData.description }
                      onChange={ handleChange }
                    />
                  </div>
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
              </DialogBody>
            </Dialog>
          </div>
        </>
      );
    }
  }
};

export default TeamForm;