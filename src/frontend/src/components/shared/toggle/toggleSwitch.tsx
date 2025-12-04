import React, { useState } from 'react';

type Props = {
  initialToggleValue: boolean;
  onToggle: () => void;
}

const ToggleSwitch = (props: Props) => {
  const { initialToggleValue, onToggle } = props;
  const [isComplete, setIsComplete] = useState(initialToggleValue);

  const handleToggle = () => {
    setIsComplete(!isComplete);
    onToggle();
  };

  return (
    <>
      <input
        type='checkbox'
        className='hidden'
        checked={ isComplete }
        onChange={ handleToggle }
      />
      <label className='toggle-switch relative inline-flex items-center cursor-pointer' onClick={ handleToggle }>
        <span className={ `toggle-slider w-16 h-8 rounded-full bg-gray-300 dark:bg-gray-800 relative inline-flex items-center transition-all duration-300 ${isComplete ? 'bg-green-400' : ''}` }>
          <span className={ `toggle-knob w-6 h-6 rounded-full bg-white dark:bg-gray-900 shadow-md transform ${isComplete ? 'translate-x-10' : ''}` } />
        </span>
      </label>
    </>
  );
};

export default ToggleSwitch;