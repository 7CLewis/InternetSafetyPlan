export interface ApiResponse<T> {
  error: ApiError;
  isFailure: boolean;
  isSuccess: boolean;
  value: T | null;
}

export interface ApiError {
  code: string;
  message: string;
}