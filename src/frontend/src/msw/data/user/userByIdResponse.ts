import { UserByIdResponse } from 'library/userAggregate/queries/UserByIdResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import { user1Email, user1Id } from 'msw/data/shared/sharedVariables';

export const UserById: ApiResponse<UserByIdResponse> = {
  error: {
    code: '',
    message: ''
  },
  isFailure: false,
  isSuccess: true,
  value: {
    id: user1Id,
    email: user1Email
  }
};