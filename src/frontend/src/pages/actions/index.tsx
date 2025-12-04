import React from 'react';
import ActionItemColumn from 'pages/actions/ActionItemColumn';
import CircleLoader from 'components/shared/loading/circleLoader';
import { useGetTeamActionItemsQuery } from 'utils/redux/api/goalApiSlice';
import { TeamActionItemsResponse } from 'library/goalAggregate/queries/TeamActionItemsResponse';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { ApiResponse } from 'utils/redux/apiResponse';
import stringDateCompare from 'library/shared/stringDateCompare';

const Actions = () => {
  const {
    data: getTeamActionItemsResponseData,
    isLoading: getTeamActionItemsResponseIsLoading,
    isError: getTeamActionItemsResponseIsError,
    isSuccess: getTeamActionItemsResponseIsSuccess
  }: {
    data?: ApiResponse<TeamActionItemsResponse[]>,
    isLoading: boolean,
    isError: boolean,
    isSuccess: boolean,
  } = useGetTeamActionItemsQuery(getTeamIdFromLocalStorage());

  if (getTeamActionItemsResponseIsLoading) return <CircleLoader />;

  if (getTeamActionItemsResponseIsError) return <p>An error occurred while loading your goals</p>;

  if (getTeamActionItemsResponseIsSuccess) {
    if (getTeamActionItemsResponseData?.value != null) {
      const actionItems = getTeamActionItemsResponseData.value;

      const today = new Date();
      const laterDueDate = new Date(new Date(today).setDate(today.getDate() + 30));

      return (
        <>
          <div className='flex flex-col pb-4 ml-4 mr-4 max-h-full h-full items-center'>
            <div className='font-bold pb-5 text-4xl' >Action Items</div>
            <div className='flex items-start p-0 overflow-x-auto h-full max-h-full'>
              <ActionItemColumn title='No Due Date' actionItems={ actionItems.filter(item => !item.isComplete && item.dueDate === null ) } />
              <ActionItemColumn title='Due Later' actionItems={ actionItems.filter(item => !item.isComplete && stringDateCompare(item.dueDate, laterDueDate) === 1) } />
              <ActionItemColumn title='Due Soon' actionItems={ actionItems.filter(item => !item.isComplete && stringDateCompare(item.dueDate, laterDueDate) === -1 && stringDateCompare(item.dueDate, today) > 0) } />
              <ActionItemColumn title='Due Now' actionItems={ actionItems.filter(item => !item.isComplete && stringDateCompare(item.dueDate, today) <= -1) } />
              <ActionItemColumn title='Completed' actionItems={ actionItems.filter(item => item.isComplete) } />
            </div>
        </div>
        </>
    );
    }
  }
};

export default Actions;