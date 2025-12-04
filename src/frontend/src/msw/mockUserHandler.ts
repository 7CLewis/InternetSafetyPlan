import { rest } from 'msw';
import { UserById } from 'msw/data/user/userByIdResponse';
import { UserByEmail } from 'msw/data/user/userByEmailResponse';
import { TeamByUserEmail } from 'msw/data/team/teamByUserEmailResponse';
import { baseUrl } from 'msw/data/shared/sharedVariables';
import { validate as uuidValidate } from 'uuid';

const mockUserHandlers = [
  // UserByEmail or UserById
  rest.get(baseUrl + '/api/users/:param', (req, res, ctx) => {
    const param = req.params.param as string;
    if (uuidValidate(param)) return res(ctx.json(UserById));

    return res(ctx.json(UserByEmail));
  }),

  // TeamByUserEmail
  rest.get(baseUrl + '/api/users/:email/team', (req, res, ctx) => {
    return res(ctx.json(TeamByUserEmail));
  })
];


export default mockUserHandlers;
