import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { setFormData, updateFormData } from 'utils/redux/forms/goals/goalFormSlice';
import { RootState } from 'utils/redux/store';
import { v4 as uuidv4 } from 'uuid';
import { Button } from '@material-tailwind/react';
import ActionItemForm from 'components/actionItem/actionItemForm';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import GoalDataFetcher from 'components/goal/GoalDataFetcher';
import { useCreateGoalAndActionItemsMutation, useEditGoalAndActionItemsMutation, useDeleteGoalAndActionItemsMutation } from 'utils/redux/api/goalApiSlice';
import { useGetTeamUltimateGoalsQuery } from 'utils/redux/api/ultimateGoalApiSlice';
import { TeamUltimateGoalsResponse } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import CircleLoader from 'components/shared/loading/circleLoader';
import GoalCategory from 'library/goalAggregate/GoalCategory';
import { CreateEditGoalAndActionItems_ActionItem } from 'library/goalAggregate/CreateEditGoalAndActionItem';

type Props = {
  goalId: string;
};

const GoalForm = (props: Props) => {
  const { goalId } = props;
  const [isNewGoal, setIsNewGoal] = useState<boolean>(false);
  const [goalDataReceived, setGoalDataReceived] = useState<boolean>(false);

  useEffect(() => setIsNewGoal(goalId == 'new'), []);

  const dispatch = useDispatch();
  const formData = useSelector((state: RootState) => state.goalForm.formData);
  const [ultimateGoalDropdown, setUltimateGoalDropdown] = useState<JSX.Element>(<></>);
  const categoryIds = Object.keys(GoalCategory);

  const teamId = getTeamIdFromLocalStorage();

  const {
    data: getTeamUltimateGoalsResponseData,
    isLoading: getTeamUltimateGoalsResponseIsLoading,
    isError: getTeamUltimateGoalsResponseIsError,
    isSuccess: getTeamUltimateGoalsResponseIsSuccess
  }: {
    data?: ApiResponse<TeamUltimateGoalsResponse[]>;
    isLoading: boolean;
    isError: boolean;
    isSuccess: boolean;
  } = useGetTeamUltimateGoalsQuery(teamId);

  useEffect(() => {
    if (getTeamUltimateGoalsResponseIsSuccess && getTeamUltimateGoalsResponseData?.isSuccess) {
      const ultimateGoals = getTeamUltimateGoalsResponseData.value;

      if (ultimateGoals === null) {
        setUltimateGoalDropdown(<p>No Ultimate Goals</p>);
      } else {
        setUltimateGoalDropdown(
          <select
            className='block w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-indigo-300'
            name='ultimateGoalId'
            onChange={ handleChange }
          >
            { ultimateGoals.map((ultimateGoal) => (
              <option key={ ultimateGoal.id } value={ ultimateGoal.id }>
                { ultimateGoal.name }
              </option>
            )) }
          </select>
        );
      }
    }
  }, [getTeamUltimateGoalsResponseIsSuccess, getTeamUltimateGoalsResponseData]);

  const [createGoalAndActionItems] = useCreateGoalAndActionItemsMutation();
  const [editGoalAndActionItems] = useEditGoalAndActionItemsMutation();
  const [deleteGoalAndActionItems] = useDeleteGoalAndActionItemsMutation();

  const updateActionItem = (updatedActionItem: CreateEditGoalAndActionItems_ActionItem) => {
    const updatedActionItems = formData.actionItems?.map((item) =>
      item.id === updatedActionItem.id ? updatedActionItem : item
    ) ?? null;
    dispatch(setFormData({
      formData: {
        id: goalId,
        ultimateGoalId: formData.ultimateGoalId,
        name: formData.name,
        category: formData.category,
        description: formData.description,
        dueDate: formData.dueDate,
        actionItems: updatedActionItems
      }
    }));
  };

  const handleChange = (event: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>) => {
    const { name, value, type } = event.currentTarget;
    const updatedValue = type === 'checkbox' ? (event.currentTarget as HTMLInputElement).checked : value;

    dispatch(updateFormData({ [name]: updatedValue }));
  };

  const handleActionItemAdd = () => {
    const actionItem = new CreateEditGoalAndActionItems_ActionItem('', '', '', Date.now().toString());
    const updatedActionItems = formData.actionItems?.concat(actionItem);
    dispatch(setFormData({
      formData: {
        id: goalId,
        ultimateGoalId: formData.ultimateGoalId,
        name: formData.name,
        category: formData.category,
        description: formData.description,
        dueDate: formData.dueDate,
        actionItems: updatedActionItems
      }
    }));
  };

  if (getTeamUltimateGoalsResponseIsLoading) return <CircleLoader />;

  if (getTeamUltimateGoalsResponseIsError || getTeamUltimateGoalsResponseData?.isFailure) return <p>An error occurred while trying to load your team's Ultimate Goals.</p>;

  const handleSubmit = () => {
    if (isNewGoal) {
      createGoalAndActionItems({ ultimateGoalId: formData.ultimateGoalId, name: formData.name, category: formData.category, description: formData.description, dueDate: formData.dueDate, actionItems: formData.actionItems })
        .unwrap()
        .then((response) => {
          console.log('Goal added successfully. ' + JSON.stringify(response));
          window.location.href = '/goals';
        })
        .catch((error) => {
          console.log(error);
        });
    } else {
      editGoalAndActionItems({ id: formData.id, ultimateGoalId: formData.ultimateGoalId, name: formData.name, category: formData.category, description: formData.description, dueDate: formData.dueDate, actionItems: formData.actionItems })
        .unwrap()
        .then((response) => {
          console.log('Goal updated successfully. ' + JSON.stringify(response));
          window.location.href = '/goals';
        })
        .catch((error) => {
          console.log(error);
        });
    }
  };

  const handleReset = () => {
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
          }]
        }
      })
    );
  };

  const handleDeleteRequest = () => {
    const result = window.confirm('Are you sure you want to delete this Goal?');
    if (result) {
      deleteGoalAndActionItems({ id: goalId })
        .unwrap()
        .then((response) => {
          console.log('Goal deleted successfully. ' + JSON.stringify(response));
          window.location.href = '/goals';
        })
        .catch((error) => {
          console.log(error);
        });
    }
  };

  const handleGoalDataReceived = () => {
    setGoalDataReceived(true);
  };

  if (!isNewGoal && !goalDataReceived) {
    console.log(goalDataReceived);
    return (
      <>
        <GoalDataFetcher goalId={ goalId } teamId={ getTeamIdFromLocalStorage() } onGoalDataReceived= { handleGoalDataReceived } />
        <CircleLoader />
      </>
    );
  } else {
    return (
      <>
        <div className='pr-16'>
          <div className='flex w-full flex-col justify-between items-start py-4'>
            <label className='px-4 text-sm'>Ultimate Goal</label>
            { ultimateGoalDropdown }
          </div>
          <div className='flex w-full flex-col justify-between items-start py-4'>
            <label className='px-4 text-sm'>Goal Category</label>
            <select
                className='block w-full p-2 border border-gray-300 rounded-md focus:ring focus:ring-indigo-300'
                name='category'
                onChange={ handleChange }
              >
                { categoryIds.map((id) => (
                  <option key={ id } value={ id }>
                    { GoalCategory[parseInt(id, 10)] }
                  </option>
                )) }
            </select>
          </div>
          <div className='flex w-full flex-col justify-between items-start'>
            <label className='px-4 text-sm'>Title</label>
            <input
              name='name'
              type='text'
              value={ formData.name }
              onChange={ handleChange }
              className='outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-xl border-blue-gray-200 hover:border-blue-gray-200 focus:border-2 focus:border-blue-500' />
          </div>
          <div className='w-full'>
            <div className='mb-4 flex flex-col items-start mt-4'>
              <label className='px-4 text-sm'>Description</label>
              <textarea
                name='description'
                className='mb-2 outline-0 outline rounded-[7px] w-full border mr-4 px-4 py-1 text-md border-blue-gray-200 text-black hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                rows={ 5 }
                value={ formData.description }
                onChange={ handleChange }
              />
            </div>
            <div className='flex flex-col items-start justify-start mb-4'>
              <label className='text-sm pr-9 pl-4'>Due Date</label>
              <input
                className='outline-0 outline rounded-[7px] text-black text-md border-blue-gray-200 px-4 py-1 border hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
                name='dueDate'
                type='date'
                value={ formData.dueDate ?? '' }
                onChange={ handleChange }
              />
            </div>
            <div className='flex flex-row flex-wrap items-start justify-start'>
              <div className='w-full'>
                <label className='pl-4 text-sm'>Action Items</label>
              </div>
              { formData.actionItems?.map((actionItem) => (
                <div key={ actionItem.id } className='w-1/2'>
                  <div className='m-4 bg-gray-200 p-4 mt-0 rounded-md'>
                  <ActionItemForm key={ actionItem.id } actionItem={ actionItem } updateActionItem={ updateActionItem } />
                  </div>
                </div>
              ))
              }
                <div className='flex align-middle justify-center'>
                  <div className='m-4 p-4 mt-0 rounded-md'>
                    <Button color='blue-gray' className='p-3 mb-3' onClick={ handleActionItemAdd }>+ Add Action Item</Button>
                  </div>
                </div>
            </div>
            <div className='flex flex-row justify-start p-4'>
              <div className='mr-2'>
                { !isNewGoal &&
                  <Button
                    variant='outlined'
                    color='red'
                    onClick={ () => handleDeleteRequest() }
                    className='mr-2'
                  >
                    Delete Goal
                  </Button>
                }
                <Button
                  variant='outlined'
                  color='gray'
                  onClick={ () => handleReset() }
                  className='mr-2'
                >
                  Clear Form
                </Button>
              </div>
              <div className='mr-2'>
                <Button
                  variant='outlined'
                  onClick={ handleSubmit }
                  className='mr-2'
                  color='green'
                >
                  Save
                </Button>
              </div>
            </div>
          </div>
        </div>
      </>
    );
  }


};

export default GoalForm;
