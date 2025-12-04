import { createApi} from '@reduxjs/toolkit/query/react';
import { CreateTeamCommand } from 'library/teamAggregate/commands/CreateTeamCommand';
import { baseQuery } from 'utils/redux/baseQuery';
import { ApiResponse } from 'utils/redux/apiResponse';
import { TeamByIdResponse } from 'library/teamAggregate/queries/TeamByIdResponse';
import { TeammateByIdResponse } from 'library/teamAggregate/queries/TeammateByIdResponse';
import { AddTeammateToTeamCommand } from 'library/teamAggregate/commands/AddTeammateToTeamCommand';
import { UpdateTeammateInformationCommand } from 'library/teamAggregate/commands/UpdateTeammateInformation';
import { DeleteTeammateCommand } from 'library/teamAggregate/commands/DeleteTeammateCommand';
import { UpdateTeamInformationCommand } from 'library/teamAggregate/commands/UpdateTeamInformationCommand';

export const teamApi = createApi({
  reducerPath: 'teamApi',
  baseQuery,
  endpoints: (builder) => ({
    getTeamById: builder.query<ApiResponse<TeamByIdResponse>, string>({
      query: (id) => `teams/${id}`
    }),
    getTeammateById: builder.query<ApiResponse<TeammateByIdResponse>, { teamId: string, teammateId: string }>({
      query: ({teamId, teammateId}) => `teams/${teamId}/teammates/${teammateId}`
    }),
    createTeam: builder.mutation<ApiResponse<string>, CreateTeamCommand>({
      query: ({ userEmail, name, description }) => ({
        url: '/teams',
        method: 'POST',
        body: {
          userEmail: userEmail,
          name: name,
          description: description
        }
      })
    }),
    updateTeamInformation: builder.mutation<ApiResponse<string>, UpdateTeamInformationCommand>({
      query: ({ teamId, name, description }) => ({
        url: '/teams',
        method: 'PUT',
        body: {
          teamId: teamId,
          name: name,
          description: description
        }
      })
    }),
    addTeammateToTeam: builder.mutation<ApiResponse<string>, AddTeammateToTeamCommand>({
      query: ({ teamId, name, dateOfBirth }) => ({
        url: '/teams/teammates',
        method: 'POST',
        body: {
          teamId: teamId,
          teammateName: name,
          teammateDateOfBirth: dateOfBirth
        }
      })
    }),
    updateTeammateInformation: builder.mutation<ApiResponse<string>, UpdateTeammateInformationCommand>({
      query: ({ teamId, teammateId, name, dateOfBirth, userId }) => ({
        url: '/teams/teammates',
        method: 'PUT',
        body: {
          teamId: teamId,
          teammateId: teammateId,
          name: name,
          dateOfBirth: dateOfBirth,
          userId: userId
        }
      })
    }),
    deleteTeammate: builder.mutation<ApiResponse<string>, DeleteTeammateCommand>({
      query: ({ teamId, teammateId }) => ({
        url: '/teams/teammates',
        method: 'DELETE',
        body: {
          teamId: teamId,
          teammateId: teammateId
        }
      })
    })
  })
});

export const { useGetTeamByIdQuery, useGetTeammateByIdQuery, useCreateTeamMutation, useUpdateTeamInformationMutation, useAddTeammateToTeamMutation, useUpdateTeammateInformationMutation, useDeleteTeammateMutation } = teamApi;