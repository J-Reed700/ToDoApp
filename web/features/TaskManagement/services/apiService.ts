import { TaskCategory, TaskItem, TaskComment } from "../domain/types";
import { CreateTaskRequest, CreateCategoryRequest, CreateCommentRequest, UpdateCommentRequest } from "./types";
import { BaseApiService } from "../../../services/api";
import { Endpoint } from "./endpoint";


export class ApiService extends BaseApiService {

  async getTaskLists(): Promise<TaskCategory[]> {
    const list = await this.request<TaskCategory[]>(Endpoint.Categories, { expectResponse: true });
    
    return list || [];
  }

  // Create a new task
  async createTask(task: CreateTaskRequest): Promise<TaskItem> {
    const result = await this.request<TaskItem>(Endpoint.Task, {
      method: 'POST',
      body: JSON.stringify(task),
      expectResponse: true,
    });
    
    if (!result) {
      throw new Error('Failed to create task - no response received');
    }
    
    return result;
  }

  // Update task status
  async updateTask(id: number, updates: Partial<TaskItem>): Promise<TaskItem> {
    const result = await this.request<TaskItem>(`${Endpoint.Task}/${id}`, {
      method: 'PUT',
      body: JSON.stringify(updates),
      expectResponse: true,
    });
    
    if (!result) {
      throw new Error('Failed to update task - no response received');
    }
    
    return result;
  }

  // Delete a task
  async deleteTask(id: number): Promise<void> {
    return this.request(`${Endpoint.Task}/${id}`, {
      method: 'DELETE',
      expectResponse: false, // DELETE operations typically don't return data
    });
  }

  // Create a category
  async createCategory(category: CreateCategoryRequest): Promise<TaskCategory> {
    const result = await this.request<TaskCategory>(Endpoint.Categories, {
      method: 'POST',
      body: JSON.stringify(category),
      expectResponse: true,
    });
    return result;
  }

  // Create a default category if none exists
  async createDefaultList(): Promise<TaskCategory> {
    const result = await this.request<TaskCategory>(Endpoint.Categories, {
      method: 'POST',
      body: JSON.stringify({
        categoryName: 'My Tasks'
      }),
      expectResponse: true,
    });
    
    if (!result) {
      throw new Error('Failed to create default category - no response received');
    }
    
    return result;
  }

  // Comment operations
  async getTaskComments(taskId: number): Promise<TaskComment[]> {
    const result = await this.request<TaskComment[]>(`${Endpoint.TaskComments}?taskId=${taskId}`, {
      expectResponse: true,
    });
    return result || [];
  }

  async createComment(comment: CreateCommentRequest): Promise<TaskComment> {
    return await this.request<TaskComment>(Endpoint.TaskComments, {
      method: 'POST',
      body: JSON.stringify(comment),
      expectResponse: true,
    });
  }

  async updateComment(commentId: number, comment: UpdateCommentRequest): Promise<void> {
    await this.request(`${Endpoint.TaskComments}/${commentId}`, {
      method: 'PUT',
      body: JSON.stringify(comment),
      expectResponse: false,
    });
  }

  async deleteComment(commentId: number): Promise<void> {
    await this.request(`${Endpoint.TaskComments}/${commentId}`, {
      method: 'DELETE',
      expectResponse: false,
    });
  }
}