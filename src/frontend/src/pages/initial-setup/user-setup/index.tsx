import React, { useEffect } from 'react';
import {useAuth} from 'react-oidc-context';
import { useCreateUserMutation, useGetUserByIdQuery } from 'utils/redux/api/userApiSlice';
import { Navigate } from 'react-router-dom';
import { InitialSetupOption, InitialSetupState } from 'pages/initial-setup/index';
import { User } from 'library/userAggregate/User';
import { ApiResponse } from 'utils/redux/apiResponse';
import { UserByIdResponse } from 'library/userAggregate/queries/UserByIdResponse';

const UserSetup = () => {
  const { user: idpUser } = useAuth();

  const [createUser, {
    data: createUserResponseData,
    isLoading: createUserResponseIsLoading,
    isError: createUserResponseIsError,
    isSuccess: createUserResponseIsSuccess
  }] = useCreateUserMutation();

  const {
    data: getUserResponseData,
    isLoading: getUserResponseIsLoading,
    isError: getUserResponseIsError,
    isSuccess: getUserResponseIsSuccess
  }: {
    data?: ApiResponse<UserByIdResponse>,
    isLoading: boolean,
    isError: boolean,
    isSuccess: boolean,
  } = useGetUserByIdQuery(idpUser?.profile.email ?? '', { skip: !createUserResponseData });


  useEffect(() => {
    if (idpUser?.profile.email) {
      createUser({ email: idpUser.profile.email })
      .unwrap()
      .then((response) => {
        console.log('User created successfully. ' + JSON.stringify(response));
      })
      .catch((error) => {
        console.log(error);
      });
    }
  }, [idpUser, createUser]);

  if (
    createUserResponseIsError ||
    createUserResponseData?.isFailure
  ) {
    return (
      <p>
        An error occurred while trying to add you as a new user.
      </p>
    );
  }

  if (createUserResponseIsLoading) return (<p>Setting up your user account...</p>);

  if (createUserResponseIsSuccess) {
    if (getUserResponseIsLoading) return <p>Loading newly-created user entity</p>;
    if (getUserResponseIsError) return <p>An error occurred while retrieving the newly-created user entity.</p>;
    if (getUserResponseIsSuccess && getUserResponseData?.value) {
      const userByIdResponse = getUserResponseData.value;
      const user = new User(userByIdResponse.id, userByIdResponse.email);
      return (<Navigate to='/initial-setup' state={ { type: InitialSetupOption.Team, user: user } as InitialSetupState } />);
    }
  }

  return (<></>);
};

export default UserSetup;