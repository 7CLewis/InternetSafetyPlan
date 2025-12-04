import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { RootState } from 'utils/redux/store';

interface AuthState {
  accessToken: string | null;
}

const initialState: AuthState = {
  accessToken: null
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setAccessToken: (state, action: PayloadAction<string>) => {
      state.accessToken = action.payload;
    },
    logOut: (state) => {
      state.accessToken = null;
    }
  }
});

export const { setAccessToken, logOut } = authSlice.actions;

export default authSlice.reducer;

export const selectAccessToken = (state: RootState) => state.auth.accessToken;
