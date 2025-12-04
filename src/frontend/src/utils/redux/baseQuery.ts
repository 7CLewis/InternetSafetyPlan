import { fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { RootState } from 'utils/redux/store';

const BASE_URL = 'https://localhost:7070/api';

export const baseQuery = fetchBaseQuery({
  baseUrl: BASE_URL,
  credentials: 'include',
  prepareHeaders: (headers, { getState }) => {
    headers.set('Access-Control-Allow-Origin', 'http://localhost:5173');
    const token = (getState() as RootState).auth.accessToken;
    if (token) headers.set('Authorization', `Bearer ${token}`);
    return headers;
  }
});