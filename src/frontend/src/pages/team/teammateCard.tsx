import TeammateForm from 'components/team/teammateForm';
import calculateAge from 'library/shared/calculateAge';
import { Teammate } from 'library/teamAggregate/Teammate';
import React, { useState } from 'react';
import { FaTrash } from 'react-icons/fa';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { useDeleteTeammateMutation } from 'utils/redux/api/teamApiSlice';

type Props = {
  teammate: Teammate;
  deviceCount: number;
  goalCount: number;
  onDelete: () => void;
  onTeammateEdit: (teammate: Teammate) => void;
}

const TeammateCard = (props: Props) => {
  const { teammate, deviceCount, goalCount, onDelete, onTeammateEdit } = props;
  const [deleteTeammateMutation] = useDeleteTeammateMutation();

  const age = calculateAge(teammate.dateOfBirth);
  const [open, setOpen] = React.useState(false);
  const [isHovered, setIsHovered] = useState(false);

  const handleClose = (isSubmit = false) => {
    if (isSubmit) {
      setOpen(false);
      return;
    }

    const result = window.confirm('Leave without saving changes to this Teammate?');
    if (result) {
      setOpen(false);
    }
  };

  const handleOpen = () => {
      setOpen(true);
  };

  const deleteTeammate = (teammateId: string) => {
    const result = window.confirm('Are you sure you want to delete this teammate?');

    if (result) {
      deleteTeammateMutation({ teamId: getTeamIdFromLocalStorage(), teammateId: teammateId })
        .unwrap()
        .then((response) => {
          console.log('Teammate removed successfully. ' + JSON.stringify(response));

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
      <div
        className='flex flex-col pb-4 ml-4 mr-4'
        onMouseEnter={ () => setIsHovered(true) }
        onMouseLeave={ () => setIsHovered(false) }
      >
        <div className='flex w-72 flex-col rounded-xl bg-white text-gray-700 shadow-md'>
          <div className='self-end p-4'>
            <FaTrash
              className={ `${ !isHovered ? 'opacity-0' : '' } text-red-500 cursor-pointer` }
              data-tip='Delete Teammate'
              onClick={ () => {
                deleteTeammate(teammate.id);
              } }
            />
          </div>
          <div className='mx-4 mt-4 flex justify-center overflow-hidden'>
            <img
              className='w-40 h-40 rounded-full'
              src={ `src/assets/images/${ teammate.id }.jpg` }
              alt='profile-pic'
            />
          </div>
          <div className='px-6 pt-3 pb-6 flex flex-col justify-center'>
            <h5 className='mb-0 block font-sans text-xl text-center font-semibold leading-snug tracking-normal text-blue-gray-900 antialiased'>
              { teammate.name }
            </h5>
            <p className='block font-sans text-base text-center font-light leading-relaxed text-inherit antialiased'>
            Age: { age }
            </p>
          </div>
          <div className='p-6 pt-0 flex-col justify-center'>
            <p>{ deviceCount } Devices</p>
            <p>{ goalCount } Active Goals</p>
          </div>
          <div className='p-6 pt-0 flex justify-center'>
            <button
              className='select-none rounded-lg bg-blue-500 py-3 px-6 text-center align-middle font-sans text-xs font-bold uppercase text-white shadow-md shadow-blue-500/20 transition-all hover:shadow-lg hover:shadow-blue-500/40 focus:opacity-[0.85] focus:shadow-none active:opacity-[0.85] active:shadow-none disabled:pointer-events-none disabled:opacity-50 disabled:shadow-none'
              type='button'
              data-ripple-light='true'
              onClick={ handleOpen }
            >
              View
            </button>
            { open && <TeammateForm teammateId={ teammate.id } handleClose={ handleClose } open={ open } key={ teammate.id } onTeammateEdit={ onTeammateEdit } /> }
          </div>
        </div>
      </div>
    </>
  );
};

export default TeammateCard;