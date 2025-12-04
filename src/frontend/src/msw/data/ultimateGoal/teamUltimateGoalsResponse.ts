import { ApiResponse } from 'utils/redux/apiResponse';
import { team1Id, ultimateGoal1Id } from 'msw/data/shared/sharedVariables';
import { TeamUltimateGoalsResponse } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsResponse';

export const TeamUltimateGoals: ApiResponse<TeamUltimateGoalsResponse[]> = {
  error: {
    code: '',
    message: ''
  },
  isFailure: false,
  isSuccess: true,
  value: [{
    id: ultimateGoal1Id,
    teamId: team1Id,
    name: 'Maintain an internet-safe home',
    description: 'Enforce and improve upon internet safety practices within our home.'
  }]
};