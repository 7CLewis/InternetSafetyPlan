import React, { useEffect, useState } from 'react';

type Props = {
  tagList: string[];
  onTagChange: (tag: string[]) => void;
};

const TagInput = (props: Props) => {
  const { tagList, onTagChange } = props;

  const [tags, setTags] = useState<string[]>(tagList);
  const [inputValue, setInputValue] = useState<string>('');

  const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setInputValue(e.target.value);
  };

  useEffect(() => {
    onTagChange(tags);
  }, [tags]);

  const handleInputKeyPress = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === 'Enter' && inputValue.trim() !== '') {
      const newTag = inputValue.trim();
      setTags([...tags, newTag]);
      setInputValue('');
    }
  };

  const handleTagRemove = (tagToRemove: string) => {
    const updatedTags = tags.filter((tag) => tag !== tagToRemove);
    setTags(updatedTags);
  };

  return (
    <>
      <label className='px-4 text-sm'>Tags</label>
      <input
        type='text'
        className='mb-2 outline-0 outline rounded-[7px] border mr-4 px-4 py-1 text-md text-black border-transparent hover:border-blue-gray-200 focus:border-2 focus:border-blue-500'
        placeholder='Add tags'
        value={ inputValue }
        onChange={ handleInputChange }
        onKeyDown={ handleInputKeyPress }
      />
      <div className='flex space-x-2'>
        { tags.map((tag) => (
          <div
            key={ tag }
            className='bg-blue-100 px-2 py-1 rounded-md flex items-center'
          >
            <span>{ tag }</span>
            <button
              className='ml-2 text-red-500 hover:text-red-700 focus:outline-none'
              onClick={ () => handleTagRemove(tag) }
            >
              X
            </button>
          </div>
        )) }
      </div>
    </>
  );
};

export default TagInput;