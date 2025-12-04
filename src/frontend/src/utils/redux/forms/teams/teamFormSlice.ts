import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { FormState } from 'utils/redux/forms/types'; // Create the necessary types

export interface TeamState {
  id: string,
  name: string,
  description: string;
}

const initialState: FormState<TeamState> = {
  formData: {
    id: '',
    name: '',
    description: ''
  }
};

export const teamFormSlice = createSlice({
  name: 'teamForm',
  initialState,
  reducers: {
    setFormData: (
      state,
      action: PayloadAction<FormState<TeamState>>
    ) => {
      state.formData = { ...state.formData, ...action.payload.formData };
    },
    updateFormData: (
      state,
      action: PayloadAction<{ [key: string]: string }>
    ) => {
      state.formData = { ...state.formData, ...action.payload };
    }
  }
});

export const { updateFormData, setFormData } = teamFormSlice.actions;

export default teamFormSlice.reducer;