import { TeamUltimateGoalsResponse } from 'library/ultimateGoalAggregate/queries/TeamUltimateGoalsResponse';
import React, { useState, useRef } from 'react';
import { FaCheck, FaPen, FaTimes, FaTrash } from 'react-icons/fa';
import { getTeamIdFromLocalStorage } from 'utils/local-storage/localStorage';
import { useCreateUltimateGoalMutation, useDeleteUltimateGoalMutation, useUpdateUltimateGoalMutation } from 'utils/redux/api/ultimateGoalApiSlice';

type Props = {
  ultimateGoal: TeamUltimateGoalsResponse;
  isEditMode?: boolean;
  onDelete: (id: string) => void;
};

const UltimateGoalEditableRow = (props: Props) => {
  const [id, setId] = useState(props.ultimateGoal.id);
  const [name, setName] = useState(props.ultimateGoal.name);
  const [description, setDescription] = useState(props.ultimateGoal.description);

  const teamId = getTeamIdFromLocalStorage();

  const [updateUltimateGoal, {
    isError: updateUltimateGoalResponseIsError
  }] = useUpdateUltimateGoalMutation();

  const [createUltimateGoal, {
    isError: createUltimateGoalResponseIsError
  }] = useCreateUltimateGoalMutation();

  const [deleteUltimateGoal, {
    isError: deleteUltimateGoalResponseIsError
  }] = useDeleteUltimateGoalMutation();

  const [isEditMode, setEditMode] = useState(props.isEditMode ?? false);
  const idRef = useRef<HTMLInputElement>(null);
  const nameRef = useRef<HTMLInputElement>(null);
  const descriptionRef = useRef<HTMLTextAreaElement>(null);

  const handleEditClick = () => {
    setEditMode(!isEditMode);
  };

  const handleSaveClick = () => {
    if (idRef.current && nameRef.current && descriptionRef.current) {
      setEditMode(false);

      const submittedName = nameRef.current.value;
      const submittedDescription = descriptionRef.current.value;
      setName(submittedName);
      setDescription(submittedDescription);

      if (id === '')
      {
        createUltimateGoal({ teamId: teamId, name: submittedName, description: submittedDescription })
          .unwrap()
          .then((response) => {
            console.log('Ultimate Goal created successfully. ' + JSON.stringify(response));
            if (response.value) setId(response.value);
          })
          .catch((error) => {
            console.log(error);
          });
          return;
      }

      if (submittedName != name || submittedDescription != description) {
        updateUltimateGoal({ ultimateGoalId: id, name: submittedName, description: submittedDescription != '' ? submittedDescription : null })
        .unwrap()
        .then((response) => {
          if (response.isSuccess) console.log('Ultimate Goal ' + id + ' updated successfully. ');
          else console.log('An error occurred while trying to update Ultimate Goal' + id + '. Error: ' + response.error.message);
        })
        .catch((error) => {
          console.log(error);
        });
      }
    }
  };

  const handleDeleteClick = () => {
    const result = window.confirm('Are you sure you want to delete this Ultimate Goal? This will result in the removal of all Goals and Action Items associated with it. This action is not recommended.');
    if (result) {
      deleteUltimateGoal(id)
      .unwrap()
      .then((response) => {
        if (response.isSuccess) {
          console.log('Ultimate Goal ' + id + ' delete successfully. ');
          props.onDelete(id);
        }
        else console.log('An error occurred while trying to delete Ultimate Goal' + id + '. Error: ' + response.error.message);
      })
      .catch((error) => {
        console.log(error);
      });
    }
  };

  if (updateUltimateGoalResponseIsError || createUltimateGoalResponseIsError || deleteUltimateGoalResponseIsError) return (<p>Error</p>);

  return (
    <tr key={ id }>
      <td className='border px-4 py-2'>
        <input type='hidden' ref={ idRef } value={ id } />
        { isEditMode ? (
          <input
            type='text'
            defaultValue={ name ?? '' }
            maxLength={ 500 }
            className='border rounded w-full'
            ref={ nameRef }
          />
        ) : (
          name
        ) }
      </td>
      <td className='border px-4 py-2'>
        { isEditMode ? (
          <textarea
            defaultValue={ description ?? '' }
            maxLength={ 500 }
            className='w-full resize-none border rounded'
            ref={ descriptionRef }
          />
        ) : (
          description
        ) }
      </td>
      <td className='border px-4 py-2'>
        { !isEditMode ? (
          <div className='flex flex-row justify-center'>
            <FaPen
              className='cursor-pointer w-5 h-5 mr-1 text-gray-500'
              onClick={ handleEditClick }
            />
          </div>
        ) : (
          <div className='flex flex-row justify-center'>
            <FaCheck
              className='cursor-pointer w-6 h-6 mr-2 text-green-500'
              onClick={ handleSaveClick }
            />
            <FaTimes
              className='cursor-pointer w-6 h-6 mr-7 text-gray-500'
              onClick={ handleEditClick }
            />
            { id ? (
              <FaTrash
                className='cursor-pointer w-6 h-6 mr-1 text-red-500'
                onClick={ handleDeleteClick }
              />) : (<></>)
            }
          </div>
        ) }
      </td>
    </tr>
  );
};

export default UltimateGoalEditableRow;
