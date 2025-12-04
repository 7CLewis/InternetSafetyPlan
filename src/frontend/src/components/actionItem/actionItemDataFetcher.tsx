import CircleLoader from 'components/shared/loading/circleLoader';
import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { useGetActionItemByIdQuery } from 'utils/redux/api/goalApiSlice';
import { setFormData } from 'utils/redux/forms/goals/actionItemFormSlice';

type Props = {
  goalId: string;
  actionItemId: string;
  onActionItemDataReceived: () => void;
}

const ActionItemDataFetcher = (props: Props) => {
  const { goalId, actionItemId, onActionItemDataReceived } = props;
  const dispatch = useDispatch();

  const {
    data: getActionItemByIdResponseData,
    isLoading: getActionItemByIdResponseIsLoading,
    isError: getActionItemByIdResponseIsError,
    isSuccess: getActionItemByIdResponseIsSuccess
  } = useGetActionItemByIdQuery({ teamId: getTeamIdFromLocalStorage(), goalId: goalId, actionItemId: actionItemId } );

  useEffect(() => {
    if (getActionItemByIdResponseIsSuccess && getActionItemByIdResponseData?.value != null) {
      const actionItem = getActionItemByIdResponseData.value;
      if (actionItem != null) {
        dispatch(
          setFormData({
            formData: {
              goalId: actionItem.goalId,
              goalName: actionItem.goalName,
              actionItemId: actionItem.id,
              name: actionItem.name,
              description: actionItem.description ?? '',
              isComplete: actionItem.isComplete,
              dueDate: actionItem.dueDate ?? ''
            }
          })
        );
      } else {
        dispatch(
          setFormData({
            formData: {
              goalId: '',
              goalName: '',
              actionItemId: '',
              name: '',
              description: '',
              isComplete: false,
              dueDate: ''
            }
          })
        );
      }
      onActionItemDataReceived();
    }
  }, [getActionItemByIdResponseIsSuccess, getActionItemByIdResponseData, dispatch]);

  if (getActionItemByIdResponseIsLoading) return <CircleLoader />;

  if (getActionItemByIdResponseIsError) return <p>An error occurred while retrieving the actionItem data.</p>;

  return null;
};

export default ActionItemDataFetcher;