export const formatDate = (date: string) => {
  // I want to extract and format with format dd/mm/yyyy from date
  const day = new Date(date).getDate();
  const month = new Date(date).getMonth() + 1;
  const year = new Date(date).getFullYear();
  return `${day}/${month}/${year}`;
};
