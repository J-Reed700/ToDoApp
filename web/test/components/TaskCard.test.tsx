import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { TaskCard } from '../../features/TaskManagement/components/TaskCard/TaskCard'
import { Priority, Status } from '../../types'
import { ApiServiceProvider } from '../../context/apiServiceContext'
import { mockApiService } from '../mocks/apiService'

vi.stubGlobal('confirm', vi.fn())

vi.mock('@dnd-kit/core', () => ({
  useDraggable: () => ({
    attributes: {},
    listeners: {},
    setNodeRef: vi.fn(),
    transform: null,
    isDragging: false,
  }),
}))

vi.mock('../../context/apiServiceContext', async () => {
  const actual = await vi.importActual('../../context/apiServiceContext')
  return {
    ...actual,
    useApiService: () => mockApiService,
  }
})

const mockTask = {
  id: 1,
  categoryId: 1,
  title: 'Test Task',
  description: 'This is a test task description',
  priority: Priority.High,
  status: Status.InProgress,
  dueDate: '2024-01-15',
}

const mockCategories = [
  { id: 1, categoryName: 'Work', colour: '#blue', tasks: [] },
  { id: 2, categoryName: 'Personal', colour: '#green', tasks: [] }
]

const mockProps = {
  task: mockTask,
  onTaskUpdated: vi.fn(),
  onTaskDeleted: vi.fn(),
  isDragging: false,
  availableCategories: mockCategories,
}

describe('TaskCard', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  describe('Rendering', () => {
    it('should render task information correctly', () => {
      render(<TaskCard {...mockProps} />)
      
      expect(screen.getByText('Test Task')).toBeInTheDocument()
      expect(screen.getByText('This is a test task description')).toBeInTheDocument()
      expect(screen.getByText(/Work/)).toBeInTheDocument()
    })

    it('should display priority badge when priority is not None', () => {
      render(<TaskCard {...mockProps} />)
      
      const priorityBadge = screen.getByText('High')
      expect(priorityBadge).toBeInTheDocument()
      expect(priorityBadge).toHaveClass('priority-badge')
    })

    it('should not display priority badge when priority is None', () => {
      const taskWithNoPriority = { ...mockTask, priority: Priority.None }
      render(<TaskCard {...mockProps} task={taskWithNoPriority} />)
      
      expect(screen.queryByText('None')).not.toBeInTheDocument()
    })

    it('should display due date when provided', () => {
      render(<TaskCard {...mockProps} />)
      
      expect(screen.getByText(/1\/\d+\/2024/)).toBeInTheDocument()
    })

    it('should not display due date when null', () => {
      const taskWithoutDate = { ...mockTask, dueDate: null }
      render(<TaskCard {...mockProps} task={taskWithoutDate} />)
      
      expect(screen.queryByText(/\d+\/\d+\/\d+/)).not.toBeInTheDocument()
    })

    it('should not display description when empty', () => {
      const taskWithoutDescription = { ...mockTask, description: '' }
      render(<TaskCard {...mockProps} task={taskWithoutDescription} />)
      
      expect(screen.queryByText('This is a test task description')).not.toBeInTheDocument()
    })
  })

  describe('User Interactions', () => {
    it('should call onTaskDeleted when delete button clicked and confirmed', async () => {
      const user = userEvent.setup()
      vi.mocked(confirm).mockReturnValue(true)
      
      render(<TaskCard {...mockProps} />)
      
      const deleteButton = screen.getByTitle('Delete task')
      await user.click(deleteButton)
      
      expect(confirm).toHaveBeenCalledWith('Are you sure you want to delete this task?')
      expect(mockProps.onTaskDeleted).toHaveBeenCalledWith(1)
    })

    it('should not call onTaskDeleted when delete cancelled', async () => {
      const user = userEvent.setup()
      vi.mocked(confirm).mockReturnValue(false)
      
      render(<TaskCard {...mockProps} />)
      
      const deleteButton = screen.getByTitle('Delete task')
      await user.click(deleteButton)
      
      expect(confirm).toHaveBeenCalled()
      expect(mockProps.onTaskDeleted).not.toHaveBeenCalled()
    })

    it('should open modal when edit button clicked', async () => {
      const user = userEvent.setup()
      render(
        <ApiServiceProvider>
          <TaskCard {...mockProps} />
        </ApiServiceProvider>
      )
      
      const editButton = screen.getByTitle('Edit task')
      await user.click(editButton)
      
      expect(screen.getByText('Task Details')).toBeInTheDocument()
      expect(screen.getByText(/Comments \(/)).toBeInTheDocument()
    })

    it('should open modal when card content clicked', async () => {
      const user = userEvent.setup()
      render(
        <ApiServiceProvider>
          <TaskCard {...mockProps} />
        </ApiServiceProvider>
      )
      
      const cardContent = screen.getByText('Test Task')
      await user.click(cardContent)
      
      expect(screen.getByText('Task Details')).toBeInTheDocument()
      expect(screen.getByRole('dialog')).toBeInTheDocument()
    })
  })

  describe('Dragging Behavior', () => {
    it('should apply dragging styles when isDragging is true', () => {
      render(<TaskCard {...mockProps} isDragging={true} />)
      
      const taskCard = screen.getByText('Test Task').closest('.task-card')
      expect(taskCard).toHaveClass('other-dragging')
    })

    it('should display drag handle', () => {
      render(<TaskCard {...mockProps} />)
      
      const dragHandle = screen.getByTitle('Drag to move')
      expect(dragHandle).toBeInTheDocument()
      expect(dragHandle).toHaveClass('drag-handle')
    })
  })

  describe('Priority Display', () => {
    const priorityTestCases = [
      { priority: Priority.Low, label: 'Low' },
      { priority: Priority.Medium, label: 'Medium' },
      { priority: Priority.High, label: 'High' },
      { priority: Priority.Critical, label: 'Critical' }
    ]

    priorityTestCases.forEach(({ priority, label }) => {
      it(`should display ${label} priority correctly`, () => {
        const taskWithPriority = { ...mockTask, priority }
        render(<TaskCard {...mockProps} task={taskWithPriority} />)
        
        const priorityBadge = screen.getByText(label)
        expect(priorityBadge).toBeInTheDocument()
        expect(priorityBadge).toHaveClass('priority-badge')
      })
    })
  })

  describe('Category Display', () => {
    it('should display category name when category exists', () => {
      render(<TaskCard {...mockProps} />)
      
      expect(screen.getByText(/Work/)).toBeInTheDocument()
    })

    it('should display "Unknown Category" when category not found', () => {
      const taskWithUnknownCategory = { ...mockTask, categoryId: 999 }
      render(<TaskCard {...mockProps} task={taskWithUnknownCategory} />)
      
      expect(screen.getByText(/Unknown Category/)).toBeInTheDocument()
    })

    it('should display "Unknown Category" when no categories provided', () => {
      render(<TaskCard {...mockProps} availableCategories={undefined} />)
      
      expect(screen.getByText(/Unknown Category/)).toBeInTheDocument()
    })
  })
})
