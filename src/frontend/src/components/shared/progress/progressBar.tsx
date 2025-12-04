import React from 'react';

type Props = {
  completedCount: number;
  totalCount: number;
  dataType: string;
}

const ProgressBar = (props: Props) => {
  const completedPercent = ((props.completedCount / props.totalCount) * 100);

  const progressBarStyle = {
    width: `${completedPercent == 0 ? 3 : completedPercent}%`,
    backgroundColor: completedPercent < 3 ? 'gray' : completedPercent < 40 ? 'gold' : completedPercent < 70 ? 'green' : 'darkgreen'
  };

	return (
    <>
      <div className='h-[20px] w-full bg-gray-300 rounded-md text-center mb-2' >
        <div className={ 'h-full rounded-md flex justify-end items-center pr-2 font-bold' } style={ progressBarStyle }>
          <span className='text-white'>{ `${ completedPercent.toFixed() }%` }</span>
        </div>
      </div>
    </>
	);
};

export default ProgressBar;
