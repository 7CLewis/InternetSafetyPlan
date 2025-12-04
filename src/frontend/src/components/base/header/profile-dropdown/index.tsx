import React, { useState, useRef, useEffect } from 'react';
import { FaUserCircle } from 'react-icons/fa';
import { useAuth } from 'react-oidc-context';

const ProfileDropdown = () => {
  const auth = useAuth();

  const [isOpen, setIsOpen] = useState(false);
  const dropdownRef = useRef<HTMLDivElement>(null);

  const handleToggle = () => {
    setIsOpen(!isOpen);
  };

  const handleClickOutside = (event: MouseEvent) => {
    if (dropdownRef.current && !dropdownRef.current.contains(event.target as Node)) {
      setIsOpen(false);
    }
  };

  useEffect(() => {
    document.addEventListener('click', handleClickOutside, true);
    return () => {
      document.removeEventListener('click', handleClickOutside, true);
    };
  }, []);

  return (
    <>
      <div ref={ dropdownRef } className='dropdown-container'>
        <FaUserCircle className='cursor-pointer w-8 h-8' onClick={ handleToggle } />
        { isOpen && (
          <div className='absolute top-12 right-4 bg-white border border-gray-300 p-2 w-40'>
            <ul className='list-none p-0 m-0'>
              <li className='p-2 cursor-pointer'>My Profile</li>
              <li className='p-2 cursor-pointer'>My Settings</li>
              <li className='p-2 cursor-pointer'>
                <button
                  className='text-red-500'
                  onClick={ () =>
                    void auth.signoutRedirect({
                      post_logout_redirect_uri: location.origin
                    })
                  }
                >
                  Log out
                </button>
              </li>
            </ul>
          </div>
        ) }
      </div>
    </>
  );
};

export default ProfileDropdown;