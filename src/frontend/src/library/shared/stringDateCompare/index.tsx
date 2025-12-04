function dateCompare(stringFirstDate: string | null, secondDate: Date): number {
  if (stringFirstDate === null) return 1;
  const firstDate = new Date(stringFirstDate);

  if (firstDate > secondDate) return 1;
  if (firstDate == secondDate) return 0;
  return -1;
}

export default dateCompare;