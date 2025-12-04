import { rest } from 'msw';
import { TeamGoals } from 'msw/data/goal/teamGoalsResponse';
import { baseUrl } from 'msw/data/shared/sharedVariables';
import { TeamActionItems } from 'msw/data/goal/teamActionItems';
import { GoalsById } from 'msw/data/goal/goalByIdResponse';
import { SuggestedGoals } from 'msw/data/goal/suggestedGoalsResponse';

const mockGoalHandlers = [
  // TeamGoals
  rest.get(baseUrl + '/api/goals/teams/:teamId', (req, res, ctx) => {
    return res(ctx.json(TeamGoals));
  }),
  // TeamActionItems
  rest.get(baseUrl + '/api/goals/teams/:teamId/action-items', (req, res, ctx) => {
    return res(ctx.json(TeamActionItems));
  }),
  // GoalById
  rest.get(baseUrl + '/api/goals/:goalId', (req, res, ctx) => {
    const { goalId } = req.params;
    if (GoalsById.some(goal => goal.value?.id == goalId)) return res(ctx.json(GoalsById.filter(goal => goal.value?.id == goalId)[0]));
    else return res(ctx.json(GoalsById.filter(goal => goal.value === null)[0]));
  }),
  // SuggestedGoals
  rest.get(baseUrl + '/api/suggested-goals', (req, res, ctx) => {
    return res(ctx.json(SuggestedGoals));
  })
];

export default mockGoalHandlers;
