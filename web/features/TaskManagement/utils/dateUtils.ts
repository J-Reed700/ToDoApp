/**
 * Utility functions for handling date operations in the Task feature
 */

/**
 * Converts a date value (Date object or string) to a Date object
 * Handles date strings as local dates to avoid timezone conversion issues
 * @param date - The date value to convert
 * @returns Date object or null if input is null/undefined
 */
export const toDate = (date: Date | string | null | undefined): Date | null => {
  if (!date) return null;
  if (date instanceof Date) return date;
  if (typeof date === 'string' && date.includes('T')) {
    const [datePart] = date.split('T');
    const [year, month, day] = datePart.split('-').map(Number);
    return new Date(year, month - 1, day); 
  }
  
  return new Date(date);
};

/**
 * Formats a date for display in the UI
 * @param date - The date to format
 * @returns Formatted date string or "Not set" if no date
 */
export const formatDisplayDate = (date: Date | string | null | undefined): string => {
  if (!date) return 'Not set';
  const dateObj = toDate(date);
  return dateObj ? dateObj.toLocaleDateString() : 'Not set';
};

/**
 * Converts a date to ISO string format for HTML date inputs
 * @param date - The date to convert
 * @returns ISO date string (YYYY-MM-DD) or empty string if no date
 */
export const toDateInputValue = (date: Date | string | null | undefined): string => {
  if (!date) return '';
  const dateObj = toDate(date);
  if (!dateObj) return '';
  
  try {
    return dateObj.toISOString().split('T')[0];
  } catch (error) {
    console.warn('Invalid date value:', date);
    return '';
  }
};

/**
 * Checks if a date value is valid
 * @param date - The date to validate
 * @returns true if the date is valid, false otherwise
 */
export const isValidDate = (date: Date | string | null | undefined): boolean => {
  if (!date) return false;
  const dateObj = toDate(date);
  return dateObj instanceof Date && !isNaN(dateObj.getTime());
};
