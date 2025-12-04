import { Teammate } from 'library/teamAggregate/Teammate';
import React from 'react';
import TeammateForm from 'components/team/teammateForm';

type Props = {
  teammate: Teammate;
};

const TeammateBar = (props: Props) => {
  const { teammate } = props;
  const [open, setOpen] = React.useState(false);

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

  return (
    <div className='m-2 border border-gray-300 rounded p-2 bg-gray-100'>
      <li
        key={ teammate.id }
        className='flex items-center justify-between'
      >
        <div className='flex items-center'>
          <img
            src={ `src/assets/images/${teammate.id}.jpg` }
            alt={ `${teammate.name}'s profile picture` }
            className='w-8 h-8 rounded-full mr-2'
          />
          <span>{ teammate.name }</span>
        </div>
        <div>
          <button className='underline' onClick={ handleOpen }>View</button>
          { open && <TeammateForm teammateId={ teammate.id } handleClose={ handleClose } open={ open } key={ teammate.id } /> }
        </div>
      </li>
    </div>
  );
};

export default TeammateBar;