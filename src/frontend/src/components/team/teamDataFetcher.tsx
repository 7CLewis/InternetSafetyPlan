import CircleLoader from 'components/shared/loading/circleLoader';
import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useGetTeamByIdQuery } from 'utils/redux/api/teamApiSlice';
import { setFormData } from 'utils/redux/forms/teams/teamFormSlice';

type Props = {
  teamId: string;
  onTeamDataReceived: () => void;
}

const TeamDataFetcher = (props: Props) => {
  const { teamId, onTeamDataReceived } = props;
  const dispatch = useDispatch();

  const {
    data: getTeamByIdResponseData,
    isLoading: getTeamByIdResponseIsLoading,
    isError: getTeamByIdResponseIsError,
    isSuccess: getTeamByIdResponseIsSuccess
  } = useGetTeamByIdQuery(teamId);

  useEffect(() => {
    if (getTeamByIdResponseIsSuccess && getTeamByIdResponseData?.value != null) {
      const team = getTeamByIdResponseData.value;
      dispatch(
        setFormData({
          formData: {
            id: team?.id ?? '',
            name: team?.name ?? '',
            description: team?.description ?? ''
          }
        })
      );
    } else {
      dispatch(
        setFormData({
          formData: {
            id: '',
            name: '',
            description: ''
          }
        })
      );
    }
    onTeamDataReceived();
  }, [getTeamByIdResponseIsSuccess, getTeamByIdResponseData, dispatch]);

  if (getTeamByIdResponseIsLoading) return <CircleLoader />;

  if (getTeamByIdResponseIsError) return <p>An error occurred while retrieving the team data.</p>;

  return null;
};

export default TeamDataFetcher;