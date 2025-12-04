import React from 'react';

type Props = {
  text?: string | null;
}

const CircleLoader = (props: Props) => {
  const { text } = props;

  return (
    <div className='fixed top-0 left-0 w-full h-full flex justify-center items-center z-50'>
      <div className='bg-white p-4 rounded-md shadow-md text-center flex flex-col justify-center items-center'>
        <div className='animate-spin rounded-full h-16 w-16 border-t-4 border-blue-500 border-solid'></div>
        <p className='mt-4 text-gray-800'>{ text }</p>
      </div>
    </div>
  );
};

export default CircleLoader;