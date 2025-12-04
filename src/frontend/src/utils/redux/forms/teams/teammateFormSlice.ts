import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { FormState } from 'utils/redux/forms/types'; // Create the necessary types

export interface TeammateState {
  id: string,
  name: string,
  dateOfBirth: string;
}

const initialState: FormState<TeammateState> = {
  formData: {
    id: '',
    name: '',
    dateOfBirth: ''
  }
};

export const teammateFormSlice = createSlice({
  name: 'teammateForm',
  initialState,
  reducers: {
    setFormData: (
      state,
      action: PayloadAction<FormState<TeammateState>>
    ) => {
      state.formData = { ...state.formData, ...action.payload.formData };
    },
    updateFormData: (
      state,
      action: PayloadAction<{ [key: string]: string | boolean }>
    ) => {
      state.formData = { ...state.formData, ...action.payload };
    }
  }
});

export const { updateFormData, setFormData } = teammateFormSlice.actions;

export default teammateFormSlice.reducer;