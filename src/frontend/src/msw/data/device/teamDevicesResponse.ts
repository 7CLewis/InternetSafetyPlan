import { ApiResponse } from 'utils/redux/apiResponse';
import { device1Id, device2Id, device3Id, teammate1Id, teammate2Id, teammate3Id, teammate4Id } from 'msw/data/shared/sharedVariables';
import { TeamDevicesResponse } from 'library/deviceAggregate/queries/TeamDevicesResponse';
import DeviceType from 'library/deviceAggregate/DeviceType';

export const TeamDevices: ApiResponse<TeamDevicesResponse[]> = {
  error: {
    code: '',
    message: ''
  },
  isFailure: false,
  isSuccess: true,
  value: [
    {
      id: device2Id,
      name: 'Samsung Galaxy S21 5G',
      nickname: 'Casey\'s Phone',
      deviceType: DeviceType.Phone,
      teammateIds: [
        teammate1Id,
        teammate2Id
      ],
      tags: [
        'Android',
        'Twitter',
        'YouTube',
        'CovenantEyes'
      ]
    },
    {
      id: device1Id,
      name: 'iPhone 13s',
      nickname: 'Shelby\'s Phone',
      deviceType: DeviceType.Phone,
      teammateIds: [
        teammate2Id
      ],
      tags: [
        'iOS',
        'Instagram',
        'TikTok'
      ]
    },
    {
      id: device3Id,
      name: 'Roku Smart TV',
      nickname: 'Family Room TV',
      deviceType: DeviceType.TV,
      teammateIds: [
        teammate1Id,
        teammate2Id,
        teammate3Id,
        teammate4Id
      ],
      tags: [
        'Netflix',
        'Hulu',
        'Prime Video',
        'Disney+',
        'PIN Protection'
      ]
    }
  ]
};