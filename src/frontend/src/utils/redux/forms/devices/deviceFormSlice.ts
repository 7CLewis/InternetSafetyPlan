import { PayloadAction, createSlice } from '@reduxjs/toolkit';
import { FormState } from 'utils/redux/forms/types'; // Create the necessary types

export interface DeviceState {
  id: string,
  teamId: string,
  name: string,
  nickname: string,
  type: number,
  teammateIds: string[],
  tags: string[]
}

const initialState: FormState<DeviceState> = {
  formData: {
    id: '',
    teamId: '',
    name: '',
    nickname: '',
    type: 1,
    teammateIds: [],
    tags: []
  }
};

export const deviceFormSlice = createSlice({
  name: 'deviceForm',
  initialState,
  reducers: {
    setFormData: (
      state,
      action: PayloadAction<FormState<DeviceState>>
    ) => {
      state.formData = { ...state.formData, ...action.payload.formData };
    },
    updateFormData: (
      state,
      action: PayloadAction<{ [key: string]: string | string[] | boolean }>
    ) => {
      state.formData = { ...state.formData, ...action.payload };
    }
  }
});

export const { updateFormData, setFormData } = deviceFormSlice.actions;

export default deviceFormSlice.reducer;