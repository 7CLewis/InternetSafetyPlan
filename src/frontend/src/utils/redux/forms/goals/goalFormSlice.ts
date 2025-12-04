import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { CreateEditGoalAndActionItems_ActionItem } from 'library/goalAggregate/CreateEditGoalAndActionItem';
import { FormState } from 'utils/redux/forms/types';
import { v4 as uuidv4 } from 'uuid';

export interface GoalState {
  id: string;
  ultimateGoalId: string;
  name: string,
  description: string,
  category: number,
  dueDate: string,
  actionItems: CreateEditGoalAndActionItems_ActionItem[];
}

const initialState: FormState<GoalState> = {
  formData: {
    id: '',
    ultimateGoalId: '',
    name: '',
    category: 14,
    description: '',
    dueDate: '',
    actionItems: [ {
      id: uuidv4(),
      name: '',
      description: '',
      dueDate: ''
    }]
  }
};

export const goalFormSlice = createSlice({
  name: 'goalForm',
  initialState,
  reducers: {
    setFormData: (
      state,
      action: PayloadAction<FormState<GoalState>>
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

export const { updateFormData, setFormData } = goalFormSlice.actions;

export default goalFormSlice.reducer;