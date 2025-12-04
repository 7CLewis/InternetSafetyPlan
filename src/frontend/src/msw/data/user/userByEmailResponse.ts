import { UserByEmailResponse } from 'library/userAggregate/queries/UserByEmailResponse';
import { ApiResponse } from 'utils/redux/apiResponse';
import { user1Email, user1Id } from 'msw/data/shared/sharedVariables';

export const UserByEmail: ApiResponse<UserByEmailResponse> = {
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