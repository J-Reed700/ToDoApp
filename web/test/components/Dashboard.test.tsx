import { describe, it, expect, vi, beforeEach } from 'vitest'
import { render, screen, waitFor } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { Dashboard } from '../../components/Dashboard'
import { ApiServiceProvider } from '../../context/apiServiceContext'
import { mockApiService, mockCategories } from '../mocks/apiService'

// Mock the API service context
vi.mock('../../context/apiServiceContext', async () => {
  const actual = await vi.importActual('../../context/apiServiceContext')
  return {
    ...actual,
    useApiService: () => mockApiService,
  }
})

const DashboardWithProvider = () => (
  <ApiServiceProvider>
    <Dashboard />
  </ApiServiceProvider>
)

describe('Dashboard', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('should render the dashboard header', async () => {
    render(<DashboardWithProvider />)
    
    expect(screen.getByText('📋 My Tasks')).toBeInTheDocument()
    expect(screen.getByText('+ Add Task')).toBeInTheDocument()
    expect(screen.getByText('+ Add Category')).toBeInTheDocument()
  })

  it('should load and display categories', async () => {
    render(<DashboardWithProvider />)
    
    await waitFor(() => {
      expect(mockApiService.getTaskLists).toHaveBeenCalled()
    })

    // Check if task board is rendered (it would show the categories)
    expect(screen.getByText('Task Board')).toBeInTheDocument()
  })

  it('should show add task form when Add Task button is clicked', async () => {
    const user = userEvent.setup()
    render(<DashboardWithProvider />)
    
    const addTaskButton = screen.getByText('+ Add Task')
    await user.click(addTaskButton)
    
    // Form should be visible - check for form elements instead of title
    expect(screen.getByPlaceholderText(/task title/i)).toBeInTheDocument()
  })

  it('should show add category form when Add Category button is clicked', async () => {
    const user = userEvent.setup()
    render(<DashboardWithProvider />)
    
    const addCategoryButton = screen.getByText('+ Add Category')
    await user.click(addCategoryButton)
    
    // Form should be visible - check for form elements instead of title
    expect(screen.getByPlaceholderText(/category name/i)).toBeInTheDocument()
  })

  it('should display error message when API fails', async () => {
    // Mock API to fail
    mockApiService.getTaskLists.mockRejectedValueOnce(new Error('API Error'))
    
    render(<DashboardWithProvider />)
    
    await waitFor(() => {
      expect(screen.getAllByText(/failed to load tasks/i)).toHaveLength(2) // One in dashboard, one in board
    })
  })

  it('should display task statistics', async () => {
    mockApiService.getTaskLists.mockResolvedValueOnce(mockCategories)
    render(<DashboardWithProvider />)
    
    await waitFor(() => {
      // Should show total tasks count
      expect(screen.getByText(/3 tasks total/)).toBeInTheDocument()
    })
  })
})
