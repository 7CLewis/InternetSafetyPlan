import Tooltip from 'components/shared/tooltip';
import React from 'react';
import { FaHome, FaCheck, FaTrophy, FaUsers } from 'react-icons/fa';
import { MdDevices } from 'react-icons/md';
import { Link } from 'react-router-dom';

const Sidebar = () => {
  return (
    <>
      <div className='bg-gray-200 w-[70px] flex flex-col justify-between items-center border-t-[1px] border-gray-300 min-w-[70px]' >
        <div className='mt-4'>
          <Tooltip text='Home'>
            <Link to='/'>
              <FaHome className='text-gray-600 hover:text-gray-900 cursor-pointer mb-4' size={ 28 } />
            </Link>
          </Tooltip>
          <Tooltip text='Goals'>
            <Link to='/goals'>
              <FaTrophy className='text-gray-600 hover:text-gray-900 cursor-pointer mb-4' size={ 28 } />
            </Link>
          </Tooltip>
          <Tooltip text='Actions'>
            <Link to='/actions'>
              <FaCheck className='text-gray-600 hover:text-gray-900 cursor-pointer mb-4' size={ 28 } />
            </Link>
          </Tooltip>
          <Tooltip text='Devices'>
            <Link to='/devices'>
              <MdDevices className='text-gray-600 hover:text-gray-900 cursor-pointer mb-4' size={ 28 } />
            </Link>
          </Tooltip>
          <Tooltip text='Team'>
            <Link to='/team'>
              <FaUsers className='text-gray-600 hover:text-gray-900 cursor-pointer mb-4' size={ 28 } />
            </Link>
          </Tooltip>
        </div>
    </div>
    </>
  );
};

export default Sidebar;