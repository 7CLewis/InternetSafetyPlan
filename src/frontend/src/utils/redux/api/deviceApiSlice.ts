import { createApi} from '@reduxjs/toolkit/query/react';
import { CreateDeviceCommand } from 'library/deviceAggregate/commands/CreateDeviceCommand';
import { EditDeviceCommand } from 'library/deviceAggregate/commands/EditDeviceCommand';
import { TeamDevicesResponse } from 'library/deviceAggregate/queries/TeamDevicesResponse';
import { DeleteGoalCommand } from 'library/goalAggregate/commands/DeleteGoalCommand';
import { ApiResponse } from 'utils/redux/apiResponse';
import { baseQuery } from 'utils/redux/baseQuery';

export const deviceApi = createApi({
  reducerPath: 'deviceApi',
  baseQuery,
  endpoints: (builder) => ({
    getTeamDevices: builder.query<ApiResponse<TeamDevicesResponse[]>, string>({
      query: (teamId) => `devices/teams/${teamId}`
    }),
    getDeviceById: builder.query<ApiResponse<TeamDevicesResponse>, string>({
      query: (deviceId) => `devices/${deviceId}`
    }),
    createDevice: builder.mutation<ApiResponse<string>, CreateDeviceCommand>({
      query: ({ teamId, name, nickname, type, teammateIds, tags }) => ({
        url: '/devices',
        method: 'POST',
        body: {
          teamId: teamId,
          name: name,
          nickname: nickname,
          deviceType: type,
          teammateIds: teammateIds,
          tags: tags
        }
      })
    }),
    editDevice: builder.mutation<ApiResponse<string>, EditDeviceCommand>({
      query: ({ id, name, nickname, type, teammateIds, tags }) => ({
        url: '/devices',
        method: 'PUT',
        body: {
          deviceId: id,
          name: name,
          nickname: nickname,
          deviceType: type,
          teammateIds: teammateIds,
          tags: tags
        }
      })
    }),
    deleteDevice: builder.mutation<ApiResponse<string>, DeleteGoalCommand>({
      query: ({id}) => ({
        url: `/devices/${ id }`,
        method: 'DELETE',
        body: {
          id: id
        }
      })
    })
  })
});

export const {
  useGetTeamDevicesQuery,
  useGetDeviceByIdQuery,
  useCreateDeviceMutation,
  useEditDeviceMutation,
  useDeleteDeviceMutation
} = deviceApi;