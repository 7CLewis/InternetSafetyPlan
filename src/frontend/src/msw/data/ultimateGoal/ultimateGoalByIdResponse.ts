import { ApiResponse } from 'utils/redux/apiResponse';
import { team1Id, ultimateGoal1Id } from 'msw/data/shared/sharedVariables';
import { UltimateGoalByIdResponse } from 'library/ultimateGoalAggregate/queries/UltimateGoalByIdResponse';

export const UltimateGoalById: ApiResponse<UltimateGoalByIdResponse> = {
  error: {
    code: '',
    message: ''
  },
  isFailure: false,
  isSuccess: true,
  value: {
    id: ultimateGoal1Id,
    teamId: team1Id,
    name: 'Maintain an internet-safe home',
    description: 'Enforce and improve upon internet safety practices within our home.'
  }
};