import { UltimateGoalsWithGoalsAndActionsResponse_Goal_Action } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsWithGoalsAndActionsResponse';
import React, { useState } from 'react';
import ActionItemFormModal from 'components/actionItem/actionItemFormModal';
import ToggleSwitch from 'components/shared/toggle/toggleSwitch';
import { useToggleActionItemCompletionMutation } from 'utils/redux/api/goalApiSlice';

type Props = {
  actionItem: UltimateGoalsWithGoalsAndActionsResponse_Goal_Action;
  goalId: string;
  goalName: string;
  showGoal: boolean;
  onCompletionToggle?: (actionItemId: string, newStatus: boolean) => void;
}

const ActionItemCard = (props: Props) => {
  const { actionItem, goalId, goalName, showGoal, onCompletionToggle } = props;
  const [open, setOpen] = useState<boolean>(false);
  const [toggleActionItemCompletion] = useToggleActionItemCompletionMutation();
  const [isComplete, setIsComplete] = useState(actionItem.isComplete);

  const handleClose = (isSubmit = false) => {
    if (isSubmit) {
      setOpen(false);
      window.location.reload();
      return;
    }

    const result = window.confirm('Leave without saving changes to this Goal?');
    if (result) {
      setOpen(false);
    }
  };

  const handleOpen = () => {
      setOpen(true);
  };

  const handleToggle = () => {
    const newCompletionStatus = !isComplete;
    setIsComplete(newCompletionStatus);
    toggleActionItemCompletion({goalId: goalId, actionItemId: actionItem.id})
    .unwrap()
    .then((response) => {
      console.log('Action item completion toggled successfully. ' + JSON.stringify(response));
      if (onCompletionToggle) onCompletionToggle(actionItem.id, newCompletionStatus);
      return response;
    })
    .catch((error) => {
      console.log(error);
    });
  };

  return (
    <>
      <div className='px-2'>
        <div className={ `rounded-lg shadow-md p-4 mt-6 ${ isComplete ? 'bg-gray-300 opacity-70' : 'bg-white' }` }>
          <div className='flex items-start'>
            <div className='flex-shrink-0'>
              <ToggleSwitch initialToggleValue={ isComplete } onToggle={ handleToggle } />
            </div>
            <div className='ml-4'>
              <h2 className='font-bold'><span className='hover:text-blue-700 hover:cursor-pointer' onClick={ handleOpen }>{ actionItem.name }</span></h2>
              { open && <ActionItemFormModal key={ actionItem.id } goalId={ goalId } actionItemId={ actionItem.id } handleClose={ handleClose } open={ open } /> }
              { showGoal && (
                <p className='text-sm'>Goal: { goalName }</p>
              ) }
              <p className='text-gray-500 text-xs mt-2'>Due: { actionItem.dueDate != null ? new Date(actionItem.dueDate).toDateString() : '[No Due Date]' }</p>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default ActionItemCard;