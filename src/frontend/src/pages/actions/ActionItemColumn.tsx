import React from 'react';
import { TeamActionItemsResponse } from 'library/goalAggregate/queries/TeamActionItemsResponse';
import ActionItemCard from 'components/actionItem/card';

type Props = {
  title: string;
  actionItems: TeamActionItemsResponse[];
}

const ActionItemColumn = (props: Props) => {
  const { title, actionItems } = props;

  const actionItemCards: JSX.Element[] = [];
  actionItems?.forEach((actionItem) => {
    actionItemCards.push(
      <div className='w-full' key={ actionItem.id }>
        <ActionItemCard actionItem={ actionItem } goalId={ actionItem.goalId } goalName={ actionItem.goalName } showGoal={ false } />
      </div>
    );
  });

  return (
    <>
      <div className='flex flex-col p-2 w-1/4 border-2 border-gray-400 m-2 bg-gray-200 rounded-lg min-w-[200px] overflow-x-auto h-5/6 max-h-full'>
        <h1 className='text-xl font-semibold mb-4'>{ title }</h1>
        <div className='max-h-full overflow-y-auto'>
          { actionItemCards }
        </div>
      </div>
    </>
  );
};

export default ActionItemColumn;
