import { createSlice } from '@reduxjs/toolkit';
import type { PayloadAction } from '@reduxjs/toolkit';
import { RootState } from 'utils/redux/store';

export interface TeamSetupState {
  name: string;
  description: string;
}

const initialState: TeamSetupState = {
  name: '',
  description: ''
};

export const teamSetupSlice = createSlice({
  name: 'counter',
  initialState,
  reducers: {
    setTeamName: (state, action: PayloadAction<string>) => {
      state.name = action.payload;
    },
    setTeamDescription: (state, action: PayloadAction<string>) => {
      state.description = action.payload;
    }
  }
});

export const { setTeamName, setTeamDescription } = teamSetupSlice.actions;

export default teamSetupSlice.reducer;

export const selectTeamName = (state : RootState) => state.teamSetup.name;
export const selectTeamDescription = (state : RootState) => state.teamSetup.description;
