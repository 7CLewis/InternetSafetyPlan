import React, { useState } from 'react';
import { useParams } from 'react-router-dom';
import GoalForm from 'components/goal/goalForm';
import SuggestedGoals from 'components/goal/suggestedGoals';

const GoalPage = () => {
  const { goalId } = useParams();
  const [ currentGoalId, setGoalId ] = useState(goalId);

  const handleGoalSelect = (newGoalId: string) => {
    setGoalId(newGoalId);
  };

  return (
    <>
    <div className='flex flex-col'>
      <div className='font-bold self-center pb-5 text-4xl' >Add New Goal</div>
      <div className='flex flex-row'>
        <div className='w-2/3'>
          <GoalForm goalId={ currentGoalId ?? 'new' } />
        </div>
        <div className='w-1/3'>
          { currentGoalId == 'new' && <SuggestedGoals onGoalSelect= { handleGoalSelect } /> }
        </div>
      </div>
    </div>
    </>
  );
};

export default GoalPage;