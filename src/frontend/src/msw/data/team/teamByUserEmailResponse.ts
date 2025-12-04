import { TeamByUserEmailResponse } from 'library/userAggregate/queries/TeamByUserEmailResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import { team1Id, teammate1Id, teammate2Id, teammate3Id, teammate4Id } from 'msw/data/shared/sharedVariables';

export const TeamByUserEmail: ApiResponse<TeamByUserEmailResponse> = {
  error: {
    code: '',
    message: ''
  },
  isFailure: false,
  isSuccess: true,
  value: {
    id: team1Id,
    name: 'Team Gold',
    description: 'My team.',
    teammates: [
      {
        id: teammate1Id,
        name: 'Casey',
        dateOfBirth: new Date(1999, 2, 4).toISOString()
      },
      {
        id: teammate2Id,
        name: 'Shelby',
        dateOfBirth: new Date(1999, 5, 25).toISOString()
      },
      {
        id: teammate3Id,
        name: 'Marbles',
        dateOfBirth: new Date(2015, 7, 14).toISOString()
      },
      {
        id: teammate4Id,
        name: 'Turnip',
        dateOfBirth: new Date(2021, 6, 3).toISOString()
      }
    ]
  }
};