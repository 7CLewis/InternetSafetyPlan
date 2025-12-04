import DeviceType from 'library/deviceAggregate/DeviceType';
import React from 'react';
import { FiWatch } from 'react-icons/fi';
import { FaBook, FaCar, FaTablet, FaQuestion } from 'react-icons/fa';
import { GrPersonalComputer } from 'react-icons/gr';
import { BsPhone, BsRouter, BsTv } from 'react-icons/bs';
import { RiHomeGearLine } from 'react-icons/ri';

type Props = {
  typeId: number;
  size: number;
}

const DeviceIcon = (props: Props) => {
  const { typeId, size } = props;

  switch (DeviceType[typeId]) {
    case 'E-Reader':
      return <FaBook size={ size }></FaBook>;
    case 'PC':
      return <GrPersonalComputer size={ size }></GrPersonalComputer>;
    case 'Phone':
      return <BsPhone size={ size }></BsPhone>;
    case 'Router':
      return <BsRouter size={ size }></BsRouter>;
    case 'TV':
      return <BsTv size={ size }></BsTv>;
    case 'Tablet':
      return <FaTablet size={ size }></FaTablet>;
    case 'Vehicle':
      return <FaCar size={ size }></FaCar>;
    case 'Watch':
      return <FiWatch size={ size }></FiWatch>;
    case 'Home Assistant':
      return <RiHomeGearLine size={ size }></RiHomeGearLine>;
    default:
      return <FaQuestion size={ size }></FaQuestion>;
  }
};

export default DeviceIcon;