export const setUserEmailInLocalStorage = (email: string) => {
  localStorage.setItem('userEmail', email);
};

export const setTeamIdInLocalStorage = (teamId: string) => {
  localStorage.setItem('teamId', teamId);
};

export const getTeamIdFromLocalStorage = (): string => {
  const value = localStorage.getItem('teamId');
  if (value == null) return window.location.href = '/login';
  return value;
};

export const getUserEmailFromLocalStorage = (): string => {
  const value = localStorage.getItem('userEmail');
  if (value == null) return window.location.href = '/login';
  return value;
};
