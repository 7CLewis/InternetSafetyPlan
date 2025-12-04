import React from 'react';
import ProfileDropdown from 'components/base/header/profile-dropdown/index';
import { Link } from 'react-router-dom';

const Header = () => {
  return (
    <>
      <header aria-label='Site Header' className='bg-white'>
        <div className='bg-gray-200 w-full px-4'>
          <div className='flex h-16 items-center justify-between'>
            <Link className='pl-1 text-teal-600 font-bold text-2xl' to='/'>
              Internet Safety Plan
            </Link>
            <div className='md:flex md:items-center md:gap-12'>
              <div className='flex items-center gap-4'>
                <ProfileDropdown />
              </div>
            </div>
          </div>
        </div>
      </header>
    </>
  );
};

export default Header;
