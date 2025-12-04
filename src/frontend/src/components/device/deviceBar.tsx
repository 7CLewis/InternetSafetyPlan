import { Device } from 'library/deviceAggregate/Device';
import React, { useState } from 'react';
import DeviceForm from 'components/device/deviceForm';
import DeviceIcon from 'components/device/deviceIcon';

type Props = {
  device: Device;
};

const DeviceBar = (props: Props) => {
  const { device } = props;
  const [open, setOpen] = useState(false);

  const handleClose = (isSubmit = false) => {
    if (isSubmit) {
      setOpen(false);
      return;
    }

    const result = window.confirm('Leave without saving changes to this Device?');
    if (result) {
      setOpen(false);
    }
  };

  const handleOpen = () => {
      setOpen(true);
  };

  return (
    <div className='m-2 border border-gray-300 rounded p-2 bg-gray-100'>
      <li
        key={ device.id }
        className='flex items-center justify-between'
      >
        <div className='flex items-center'>
          <DeviceIcon typeId={ device.type } size={ 32 } />
          <span className='pl-2'>{ device.name }</span>
        </div>
        <div>
          <button className='underline' onClick={ handleOpen }>View</button>
          <DeviceForm deviceId={ device.id } key={ device.id } handleClose={ handleClose } open={ open } />
        </div>
      </li>
    </div>
  );
};

export default DeviceBar;