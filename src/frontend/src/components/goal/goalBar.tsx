import ActionItemCard from 'components/actionItem/card';
import ProgressBar from 'components/shared/progress/progressBar';
import { Goal } from 'library/goalAggregate/Goal';
import React, { useState } from 'react';
import { IconContext } from 'react-icons';
import { FiChevronDown, FiChevronUp } from 'react-icons/fi';
import { Link } from 'react-router-dom';

type Props = {
  goal: Goal;
}

const GoalBar = (props: Props) => {
  const { goal } = props;
  const [isExpanded, setIsExpanded] = useState(false);
  const [completedCount, setCompletedCount] = useState(goal.actionItems?.reduce((count, x) => count + (x.isComplete ? 1 : 0), 0) ?? 0);

  const handleArrowClick = () => {
    setIsExpanded((prevState) => !prevState);
  };

  const handleActionItemCompletionToggle = (actionItemId: string, newStatus: boolean) => {
    newStatus ? setCompletedCount(completedCount + 1) : setCompletedCount(completedCount - 1);
  };

  const actionItemCards: JSX.Element[] = [];
  goal.actionItems?.forEach((actionItem) => {
    actionItemCards.push(
      <div className='w-1/4' key={ actionItem.id } >
        <ActionItemCard actionItem={ actionItem } onCompletionToggle={ handleActionItemCompletionToggle } goalId={ goal.id } goalName={ goal.name } showGoal={ false } />
      </div>
    );
  });

  return (
    <>
      <div className='flex flex-row justify-center'>
        <div className='bg-white rounded-md shadow-md p-4 pb-0 w-[1200px] m-4 mb-0'>
          <ProgressBar
            completedCount={ completedCount }
            totalCount={ goal.actionItems?.length ?? 0 }
            dataType='action items'
            key={ goal.id }
          />
          <h2 className='text-xl font-bold'><Link className='hover:text-blue-700 hover:cursor-pointer' to={ goal.id }>{ goal.name }</Link></h2>
          <p className='text-gray-500 mt-0'>{ goal.description }</p>
          <p className='text-gray-500 text-xs mt-5'>Due: { goal.dueDate != null ? new Date(goal.dueDate).toDateString() : '[No Due Date]' }</p>

          <div className='flex flex-row w-full justify-start'>

            { isExpanded && (
              <div className='flex w-full mt-5'>
                { actionItemCards }
              </div>
            ) }
          </div>

          <div className='flex flex-row justify-center mt-3 mb-0'>
          <IconContext.Provider value={ { style: { verticalAlign: 'bottom' } } }>
            { isExpanded ? (
              <FiChevronUp
                className='text-gray-500 hover:text-gray-700 cursor-pointer'
                size={ 42 }
                onClick={ handleArrowClick }
              />
            ) : (
              <FiChevronDown
                className='text-gray-500 hover:text-gray-700 cursor-pointer align-bottom'
                size={ 42 }
                onClick={ handleArrowClick } />
            ) }
            </IconContext.Provider>
          </div>
        </div>
      </div>
    </>
  );
};

export default GoalBar;