import { configureStore } from '@reduxjs/toolkit';
import userSetupReducer from 'pages/initial-setup/user-setup/slice';
import teamSetupReducer from 'pages/initial-setup/team-setup/slice';
import authReducer from 'utils/auth/authSlice';
import { teamApi } from 'utils/redux/api/teamApiSlice';
import { userApi } from 'utils/redux/api/userApiSlice';
import { setupListeners } from '@reduxjs/toolkit/query';
import { ultimateGoalApi } from 'utils/redux/api/ultimateGoalApiSlice';
import { goalApi } from 'utils/redux/api/goalApiSlice';
import goalFormReducer from 'utils/redux/forms/goals/goalFormSlice';
import actionItemFormReducer from 'utils/redux/forms/goals/actionItemFormSlice';
import teammateFormReducer from 'utils/redux/forms/teams/teammateFormSlice';
import teamFormReducer from 'utils/redux/forms/teams/teamFormSlice';
import deviceFormReducer from 'utils/redux/forms/devices/deviceFormSlice';
import { deviceApi } from 'utils/redux/api/deviceApiSlice';

export const store = configureStore({
  reducer: {
    // API
    [deviceApi.reducerPath]: deviceApi.reducer,
    [goalApi.reducerPath]: goalApi.reducer,
    [teamApi.reducerPath]: teamApi.reducer,
    [userApi.reducerPath]: userApi.reducer,
    [ultimateGoalApi.reducerPath]: ultimateGoalApi.reducer,
    teamSetup: teamSetupReducer,
    auth: authReducer,
    userSetup: userSetupReducer,
    // Forms
    actionItemForm: actionItemFormReducer,
    deviceForm: deviceFormReducer,
    goalForm: goalFormReducer,
    teammateForm: teammateFormReducer,
    teamForm: teamFormReducer
  },
  // Adding the api middleware enables caching, invalidation, polling,
  // and other useful features of `rtk-query`.
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware().concat(deviceApi.middleware).concat(goalApi.middleware).concat(userApi.middleware).concat(teamApi.middleware).concat(ultimateGoalApi.middleware)
});

// optional, but required for refetchOnFocus/refetchOnReconnect behaviors
// see `setupListeners` docs - takes an optional callback as the 2nd arg for customization
setupListeners(store.dispatch);

// Infer the `RootState` and `AppDispatch` types from the store itself
export type RootState = ReturnType<typeof store.getState>
// Inferred type: {posts: PostsState, comments: CommentsState, users: UsersState}
export type AppDispatch = typeof store.dispatch