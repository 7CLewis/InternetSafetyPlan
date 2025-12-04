import CircleLoader from 'components/shared/loading/circleLoader';
import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useGetTeammateByIdQuery } from 'utils/redux/api/teamApiSlice';
import { setFormData } from 'utils/redux/forms/teams/teammateFormSlice';

type Props = {
  teamId: string;
  teammateId: string;
}

const TeammateDataFetcher = (props: Props) => {
  const { teamId, teammateId } = props;
  const dispatch = useDispatch();

  const {
    data: getTeammateByIdResponseData,
    isLoading: getTeammateByIdResponseIsLoading,
    isError: getTeammateByIdResponseIsError,
    isSuccess: getTeammateByIdResponseIsSuccess
  } = useGetTeammateByIdQuery({ teamId, teammateId });

  useEffect(() => {
    if (getTeammateByIdResponseIsSuccess && getTeammateByIdResponseData?.value != null) {
      const teammate = getTeammateByIdResponseData.value;
      dispatch(
        setFormData({
          formData: {
            id: teammate?.id ?? '',
            name: teammate?.name ?? '',
            dateOfBirth:
              teammate?.dateOfBirth !== null && teammate?.dateOfBirth !== undefined
                ? new Date(teammate.dateOfBirth).toISOString().slice(0, 10)
                : new Date().toISOString().slice(0, 10)
          }
        })
      );
    } else {
      dispatch(
        setFormData({
          formData: {
            id: '',
            name: '',
            dateOfBirth: new Date().toISOString().slice(0, 10)
          }
        })
      );
    }
  }, [getTeammateByIdResponseIsSuccess, getTeammateByIdResponseData, dispatch]);

  if (getTeammateByIdResponseIsLoading) return <CircleLoader />;

  if (getTeammateByIdResponseIsError) return <p>An error occurred while retrieving the teammate data.</p>;

  return null;
};

export default TeammateDataFetcher;