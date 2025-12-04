import { rest } from 'msw';
import { TeamByUserEmail } from 'msw/data/team/teamByUserEmailResponse';
import { baseUrl } from 'msw/data/shared/sharedVariables';

const mockTeamHandlers = [
  // TeamById
  rest.get(baseUrl + '/api/users/:email/team', (req, res, ctx) => {
    return res(ctx.json(TeamByUserEmail));
  })
];

export default mockTeamHandlers;
