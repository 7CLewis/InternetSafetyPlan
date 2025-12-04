function calculateAge(dateOfBirth: string) {
  const currentDate = new Date();
  const dob = new Date(dateOfBirth);
  let age = currentDate.getFullYear() - dob.getFullYear();

  if (
    currentDate.getMonth() < dob.getMonth() ||
    (currentDate.getMonth() === dob.getMonth() &&
      currentDate.getDate() < dob.getDate())
  ) age--; // Subtract 1 if the birthday hasn't occurred yet this year

  return age;
}

export default calculateAge;