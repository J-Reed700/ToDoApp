import { Status, Priority } from '../types';

export const STATUS_CONFIG = {
  [Status.None]: { title: 'To Do', color: '#6B7280' },
  [Status.InProgress]: { title: 'In Progress', color: '#3B82F6' },
  [Status.OnHold]: { title: 'On Hold', color: '#F59E0B' },
  [Status.Cancelled]: { title: 'Cancelled', color: '#EF4444' },
  [Status.Completed]: { title: 'Done', color: '#10B981' },
} as const;

// Column Order for Task Board
export const COLUMN_ORDER = [
  Status.None,
  Status.OnHold,
  Status.InProgress, 
  Status.Completed,
  Status.Cancelled
] as const;

// Priority Labels
export const PRIORITY_LABELS = {
  [Priority.None]: 'None',
  [Priority.Low]: 'Low',
  [Priority.Medium]: 'Medium',
  [Priority.High]: 'High',
  [Priority.Critical]: 'Critical',
} as const;

// Date Formats
export const DATE_FORMATS = {
  DISPLAY: 'MMM dd, yyyy',
  FULL: 'MMMM dd, yyyy HH:mm',
  TIME: 'HH:mm',
} as const;
