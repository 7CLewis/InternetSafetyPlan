import ActionItemCard from 'components/actionItem/card';
import {
  UltimateGoalsWithGoalsAndActionsResponse as UltimateGoal
} from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsWithGoalsAndActionsResponse';
import React, { useState } from 'react';
import { FaChevronRight } from 'react-icons/fa';

type Props = {
  ultimateGoal: UltimateGoal;
};

const UltimateGoalAccordion = (props: Props) => {
  const [expanded, setExpanded] = useState(false);

  const toggleAccordion = () => {
    setExpanded(!expanded);
  };

  const actionItemCards: JSX.Element[] = [];

  props.ultimateGoal.goals?.forEach((goal) => {
    if (goal.actions) {
      goal.actions.forEach((actionItem) => {
        actionItemCards.push(
          <div className='w-1/3' key={ actionItem.id } >
            <ActionItemCard actionItem={ actionItem } goalId={ goal.id } goalName={ goal.name } showGoal={ true } />
          </div>
        );
      });
    }
  });

  return (
    <>
      <div className='border border-gray-300 rounded p-4 bg-gray-100'>
        <div
          className='flex items-center justify-between cursor-pointer'
          onClick={ toggleAccordion }
        >
          <div className='flex items-center'>
            <FaChevronRight
              className={ `mr-2 transition-transform ${
                expanded ? 'transform rotate-90' : ''
              }` }
            />
            <span className=''>{ props.ultimateGoal.name }</span>
          </div>
        </div>
        { expanded && (
          <div className='flex flex-row w-full flex-wrap'>
            { actionItemCards }
          </div>
        ) }
      </div>
    </>
  );
};

export default UltimateGoalAccordion;
