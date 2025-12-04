import { rest } from 'msw';
import { UltimateGoalById } from 'msw/data/ultimateGoal/ultimateGoalByIdResponse';
import { TeamUltimateGoalsWithGoalsAndActions } from 'msw/data/ultimateGoal/teamUltimateGoalsWithGoalsAndActionsResponse';
import { baseUrl } from 'msw/data/shared/sharedVariables';

const mockUltimateGoalHandlers = [
  // UltimateGoalById
  rest.get(baseUrl + '/api/ultimateGoals/:id', (req, res, ctx) => {
    return res(ctx.json(UltimateGoalById));
  }),

  // TeamUltimateGoalsWithGoalsAndActions
  rest.get(baseUrl + '/api/ultimateGoals/teams/:teamId?includeGoalsAndActions=true', (req, res, ctx) => {
    return res(ctx.json(TeamUltimateGoalsWithGoalsAndActions));
  })
];

export default mockUltimateGoalHandlers;