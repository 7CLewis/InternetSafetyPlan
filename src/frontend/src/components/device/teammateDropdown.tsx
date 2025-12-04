import { Teammate } from 'library/teamAggregate/Teammate';
import React, { useEffect, useState } from 'react';
import { FaPlus } from 'react-icons/fa';

interface Props {
  teammates: Teammate[]
  associatedTeammateIds: string[];
  onTeammateChange: (teammateIds: string[]) => void;
}

const TeammateDropdown = (props: Props) => {
  const { teammates, associatedTeammateIds, onTeammateChange } = props;
  const [addedTeammates, setAddedTeammates] = useState<Teammate[]>(teammates.filter(teammate => associatedTeammateIds.includes(teammate.id)));
  const [availableTeammates, setAvailableTeamates] = useState<Teammate[]>(teammates.filter(teammate => !associatedTeammateIds.includes(teammate.id)));
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
    setAddedTeammates(teammates.filter(teammate => associatedTeammateIds.includes(teammate.id)));
  }, [associatedTeammateIds]);

  useEffect(() => {
    onTeammateChange(addedTeammates.map((teammate) => teammate.id));
  }, [availableTeammates]);

  const toggleDropdown = () => {
    setIsOpen(!isOpen);
  };

  const handleOptionSelect = (teammateId: string) => {
    const updatedAvailableTeammates = availableTeammates.filter((teammate) => teammate.id !== teammateId);
    setAvailableTeamates(updatedAvailableTeammates);

    setAddedTeammates([...addedTeammates, teammates.filter(teammate => teammate.id == teammateId)[0]]);

    setIsOpen(false);
  };

  const [hoveredTeammate, setHoveredTeammate] = useState<string | null>(null);

  const handleMouseEnter = (teammateId: string) => {
    setHoveredTeammate(teammateId);
  };

  const handleMouseLeave = () => {
    setHoveredTeammate(null);
  };

  const handleTeammateRemove = (teammateIdToRemove: string) => {
    const updatedAddedTeammates = addedTeammates.filter((teammate) => teammate.id !== teammateIdToRemove);
    setAddedTeammates(updatedAddedTeammates);

    setAvailableTeamates([...availableTeammates, teammates.filter(teammate => teammate.id === teammateIdToRemove)[0]]);
  };

  return (
    <div className='relative flex pl-4'>
      <div className='flex items-center'>
        { addedTeammates.map((teammate) => (
          <div
            className='flex items-center relative'
            onMouseEnter={ () => handleMouseEnter(teammate.id) }
            onMouseLeave={ handleMouseLeave }
            key={ teammate.id }
          >
            <img
              src={ `src/assets/images/${teammate.id}.jpg` }
              alt={ `${teammate.name}'s profile picture` }
              className='w-14 h-14 rounded-full mr-2 cursor-pointer'
            />
            { hoveredTeammate === teammate.id && (
              <button
                className='absolute top-0 right-0 px-2 py-1 bg-red-500 rounded-full text-white text-xs cursor-pointer'
                onClick={ () => handleTeammateRemove(teammate.id) }
              >
                X
              </button>
            ) }
          </div>
        )) }
        <div className='flex items-center relative'>
          <FaPlus className='cursor-pointer' onClick={ toggleDropdown } size={ 32 } />
          { isOpen && (
            <div className='absolute mt-2 left-0 w-56 rounded-md shadow-lg bg-white ring-1 ring-black ring-opacity-5 z-10'>
              <div className='py-1'>
                { availableTeammates.map((teammate) => (
                  <button
                    key={ teammate.id }
                    onClick={ () => handleOptionSelect(teammate.id) }
                    className='w-full flex items-center px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:text-gray-900'
                  >
                    <img
                      src={ `src/assets/images/${teammate.id}.jpg` }
                      alt={ `${teammate.name}'s profile picture` }
                      className='w-6 h-6 rounded-full'
                    />
                    <span className='ml-2'>{ teammate.name }</span>
                  </button>
                )) }
              </div>
            </div>
          ) }
        </div>
      </div>
    </div>
  );
};

export default TeammateDropdown;