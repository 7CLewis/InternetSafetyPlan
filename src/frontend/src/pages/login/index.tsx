import React from 'react';
import { useAuth } from 'react-oidc-context';
import { Navigate, useLocation } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { selectAccessToken, setAccessToken } from 'utils/auth/authSlice';
import { useGetTeamByUserEmailQuery, useGetUserByEmailQuery } from 'utils/redux/api/userApiSlice';
import { ApiResponse } from 'utils/redux/apiResponse';
import { InitialSetupOption, InitialSetupState } from 'pages/initial-setup/index';
import { User } from 'library/userAggregate/User';
import { UserByEmailResponse } from 'library/userAggregate/queries/UserByEmailResponse';
import { TeamByUserEmailResponse } from 'library/userAggregate/queries/TeamByUserEmailResponse';
import { setTeamIdInLocalStorage, setUserEmailInLocalStorage } from 'utils/local-storage/localStorage';
import CircleLoader from 'components/shared/loading/circleLoader';

interface LocationState {
  from: Location;
}

// See decision tree here: [TODO: AzDo link to login/setup decision tree]
const Login = () => {
  const dispatch = useDispatch();
  const location = useLocation();
  const { isAuthenticated, user: idpUser } = useAuth();
  const token: string | null = useSelector(selectAccessToken);
  const locationState: LocationState | undefined = location.state as LocationState | undefined;
  const from: string = locationState ? locationState.from.pathname : '/';

  if (isAuthenticated && idpUser) {
    if (idpUser?.access_token && !token) {
      dispatch(setAccessToken(idpUser?.access_token));
    }

    const {
      data: getUserResponseData,
      isLoading: getUserResponseIsLoading,
      isError: getUserResponseIsError,
      isSuccess: getUserResponseIsSuccess
    }: {
      data?: ApiResponse<UserByEmailResponse>,
      isLoading: boolean,
      isError: boolean,
      isSuccess: boolean,
    } = useGetUserByEmailQuery(idpUser?.profile.email ?? '', { skip: !idpUser?.profile.email || !token });

    const {
      data: getTeamResponseData,
      isLoading: getTeamResponseIsLoading,
      isError: getTeamResponseIsError,
      isSuccess: getTeamResponseIsSuccess
    }: {
      data?: ApiResponse<TeamByUserEmailResponse>,
      isLoading: boolean,
      isError: boolean,
      isSuccess: boolean,
    } = useGetTeamByUserEmailQuery(idpUser?.profile.email ?? '', { skip: !getUserResponseData?.value });

    if (getUserResponseIsLoading || getTeamResponseIsLoading) return (<CircleLoader />);

    if (
      getUserResponseIsError ||
      getUserResponseData?.isFailure ||
      getTeamResponseIsError ||
      getTeamResponseData?.isFailure
    ) {
      return (
        <p>
          An error occurred while trying to contact the server.
        </p>
      );
    }

    if (getUserResponseIsSuccess && getUserResponseData?.isSuccess) {
      if (!getUserResponseData.value) return (<Navigate to='/initial-setup' state={ { type: InitialSetupOption.User } as InitialSetupState } />);

      const userByEmailResponse = getUserResponseData.value;
      if (getTeamResponseIsSuccess && getTeamResponseData?.isSuccess) {
        if (!getTeamResponseData.value) {
          const user = new User(userByEmailResponse.id, userByEmailResponse.email);
          setUserEmailInLocalStorage(userByEmailResponse.email);
          return (<Navigate to='/initial-setup' state={ { type: InitialSetupOption.Team, user: user } as InitialSetupState } />);
        }
        setTeamIdInLocalStorage(getTeamResponseData.value.id);
        setUserEmailInLocalStorage(userByEmailResponse.email);
        return (<Navigate to={ from } replace={ true } />);
      }
    }
  }
  return (<></>);
};

export default Login;
