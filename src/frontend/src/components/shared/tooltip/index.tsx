import React from 'react';

type Props = {
  text: string;
  children: React.ReactNode;
}

const Tooltip = (props: Props) => {
  const { text, children } = props;

  return (
    <>
      <div className='relative group'>
        { children }
        <div className='opacity-0 bg-gray-700 text-white text-xs rounded-lg py-1 px-2 absolute left-full top-1/2 transform -translate-y-1/2 transition-opacity duration-300 group-hover:opacity-100'>
          { text }
        </div>
      </div>
    </>
  );
};

export default Tooltip;