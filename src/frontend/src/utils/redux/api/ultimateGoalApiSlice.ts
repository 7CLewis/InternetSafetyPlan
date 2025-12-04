import { createApi} from '@reduxjs/toolkit/query/react';
import { CreateUltimateGoalCommand } from 'library/ultimateGoalAggregate/commands/CreateUltimateGoalCommand';
import { UpdateUltimateGoalInformationCommand } from 'library/ultimateGoalAggregate/commands/UpdateUltimateGoalInformationCommand';
import { TeamUltimateGoalsResponse } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsResponse';
import { UltimateGoalsWithGoalsAndActionsResponse } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsWithGoalsAndActionsResponse';
import { UltimateGoalByIdResponse } from 'library/ultimateGoalAggregate/queries/UltimateGoalByIdResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import { baseQuery } from 'utils/redux/baseQuery';

export const ultimateGoalApi = createApi({
  reducerPath: 'ultimateGoalApi',
  baseQuery,
  endpoints: (builder) => ({
    getUltimateGoalById: builder.query<ApiResponse<UltimateGoalByIdResponse>, string>({
      query: (id) => `ultimateGoals/${id}`
    }),
    getTeamUltimateGoals: builder.query<ApiResponse<TeamUltimateGoalsResponse[]>, string>({
      query: (teamId) => `ultimateGoals/teams/${teamId}`
    }),
    getTeamUltimateGoalsWithGoalsAndActions: builder.query<ApiResponse<UltimateGoalsWithGoalsAndActionsResponse[]>, string>({
      query: (teamId) => `ultimateGoals/teams/${teamId}?includeGoalsAndActions=true`
    }),
    createUltimateGoal: builder.mutation<ApiResponse<string>, CreateUltimateGoalCommand>({
      query: ({ teamId, name, description }) => ({
        url: '/ultimateGoals',
        method: 'POST',
        body: {
          teamId: teamId,
          name: name,
          description: description
        }
      })
    }),
    updateUltimateGoal: builder.mutation<ApiResponse<string>, UpdateUltimateGoalInformationCommand>({
      query: ({ ultimateGoalId, name, description }) => ({
        url: '/ultimateGoals',
        method: 'PUT',
        body: {
          ultimateGoalId: ultimateGoalId,
          name: name,
          description: description
        }
      })
    }),
    deleteUltimateGoal: builder.mutation<ApiResponse<string>, string>({
      query: (id) => ({
        url: `/ultimateGoals/${id}`,
        method: 'DELETE'
      })
    })
  })
});

export const {
  useGetUltimateGoalByIdQuery,
  useGetTeamUltimateGoalsQuery,
  useGetTeamUltimateGoalsWithGoalsAndActionsQuery,
  useCreateUltimateGoalMutation,
  useUpdateUltimateGoalMutation,
  useDeleteUltimateGoalMutation
} = ultimateGoalApi;