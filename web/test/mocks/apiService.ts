import { vi } from 'vitest'
import { TaskCategory, TaskItem, Priority, Status } from '../../types'

// Mock data
export const mockCategories: TaskCategory[] = [
  {
    id: 1,
    categoryName: 'Work Tasks',
    colour: '#3B82F6',
    tasks: [
      {
        id: 1,
        categoryId: 1,
        title: 'Complete project',
        description: 'Finish the React app',
        priority: Priority.High,
        status: Status.InProgress,
        dueDate: '2024-01-15',
      },
      {
        id: 2,
        categoryId: 1,
        title: 'Review code',
        description: 'Review pull requests',
        priority: Priority.Medium,
        status: Status.ToDo,
        dueDate: '2024-01-20',
      },
    ],
  },
  {
    id: 2,
    categoryName: 'Personal',
    colour: '#10B981',
    tasks: [
      {
        id: 3,
        categoryId: 2,
        title: 'Buy groceries',
        description: 'Weekly shopping',
        priority: Priority.Low,
        status: Status.Completed,
        dueDate: '2024-01-10',
      },
    ],
  },
]

export const mockApiService = {
  getTaskLists: vi.fn().mockResolvedValue(mockCategories),
  createTask: vi.fn().mockImplementation((task) => 
    Promise.resolve({ ...task, id: Date.now() })
  ),
  updateTask: vi.fn().mockImplementation((id, task) =>
    Promise.resolve({ ...task, id })
  ),
  deleteTask: vi.fn().mockResolvedValue(undefined),
  createCategory: vi.fn().mockImplementation((category) =>
    Promise.resolve({ ...category, id: Date.now(), tasks: [] })
  ),
}
