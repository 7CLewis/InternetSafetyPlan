import { Device } from 'library/deviceAggregate/Device';
import React, { useState } from 'react';
import DeviceIcon from 'components/device/deviceIcon';
import DeviceForm from 'components/device/deviceForm';
import { useDeleteDeviceMutation } from 'utils/redux/api/deviceApiSlice';
import { FaTrash } from 'react-icons/fa';

type Props = {
  device: Device;
  onDelete: () => void;
  onDeviceEdit: (device: Device) => void;
}

const DeviceCardItem = (props: Props) => {
  const { device, onDelete, onDeviceEdit } = props;
  const [open, setOpen] = useState(false);
  const [isHovered, setIsHovered] = useState(false);
  const [deleteDevice] = useDeleteDeviceMutation();

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

  const handleDeviceDelete = (deviceId: string) => {
    const result = window.confirm('Are you sure you want to delete this device?');

    if (result) {
      deleteDevice({ id: deviceId })
        .unwrap()
        .then((response) => {
          console.log('Device deleted successfully. ' + JSON.stringify(response));

          onDelete();
          return response;
        })
        .catch((error) => {
          console.log(error);
        });
    }
  };

  return (
    <>
      <div className='flex flex-row justify-center'
        onMouseEnter={ () => setIsHovered(true) }
        onMouseLeave={ () => setIsHovered(false) }
      >
        <div className='bg-white rounded-md shadow-md px-4 py-2 w-[1200px] m-4 mb-0'>
          <div className='flex flex-row w-full'>
            <div className='flex'>
              <div className='flex flex-col align-middle justify-center pr-4'>
                <DeviceIcon typeId={ device.type } size={ 64 } />
              </div>
            </div>
            <div className='flex flex-col w-full'>
              <div className='flex flex-row w-full'>
                <div className='flex flex-col w-full'>
                  <div className='flex flex-row w-full justify-between'>
                  <h2 className='text-xl font-bold'><span className='hover:text-blue-700 hover:cursor-pointer' onClick={ handleOpen }>{ device.name }</span></h2>
                  <div className='self-end p-4'>
                    <FaTrash
                      className={ `${ !isHovered ? 'opacity-0' : '' } text-red-500 cursor-pointer` }
                      data-tip='Delete Device'
                      onClick={ () => handleDeviceDelete(device.id) }
                    />
                  </div>
                  </div>
                  { open && <DeviceForm deviceId={ device.id } key={ device.id } handleClose={ handleClose } open={ open } onDeviceEdit={ onDeviceEdit } /> }
                  <p className='text-gray-500 mt-0'>{ device.nickname }</p>
                </div>
              </div>
              <div className='flex flex-row'>
                <div className='py-2 flex flex-row'>
                  <div className='pr-3 flex flex-row'>
                    { device.teammateIds.map((teammateId) => (
                          <img
                            key={ teammateId }
                            className='w-10 h-10 rounded-full mr-2'
                            src={ `src/assets/images/${ teammateId }.jpg` }
                            alt='profile-pic'
                          />
                        ))
                    }
                  </div>
                  <div className='pl-3 pr-3 flex flex-row'>
                    { device.tags.map((tag, index) => (
                        <div className='flex flex-col justify-center' key={ tag }>
                          <span
                            key={ index }
                            className='bg-blue-200 text-gray-700 rounded-sm px-2 text-center py-1 text-xs font-semibold mr-2'
                          >
                            { tag }
                          </span>
                        </div>
                      ))
                    }
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default DeviceCardItem;