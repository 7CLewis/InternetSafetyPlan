import React, { useState } from 'react';
import {
  Button,
  Dialog,
  DialogBody,
  DialogHeader
} from '@material-tailwind/react';
import { updateFormData } from 'utils/redux/forms/teams/teammateFormSlice';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from 'utils/redux/store';
import { RxCross1 } from 'react-icons/rx';
import { Link } from 'react-router-dom';
import TeammateDataFetcher from 'components/team/teammateDataFetcher';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { useAddTeammateToTeamMutation, useUpdateTeammateInformationMutation } from 'utils/redux/api/teamApiSlice';
import { Teammate } from 'library/teamAggregate/Teammate';
import Dropzone from 'react-dropzone';
import { FiEdit } from 'react-icons/fi';

type Props = {
  teammateId: string;
  open: boolean;
  handleClose: (isSubmit?: boolean) => void;
  onTeammateAdd?: (teammate: Teammate) => void;
  onTeammateEdit?: (teammate: Teammate) => void;
};

const TeammateForm = (props: Props) => {
  const { teammateId, open, handleClose, onTeammateAdd, onTeammateEdit } = props;
  const dispatch = useDispatch();
  const formData = useSelector((state: RootState) => state.teammateForm.formData);

  const [addTeammateToTeam] = useAddTeammateToTeamMutation();
  const [updateTeammateInformation] = useUpdateTeammateInformationMutation();

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = event.currentTarget;
    const updatedValue = type === 'checkbox' ? (event.currentTarget as HTMLInputElement).checked : value;

    dispatch(updateFormData({ [name]: updatedValue }));
  };

  const handleSubmit = () => {
    if ( teammateId == 'new') {
      addTeammateToTeam({ teamId: getTeamIdFromLocalStorage(), name: formData.name, dateOfBirth: formData.dateOfBirth })
        .unwrap()
        .then((response) => {
          console.log('Teammate added successfully. ' + JSON.stringify(response));
          const newTeammate = new Teammate(response, formData.name, formData.dateOfBirth);

          if (onTeammateAdd) onTeammateAdd(newTeammate);

          handleClose(true);
          return response;
        })
        .catch((error) => {
          console.log(error);
        });
    } else {
      updateTeammateInformation({ teamId: getTeamIdFromLocalStorage(), teammateId: teammateId, name: formData.name, dateOfBirth: formData.dateOfBirth, userId: null })
        .unwrap()
        .then((response) => {
          console.log('Teammate updated successfully. ' + JSON.stringify(response));
          const editedTeammate = new Teammate(teammateId, formData.name, formData.dateOfBirth);
          if (onTeammateEdit) onTeammateEdit(editedTeammate);

          handleClose(true);
          return response;
        })
        .catch((error) => {
          console.log(error);
        });
    }
  };

  const [isHovered, setIsHovered] = useState(false);
  const [isFileUploadModalOpen, setIsFileUploadModalOpen] = useState(false);

  const handleProfilePictureUpload = (picture: File) => {
    console.log(picture);
    setIsFileUploadModalOpen(false);
  };

  const handleEditClick = () => {
    setIsHovered(false);
    setIsFileUploadModalOpen(true);
  };

  return (
    <>
      <TeammateDataFetcher teamId={ getTeamIdFromLocalStorage() } teammateId={ teammateId } />
      <div>
        <Dialog
          open={ open }
          handler={ handleClose }
          className=' bg-white shadow-xl rounded-md text-center'
        >
          <DialogHeader className='pb-0'>
            <div className='flex w-full flex-row justify-between items-center'>
              <p className='outline-0 outline rounded-[7px] border mr-4 px-4 py-1 text-xl border-transparent'>{ teammateId == 'new' ? 'Add' : 'Edit' } Teammate</p>
              <RxCross1 className='cursor-pointer m-2 text-red-600 stroke-1 hover:stroke-2' onClick={ () => handleClose() }>X</RxCross1>
            </div>
          </DialogHeader>
          <DialogBody className='pt-0'>
            <div className='w-full'>
              <div className='flex flex-row justify-start items-center px-4 mt-2 mb-5 text-sm'>
                <Link to='/team' className='underline'>Go to Team page</Link>
              </div>
              <div className='mb-4 flex flex-col items-start'>
                <label className='px-4 text-sm'>Profile Picture</label>
                <div
                    className='relative'
                    onMouseEnter={ () => setIsHovered(true) }
                    onMouseLeave={ () => setIsHovered(false) }
                >
                  <img
                    src={ `src/assets/images/${teammateId}.jpg` }
                    alt={ `${formData.name}'s profile picture` }
                    className={ `w-20 h-20 rounded-full mr-2 ml-4 ${isHovered ? 'opacity-30' : ''}` }
                  />
                  { isHovered && (
                    <div className='absolute inset-0 flex items-center justify-center cursor-pointer mr-2 ml-4' onClick={ handleEditClick }>
                      <FiEdit size={ 24 } color='gray' />
                    </div>
                  ) }
                  { isFileUploadModalOpen && (
                    <div className='fixed top-0 left-0 w-full h-full bg-black bg-opacity-50 flex items-center justify-center'
                      onMouseEnter={ () => setIsHovered(false) }>
                      <div className='bg-white p-4'>
                        <Dropzone
                          multiple={ false }
                          onDropRejected={ msg => console.log('fail: ' + JSON.stringify(msg)) }
                          maxSize={ 50000000 }
                          onDrop={ files => handleProfilePictureUpload(files[0]) }
                        >
                          { ({getRootProps, getInputProps}) => (
                            <section className='border-2 p-2 cursor-pointer'>
                              <div { ...getRootProps() }>
                                <input { ...getInputProps() } />
                                <p>Drag & drop here, or click to browse for a file</p>
                              </div>
                            </section>
                          ) }
                        </Dropzone>
                        <Button
                          variant='outlined'
                          color='gray'
                          onClick={ () => setIsFileUploadModalOpen(false) }
                          className='mr-2 mt-4'
                        >
                          Cancel
                        </Button>
                      </div>
                    </div>
                  ) }
                </div>
              </div>
              <div className='mb-4 flex flex-col items-start'>
                <label className='px-4 text-sm'>Name</label>
                <input type='text'
                  name='name'
                  className='outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-md text-black border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                  value={ formData.name }
                  onChange={ handleChange }
                />
              </div>
              <div className='flex flex-col items-start mb-4'>
                <label className='text-sm pr-9 px-4'>Date Of Birth</label>
                <input
                  className='outline-0 outline rounded-[7px] text-black text-md border-blue-gray-200 px-4 py-1 border border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                  name='dateOfBirth'
                  type='date'
                  value={ formData.dateOfBirth ?? '' }
                  onChange={ handleChange }
                />
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
};

export default TeammateForm;