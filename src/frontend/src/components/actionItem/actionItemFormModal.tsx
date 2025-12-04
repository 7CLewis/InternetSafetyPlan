import React, { useState } from 'react';
import {
  Button,
  Checkbox,
  Dialog,
  DialogBody,
  DialogHeader
} from '@material-tailwind/react';
import { updateFormData } from 'utils/redux/forms/goals/actionItemFormSlice';
import { useDispatch, useSelector } from 'react-redux';
import { RootState } from 'utils/redux/store';
import { RxCross1 } from 'react-icons/rx';
import { Link } from 'react-router-dom';
import { useEditActionItemMutation } from 'utils/redux/api/goalApiSlice';
import ActionItemDataFetcher from 'components/actionItem/actionItemDataFetcher';
import CircleLoader from 'components/shared/loading/circleLoader';

type Props = {
  actionItemId: string;
  goalId: string;
  open: boolean;
  handleClose: (isSubmit?: boolean) => void;
};

const ActionItemFormModal = (props: Props) => {
  const { actionItemId, goalId, open, handleClose } = props;
  const dispatch = useDispatch();
  const formData = useSelector((state: RootState) => state.actionItemForm.formData);
  const [editActionItem] = useEditActionItemMutation();
  const [actionItemDataReceived, setActionItemDataReceived] = useState<boolean>(false);

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value, type } = event.currentTarget;
    const updatedValue = type === 'checkbox' ? (event.currentTarget as HTMLInputElement).checked : value;

    dispatch(updateFormData({ [name]: updatedValue }));
  };

  const handleSubmit = () => {
    editActionItem({ goalId: goalId, actionItemId: actionItemId, name: formData.name, description: formData.description, dueDate: formData.dueDate })
      .unwrap()
      .then((response) => {
        console.log('Action Item ' + actionItemId + ' updated successfully. ');
        handleClose(true);
        return response;
      })
      .catch((error) => {
        console.log(error);
      });

    handleClose(true);
  };

  const handleActionItemDataReceived = () => {
    setActionItemDataReceived(true);
  };

  if (actionItemId != 'new' && !actionItemDataReceived) {
    return (
      <>
        <ActionItemDataFetcher goalId= { goalId } actionItemId={ actionItemId } onActionItemDataReceived= { handleActionItemDataReceived } />
        <CircleLoader />;
      </>
    );
  } else {
    console.log(formData.dueDate);
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
                <input
                  name='name'
                  type='text'
                  value={ formData.name }
                  onChange={ handleChange }
                  className='outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-xl border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500' />
                  <RxCross1 className='cursor-pointer m-2 text-red-600 stroke-1 hover:stroke-2' onClick={ () => handleClose() }>X</RxCross1>
              </div>
            </DialogHeader>
            <DialogBody className='pt-0'>
              <div className='w-full'>
                { actionItemId != 'new' &&
                  <div className='flex flex-row justify-start items-center px-4 mb-5 mt-2 text-sm'>
                    <Link to='/goals' className='underline'>Go to Goals page</Link>
                  </div>
                }
                <div className='mb-4 flex flex-col items-start'>
                  <label className='px-4 text-sm'>Description</label>
                  <textarea
                    name='description'
                    className='mb-2 outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-md text-black border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                    rows={ 5 }
                    value={ formData.description }
                    onChange={ handleChange }
                  />
                </div>
                <div className='flex flex-col items-start justify-start mb-4'>
                  <label className='text-sm pr-9 pl-4'>Due Date</label>
                  <input
                    className='outline-0 outline rounded-[7px] text-black text-md border-blue-gray-200 px-4 py-1 border border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                    name='dueDate'
                    type='date'
                    value={ new Date(formData.dueDate).toISOString().split('T')[0] ?? '' }
                    onChange={ handleChange }
                  />
                </div>
                <div className='flex flex-col items-start'>
                  <label className='px-4 text-sm'>Completed</label>
                  <div className='pl-1'>
                    <Checkbox className='p-0 pl-4' defaultChecked={ formData.isComplete } />
                  </div>
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
};

export default ActionItemFormModal;