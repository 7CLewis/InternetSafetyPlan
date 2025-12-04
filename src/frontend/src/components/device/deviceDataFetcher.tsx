import CircleLoader from 'components/shared/loading/circleLoader';
import React, { useEffect } from 'react';
import { useDispatch } from 'react-redux';
import { useGetDeviceByIdQuery } from 'utils/redux/api/deviceApiSlice';
import { setFormData } from 'utils/redux/forms/devices/deviceFormSlice';

type Props = {
  deviceId: string;
  onDeviceDataReceived: () => void;
}

const DeviceDataFetcher = (props: Props) => {
  const { deviceId, onDeviceDataReceived } = props;
  const dispatch = useDispatch();

  const {
    data: getDeviceByIdResponseData,
    isLoading: getDeviceByIdResponseIsLoading,
    isError: getDeviceByIdResponseIsError,
    isSuccess: getDeviceByIdResponseIsSuccess
  } = useGetDeviceByIdQuery(deviceId);

  useEffect(() => {
    if (getDeviceByIdResponseIsSuccess && getDeviceByIdResponseData?.value != null) {
      const device = getDeviceByIdResponseData.value;
      if (device != null) {
        dispatch(
          setFormData({
            formData: {
              id: device.id,
              teamId: device.teamId,
              name: device.name,
              nickname: device.nickname ?? '',
              type: device.type,
              tags: device.tags,
              teammateIds: device.teammateIds
            }
          })
        );
      } else {
        dispatch(
          setFormData({
            formData: {
              id: '',
              teamId: '',
              name: '',
              nickname: '',
              type: 0,
              tags: [],
              teammateIds: []
            }
          })
        );
      }
      onDeviceDataReceived();
    }
  }, [getDeviceByIdResponseIsSuccess, getDeviceByIdResponseData, dispatch]);

  if (getDeviceByIdResponseIsLoading) return <CircleLoader />;

  if (getDeviceByIdResponseIsError) return <p>An error occurred while retrieving the device data.</p>;

  return null;
};

export default DeviceDataFetcher;