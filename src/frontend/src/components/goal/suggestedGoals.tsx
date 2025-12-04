import React, { useState } from 'react';
import SuggestedGoalBar from 'components/goal/suggestedGoalBar';
import { useGetSuggestedGoalsQuery } from 'utils/redux/api/goalApiSlice';
import { SuggestedGoalsResponse } from 'library/goalAggregate/queries/SuggestedGoalsResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import CircleLoader from 'components/shared/loading/circleLoader';
import GoalCategory from 'library/goalAggregate/GoalCategory';

type Props = {
  onGoalSelect: (newGoalId: string) => void;
}

const SuggestedGoals = (props: Props) => {
  const { onGoalSelect } = props;

  const categoryIds = Object.keys(GoalCategory);

  const [selectedCategory, setSelectedCategory] = useState<number | null>(null);

  const {
    data: getSuggestedGoalResponseData,
    isLoading: getSuggestedGoalResponseIsLoading,
    isError: getSuggestedGoalResponseIsError,
    isSuccess: getSuggestedGoalResponseIsSuccess
  }: {
    data?: ApiResponse<SuggestedGoalsResponse[]>,
    isLoading: boolean,
    isError: boolean,
    isSuccess: boolean,
  } = useGetSuggestedGoalsQuery();

  if (getSuggestedGoalResponseIsLoading) return <CircleLoader />;

  if (getSuggestedGoalResponseIsError) return <p>An error occurred while loading your goals</p>;


  if (getSuggestedGoalResponseIsSuccess) {
    if (getSuggestedGoalResponseData?.value != null) {
      const filteredGoals = selectedCategory
        ? getSuggestedGoalResponseData.value.filter((goal) => goal.category === selectedCategory)
        : getSuggestedGoalResponseData.value;

      if (filteredGoals.length == 0) {
        return (
          <div className='pl-4'>
            <h2 className='text-2xl font-bold mb-2'>Suggested Goals</h2>
            <select
              className='block w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-indigo-300'
              onChange={ (e) => setSelectedCategory(parseInt(e.target.value, 10)) }
            >
              { categoryIds.map((id) => (
                <option key={ id } value={ id }>
                  { GoalCategory[parseInt(id, 10)] }
                </option>
              )) }
            </select>
            { selectedCategory && (
              <ul className='mt-4'>
                ( No Goals found )
              </ul>
            ) }
          </div>
        );
      }

      return (
        <div className='pl-4'>
          <h2 className='text-2xl font-bold mb-2'>Suggested Goals</h2>
          <select
            className='block w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-indigo-300'
            onChange={ (e) => setSelectedCategory(parseInt(e.target.value, 10)) }
          >
            { categoryIds.map((id) => (
              <option key={ id } value={ id }>
                { GoalCategory[parseInt(id, 10)] }
              </option>
            )) }
          </select>
          { selectedCategory && (
            <ul className='mt-4'>
              { filteredGoals.map((goal) => (
                <SuggestedGoalBar onGoalSelect={ onGoalSelect } goal={ goal } key={ goal.id } />
              )) }
            </ul>
          ) }
        </div>
      );
    }
  }
};

export default SuggestedGoals;