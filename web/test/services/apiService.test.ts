import { describe, it, expect, vi, beforeEach } from 'vitest'
import { ApiService } from '../../features/TaskManagement/services/apiService'
import { Priority, Status } from '../../types'

// Mock the BaseApiService
vi.mock('../../services/api', () => ({
  BaseApiService: vi.fn().mockImplementation(() => ({
    request: vi.fn(),
  })),
}))

describe('ApiService', () => {
  let apiService: ApiService
  let mockRequest: any

  beforeEach(() => {
    mockRequest = vi.fn()
    
    // Create a proper mock that extends the base class
    apiService = Object.create(ApiService.prototype)
    ;(apiService as any).request = mockRequest
    
    vi.clearAllMocks()
  })

  describe('getTaskLists', () => {
    it('should fetch task lists successfully', async () => {
      const mockCategories = [
        { id: 1, categoryName: 'Work', colour: '#blue', tasks: [] },
      ]
      
      mockRequest.mockResolvedValueOnce(mockCategories)
      
      const result = await apiService.getTaskLists()
      
      expect(result).toEqual(mockCategories)
      expect(mockRequest).toHaveBeenCalledWith('/categories', { expectResponse: true })
    })

    it('should return empty array when API returns null', async () => {
      mockRequest.mockResolvedValueOnce(null)
      
      const result = await apiService.getTaskLists()
      
      expect(result).toEqual([])
    })

    it('should return empty array when categories is undefined', async () => {
      mockRequest.mockResolvedValueOnce(undefined)
      
      const result = await apiService.getTaskLists()
      
      expect(result).toEqual([])
    })
  })

  describe('createTask', () => {
    it('should create a task successfully', async () => {
      const newTask = {
        categoryId: 1,
        title: 'New Task',
        description: 'Task description',
        priority: Priority.High,
        status: Status.None,
        dueDate: '2024-01-15',
      }
      
      const expectedTask = { ...newTask, id: 1 }
      mockRequest.mockResolvedValueOnce(expectedTask)
      
      const result = await apiService.createTask(newTask)
      
      expect(result).toEqual(expectedTask)
      expect(mockRequest).toHaveBeenCalledWith('/task', {
        method: 'POST',
        body: JSON.stringify(newTask),
        expectResponse: true,
      })
    })
  })

  describe('updateTask', () => {
    it('should update a task successfully', async () => {
      const taskId = 1
      const updates = {
        title: 'Updated Task',
        status: Status.Completed,
      }
      
      const updatedTask = { id: taskId, ...updates }
      mockRequest.mockResolvedValueOnce(updatedTask)
      
      const result = await apiService.updateTask(taskId, updates)
      
      expect(result).toEqual(updatedTask)
      expect(mockRequest).toHaveBeenCalledWith('/task/1', {
        method: 'PUT',
        body: JSON.stringify(updates),
        expectResponse: true,
      })
    })
  })

  describe('deleteTask', () => {
    it('should delete a task successfully', async () => {
      const taskId = 1
      mockRequest.mockResolvedValueOnce(undefined)
      
      await apiService.deleteTask(taskId)
      
      expect(mockRequest).toHaveBeenCalledWith('/task/1', {
        method: 'DELETE',
        expectResponse: false,
      })
    })
  })

  describe('createCategory', () => {
    it('should create a category successfully', async () => {
      const newCategory = {
        categoryName: 'New Category',
      }
      
      const expectedCategory = { 
        ...newCategory, 
        id: 1, 
        colour: '#defaultcolor',
        tasks: [] 
      }
      mockRequest.mockResolvedValueOnce(expectedCategory)
      
      const result = await apiService.createCategory(newCategory)
      
      expect(result).toEqual(expectedCategory)
      expect(mockRequest).toHaveBeenCalledWith('/categories', {
        method: 'POST',
        body: JSON.stringify(newCategory),
        expectResponse: true,
      })
    })
  })

  describe('error handling', () => {
    it('should throw error when API request fails', async () => {
      const error = new Error('API Error')
      mockRequest.mockRejectedValueOnce(error)
      
      await expect(apiService.getTaskLists()).rejects.toThrow('API Error')
    })
  })
})
