import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import { RootState } from 'utils/redux/store';

export interface UserSetupState {
  name: string;
  dateOfBirth: string | null;
}

const initialState: UserSetupState = {
  name: '',
  dateOfBirth: null
};

export const userSetupSlice = createSlice({
  name: 'counter',
  initialState,
  reducers: {
    setName: (state, action: PayloadAction<string>) => {
      state.name = action.payload;
    },
    setDateOfBirth: (state, action: PayloadAction<Date>) => {
      state.dateOfBirth = action.payload.toISOString();
    }
  }
});

export const { setName, setDateOfBirth } = userSetupSlice.actions;

export default userSetupSlice.reducer;

export const selectName = (state : RootState) => state.userSetup.name;
export const selectDateOfBirth = (state : RootState) => state.userSetup.dateOfBirth;