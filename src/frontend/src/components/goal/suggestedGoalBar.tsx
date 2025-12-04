import { Goal } from 'library/goalAggregate/Goal';
import React from 'react';
import { FaCheck } from 'react-icons/fa';

type Props = {
  goal: Goal;
  onGoalSelect: (newGoalId: string) => void;
};

const SuggestedGoalBar = (props: Props) => {
  const { goal, onGoalSelect } = props;

  const handleGoalSelect = (goalId: string) => {
    onGoalSelect(goalId);
  };

  return (
    <div className='border mb-2 border-gray-300 rounded p-2 bg-gray-100 cursor-pointer' onClick={ () => handleGoalSelect(goal.id) }>
      <li
        key={ goal.id }
        className='flex items-center justify-between'
      >
        <div className='flex items-center'>
          <span className='pl-2'>{ goal.name }</span>
        </div>
        <div className='flex flex-row items-center'>
          <FaCheck size={ 24 } />
          <p className='pl-2 text-2xl'>{ goal.actionItems?.length ?? 0 }</p>
        </div>
      </li>
  </div>
  );
};

export default SuggestedGoalBar;