import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { FormState } from 'utils/redux/forms/types'; // Create the necessary types

export interface ActionItemState {
  goalId: string,
  goalName: string,
  actionItemId: string,
  name: string,
  description: string,
  dueDate: string,
  isComplete: boolean
}

const initialState: FormState<ActionItemState> = {
  formData: {
    goalId: '',
    goalName: '',
    actionItemId: '',
    name: '',
    description: '',
    dueDate: '',
    isComplete: false
  }
};

export const actionItemFormSlice = createSlice({
  name: 'actionItemForm',
  initialState,
  reducers: {
    setFormData: (
      state,
      action: PayloadAction<FormState<ActionItemState>>
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

export const { updateFormData, setFormData } = actionItemFormSlice.actions;

export default actionItemFormSlice.reducer;