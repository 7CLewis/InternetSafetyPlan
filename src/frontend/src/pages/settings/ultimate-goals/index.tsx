import { TeamUltimateGoalsResponse } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsResponse';
import React, { useState, useEffect } from 'react';
import { useGetTeamUltimateGoalsQuery } from 'utils/redux/api/ultimateGoalApiSlice';
import { ApiResponse } from 'utils/redux/apiResponse';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { FaPlus } from 'react-icons/fa';
import UltimateGoalEditableRow from 'pages/settings/ultimate-goals/ultimateGoalEditableRow';
import { v4 as uuidv4 } from 'uuid';
import CircleLoader from 'components/shared/loading/circleLoader';

const UltimateGoals = () => {
  const [tableData, setTableData] = useState<JSX.Element[]>([]);

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

  const handleAddClick = () => {
    const newKey = uuidv4();
    const newEntry = <UltimateGoalEditableRow key={ newKey } ultimateGoal={ new TeamUltimateGoalsResponse('','','','') } isEditMode={ true } onDelete={ () => handleChildDeleteAction(newKey) } />;
    const newTableData = [...tableData, newEntry];
    setTableData(newTableData);
  };

  const handleChildDeleteAction = (id: string) => {
    setTableData((prevState) => prevState.filter((element) => element.key !== id));
  };

  useEffect(() => {
    if (getTeamUltimateGoalsResponseIsSuccess && getTeamUltimateGoalsResponseData?.isSuccess) {
      const ultimateGoals = getTeamUltimateGoalsResponseData.value;

      if (ultimateGoals === null) {
        setTableData([]);
      } else {
        setTableData(ultimateGoals.map((ultimateGoal) => (
          <UltimateGoalEditableRow key={ ultimateGoal.id } ultimateGoal={ ultimateGoal } onDelete={ handleChildDeleteAction } />
        )));
      }
    }
  }, [getTeamUltimateGoalsResponseIsSuccess, getTeamUltimateGoalsResponseData]);

  if (getTeamUltimateGoalsResponseIsLoading) return <CircleLoader />;

  if (getTeamUltimateGoalsResponseIsError || getTeamUltimateGoalsResponseData?.isFailure) return <p>An error occurred while trying to load your team's Ultimate Goals.</p>;

  return (
    <>
      <div className='p-4 m-4 flex flex-col'>
        <div className='font-bold self-center pb-5 text-4xl'>
          <p>Ultimate Goals</p>
        </div>
        <table className='table-fixed w-full'>
          <thead>
            <tr>
              <th className='px-4 py-2 text-left'>Name</th>
              <th className='px-4 py-2 text-left'>Description</th>
              <th className='px-4 py-2 w-52'>Actions</th>
            </tr>
          </thead>
          <tbody>
            { tableData }
          </tbody>
        </table>
        { tableData.length < 5 &&
          <div className='flex flex-row align-middle mt-2'>
            <FaPlus
              className='cursor-pointer w-6 h-6 mr-3 text-gray-500'
              onClick={ handleAddClick }
            />
            <p>Add another Ultimate Goal!</p>
          </div>
        }
      </div>
    </>
  );
};

export default UltimateGoals;
