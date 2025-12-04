import CircleLoader from 'components/shared/loading/circleLoader';
import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useGetGoalByIdQuery } from 'utils/redux/api/goalApiSlice';
import { setFormData } from 'utils/redux/forms/goals/goalFormSlice';
import { v4 as uuidv4 } from 'uuid';

type Props = {
  teamId: string;
  goalId: string;
  onGoalDataReceived: () => void;
}

const GoalDataFetcher = (props: Props) => {
  const { teamId, goalId, onGoalDataReceived } = props;
  const dispatch = useDispatch();

  const {
    data: getGoalByIdResponseData,
    isLoading: getGoalByIdResponseIsLoading,
    isError: getGoalByIdResponseIsError,
    isSuccess: getGoalByIdResponseIsSuccess
  } = useGetGoalByIdQuery({ teamId, goalId });

  useEffect(() => {
    if (getGoalByIdResponseIsSuccess && getGoalByIdResponseData?.value != null) {
      const goal = getGoalByIdResponseData.value;
      if (goal != null) {
        dispatch(
          setFormData({
            formData: {
              id: goal.id,
              ultimateGoalId: goal.ultimateGoalId,
              name: goal.name,
              category: goal.category,
              description: goal.description ?? '',
              dueDate:
                goal?.dueDate !== null && goal?.dueDate !== undefined && goal.dueDate != '0001-01-01T00:00:00' //TODO: Return something else, or make global function to check for min date
                  ? new Date(goal.dueDate).toISOString().slice(0, 10)
                  : new Date().toISOString().slice(0, 10),
              actionItems: goal?.actionItems ??
              [
                {
                  id: uuidv4(),
                  name: '',
                  description: '',
                  dueDate: new Date().toISOString().slice(0, 10)
                }
              ]
            }
          })
        );
      } else {
        dispatch(
          setFormData({
            formData: {
              id: '',
              ultimateGoalId: '',
              name: '',
              category: 14,
              description: '',
              dueDate: new Date().toISOString().slice(0, 10),
              actionItems: [
                {
                  id: uuidv4(),
                  name: '',
                  description: '',
                  dueDate: new Date().toISOString().slice(0, 10)
                }
              ]
            }
          })
        );
      }
      onGoalDataReceived();
    }
  }, [getGoalByIdResponseIsSuccess, getGoalByIdResponseData, dispatch]);

  if (getGoalByIdResponseIsLoading) return <CircleLoader />;

  if (getGoalByIdResponseIsError) return <p>An error occurred while retrieving the goal data.</p>;

  return null;
};

export default GoalDataFetcher;