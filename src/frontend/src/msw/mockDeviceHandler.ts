import { rest } from 'msw';
import { TeamDevices } from 'msw/data/device/teamDevicesResponse';
import { baseUrl } from 'msw/data/shared/sharedVariables';

const mockDeviceHandlers = [
  // TeamDevices
  rest.get(baseUrl + '/api/devices/:teamId', (req, res, ctx) => {
    return res(ctx.json(TeamDevices));
  })
];

export default mockDeviceHandlers;
