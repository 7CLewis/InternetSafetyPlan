import { createApi} from '@reduxjs/toolkit/query/react';
import { CreateGoalAndActionItemsCommand } from 'library/goalAggregate/commands/CreateGoalAndActionItemsCommand';
import { DeleteGoalCommand } from 'library/goalAggregate/commands/DeleteGoalCommand';
import { EditActionItemCommand } from 'library/goalAggregate/commands/EditActionItemCommand';
import { EditGoalAndActionItemsCommand } from 'library/goalAggregate/commands/EditGoalAndActionItemsCommand';
import { ToggleActionItemCompletionCommand } from 'library/goalAggregate/commands/ToggleActionItemCompletionCommand';
import { SuggestedGoalsResponse } from 'library/goalAggregate/queries/SuggestedGoalsResponse';
import { TeamActionItemsResponse } from 'library/goalAggregate/queries/TeamActionItemsResponse';
import { TeamGoalsResponse } from 'library/goalAggregate/queries/TeamGoalsResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import { baseQuery } from 'utils/redux/baseQuery';

export const goalApi = createApi({
  reducerPath: 'goalApi',
  baseQuery,
  endpoints: (builder) => ({
    getTeamGoals: builder.query<ApiResponse<TeamGoalsResponse[]>, string>({
      query: (teamId) => `goals/teams/${teamId}`
    }),
    getGoalById: builder.query<ApiResponse<TeamGoalsResponse>, { teamId: string, goalId: string }>({
      query: ({ goalId }) => `goals/${goalId}`
    }),
    getTeamActionItems: builder.query<ApiResponse<TeamActionItemsResponse[]>, string>({
      query: (teamId) => `goals/teams/${teamId}/action-items`
    }),
    getActionItemById: builder.query<ApiResponse<TeamActionItemsResponse>, { teamId: string, goalId: string, actionItemId: string }>({
      query: ({ teamId, goalId, actionItemId }) => `goals/teams/${teamId}/goals/${goalId}/action-items/${actionItemId}`
    }),
    getSuggestedGoals: builder.query<ApiResponse<SuggestedGoalsResponse[]>, void >({
      query: () => 'goals/suggested'
    }),
    createGoalAndActionItems: builder.mutation<ApiResponse<string>, CreateGoalAndActionItemsCommand>({
      query: ({ ultimateGoalId, name, category, description, dueDate, actionItems }) => ({
        url: '/goals',
        method: 'POST',
        body: {
          ultimateGoalId: ultimateGoalId,
          name: name,
          category: category,
          description: description,
          dueDate: dueDate,
          actionItems: actionItems
        }
      })
    }),
    editGoalAndActionItems: builder.mutation<ApiResponse<string>, EditGoalAndActionItemsCommand>({
      query: ({ id, ultimateGoalId, name, category, description, dueDate, actionItems }) => ({
        url: '/goals',
        method: 'PUT',
        body: {
          id: id,
          ultimateGoalId: ultimateGoalId,
          name: name,
          category: category,
          description: description,
          dueDate: dueDate,
          actionItems: actionItems
        }
      })
    }),
    deleteGoalAndActionItems: builder.mutation<ApiResponse<string>, DeleteGoalCommand>({
      query: ({id}) => ({
        url: `/goals/${ id }`,
        method: 'DELETE'
      })
    }),
    toggleActionItemCompletion: builder.mutation<ApiResponse<string>, ToggleActionItemCompletionCommand>({
      query: ({goalId, actionItemId}) => ({
        url: `/goals/action-items/${ actionItemId }/toggleCompletion`,
        method: 'PUT',
        body: {
          goalId: goalId,
          actionItemId: actionItemId
        }
      })
    }),
    editActionItem: builder.mutation<ApiResponse<string>, EditActionItemCommand>({
      query: ({ goalId, actionItemId, name, description, dueDate }) => ({
        url: `/goals/action-items/${ actionItemId }`,
        method: 'PUT',
        body: {
          goalId: goalId,
          actionItemId: actionItemId,
          name: name,
          description: description,
          dueDate: dueDate != null ? new Date(dueDate).toISOString() : null
        }
      })
    })
  })
});

export const {
  useGetTeamGoalsQuery,
  useGetGoalByIdQuery,
  useGetTeamActionItemsQuery,
  useGetActionItemByIdQuery,
  useGetSuggestedGoalsQuery,
  useCreateGoalAndActionItemsMutation,
  useEditGoalAndActionItemsMutation,
  useDeleteGoalAndActionItemsMutation,
  useToggleActionItemCompletionMutation,
  useEditActionItemMutation
} = goalApi;