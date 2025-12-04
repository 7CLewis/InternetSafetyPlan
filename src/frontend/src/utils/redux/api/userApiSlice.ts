import { createApi} from '@reduxjs/toolkit/query/react';
import { CreateUserCommand } from 'library/userAggregate/commands/CreateUserCommand';
import { TeamByUserEmailResponse } from 'library/userAggregate/queries/TeamByUserEmailResponse';
import { UserByEmailResponse } from 'library/userAggregate/queries/UserByEmailResponse';
import { UserByIdResponse } from 'library/userAggregate/queries/UserByIdResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import { baseQuery } from 'utils/redux/baseQuery';

export const userApi = createApi({
  reducerPath: 'userApi',
  baseQuery,
  endpoints: (builder) => ({
    getUserById: builder.query<ApiResponse<UserByIdResponse>, string>({
      query: (id) => `users/${encodeURIComponent(id)}`
    }),
    getUserByEmail: builder.query<ApiResponse<UserByEmailResponse>, string>({
      query: (email) => `users/${encodeURIComponent(email)}`
    }),
    getTeamByUserEmail: builder.query<ApiResponse<TeamByUserEmailResponse>, string>({
      query: (email) => `users/${encodeURIComponent(email)}/team`
    }),
    createUser: builder.mutation<ApiResponse<string>, CreateUserCommand>({
      query: ({ email }) => ({
        url: '/users',
        method: 'POST',
        body: {
          email: email
        }
      })
    })
  })
});

export const { useGetUserByEmailQuery, useGetUserByIdQuery, useGetTeamByUserEmailQuery, useCreateUserMutation } = userApi;