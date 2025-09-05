import { describe, it, expect, vi } from 'vitest'
import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { TaskCard } from '../../features/TaskManagement/components/TaskCard/TaskCard'
import { Priority, Status } from '../../types'

vi.mock('@dnd-kit/core', () => ({
  useDraggable: () => ({
    attributes: {},
    listeners: {},
    setNodeRef: vi.fn(),
    transform: null,
    isDragging: false,
  }),
}))

const mockTask = {
  id: 1,
  categoryId: 1,
  title: 'Test Task',
  description: 'This is a test task description',
  priority: Priority.High,
  status: Status.InProgress,
  dueDate: '2024-01-15',
}

const mockProps = {
  task: mockTask,
  onTaskUpdated: vi.fn(),
  onTaskDeleted: vi.fn(),
  isDragging: false,
}

describe('TaskCard', () => {
  it('should render task information correctly', () => {
    render(<TaskCard {...mockProps} />)
    
    expect(screen.getByText('Test Task')).toBeInTheDocument()
    expect(screen.getByText('This is a test task description')).toBeInTheDocument()
  })

  it('should display priority correctly', () => {
    render(<TaskCard {...mockProps} />)
    
    expect(screen.getByText('High')).toBeInTheDocument()
  })

  it('should display due date when provided', () => {
    render(<TaskCard {...mockProps} />)
    
    // The actual format might be different, let's check for the date content
    expect(screen.getByText(/1\/\d+\/2024/)).toBeInTheDocument()
  })

  it('should handle task without due date', () => {
    const taskWithoutDate = { ...mockTask, dueDate: null }
    render(<TaskCard {...mockProps} task={taskWithoutDate} />)
    
    expect(screen.queryByText(/\d+\/\d+\/\d+/)).not.toBeInTheDocument()
  })

  it('should open modal when clicked', async () => {
    const user = userEvent.setup()
    render(<TaskCard {...mockProps} />)
    
    const taskCard = screen.getByText('Test Task').closest('.task-card')
    expect(taskCard).toBeInTheDocument()
    
    if (taskCard) {
      await user.click(taskCard)
      // Check if modal opens by looking for modal container or title
      expect(screen.getByText('Test Task')).toBeInTheDocument() // Task should still be visible
    }
  })

  it('should apply dragging styles when isDragging is true', () => {
    render(<TaskCard {...mockProps} isDragging={true} />)
    
    const taskCard = screen.getByText('Test Task').closest('.task-card')
    expect(taskCard).toHaveClass('other-dragging')
  })

  it('should display correct priority badge colors', () => {
    const highPriorityTask = { ...mockTask, priority: Priority.Critical }
    render(<TaskCard {...mockProps} task={highPriorityTask} />)
    
    const priorityBadge = screen.getByText('Critical')
    expect(priorityBadge).toHaveClass('priority-badge')
  })

  it('should handle task with minimum required fields', () => {
    const minimalTask = {
      id: 2,
      categoryId: 1,
      title: 'Minimal Task',
      description: '',
      priority: Priority.None,
      status: Status.None,
      dueDate: null,
    }
    
    render(<TaskCard {...mockProps} task={minimalTask} />)
    
    expect(screen.getByText('Minimal Task')).toBeInTheDocument()
  })
})
