import { CreateEditGoalAndActionItems_ActionItem } from 'library/goalAggregate/CreateEditGoalAndActionItem';
import React, { useEffect, useState } from 'react';

type Props = {
  actionItem: CreateEditGoalAndActionItems_ActionItem;
  updateActionItem: (updatedActionItem: CreateEditGoalAndActionItems_ActionItem) => void;
};

const ActionItemForm = (props: Props) => {
  const { actionItem, updateActionItem } = props;
  const [editedActionItem, setEditedActionItem] = useState<CreateEditGoalAndActionItems_ActionItem>({
    id: actionItem.id,
    name: actionItem.name,
    description: actionItem.description,
    dueDate: actionItem?.dueDate !== null && actionItem?.dueDate !== undefined && actionItem.dueDate != '0001-01-01T00:00:00' //TODO: Return something else, or make global function to check for min date
    ?  new Date().toISOString().slice(0, 10)
    :  new Date().toISOString().slice(0, 10)
  });

  useEffect(() => {
    handleUpdate();
  }, [editedActionItem]);

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    setEditedActionItem({
      ...editedActionItem,
      [name]: value
    });
  };

  const handleUpdate = () => {
    updateActionItem(editedActionItem);
  };

  return (
    <div>
      <div className='mb-4 flex flex-col items-start'>
        <label className='px-4 text-sm'>Title</label>
        <input
          name='name'
          className='mb-2 outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-md text-black border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
          value={ editedActionItem.name ?? '' }
          onChange={ handleInputChange }
        />
      </div>
      <div className='mb-4 flex flex-col items-start'>
        <label className='px-4 text-sm'>Description</label>
        <textarea
          name='description'
          className='mb-2 outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-md text-black border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
          rows={ 5 }
          value={ editedActionItem.description ?? '' }
          onChange={ handleInputChange }
        />
      </div>
      <div className='flex flex-col items-start justify-start mb-4'>
        <label className='text-sm pr-9 pl-4'>Due Date</label>
        <input
          className='outline-0 outline rounded-[7px] text-black text-md border-blue-gray-200 px-4 py-1 border border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
          name='dueDate'
          type='date'
          value={ editedActionItem.dueDate ?? '' }
          onChange={ handleInputChange }
        />
      </div>
    </div>
  );
};

export default ActionItemForm;