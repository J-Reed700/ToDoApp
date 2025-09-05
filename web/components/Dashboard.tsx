import React, { useState, useEffect } from 'react';
import { 
  TaskCategory, 
  TaskItem, 
  CreateTaskRequest, 
  CreateCategoryRequest 
} from '../types';

import { 
  TaskBoard, 
  AddTaskForm, 
  AddCategoryForm 
} from '../features/TaskManagement/components';

import { useApiService } from '../context';

import './Dashboard.css';

export const Dashboard: React.FC = () => {
  const [categories, setCategories] = useState<TaskCategory[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [showAddForm, setShowAddForm] = useState(false);
  const [showAddCategory, setShowAddCategory] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const apiService = useApiService();

  const currentList = categories[0];
  useEffect(() => {
    loadTasks();
  }, []);

  const loadTasks = async () => {
    if (isLoading) return;
    
    setIsLoading(true);
    try {
      const taskList = await apiService.getTaskLists();
      
      setCategories(taskList);
      setError(null);
    } catch (err) {
      setError('Failed to load tasks. Please check your backend connection.');
      console.error('Error loading tasks:', err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleAddTask = async (taskData: CreateTaskRequest) => {
    try {
      const newTask =await apiService.createTask(taskData);
      setShowAddForm(false);
      categories.find(l => l.id === taskData.categoryId)?.tasks.push(newTask);
    } catch (err) {
      setError('Failed to create task');
      console.error('Error creating task:', err);
    }
  };

  const handleAddCategory = async (listData: CreateCategoryRequest) => {
    try {
      const newCategory = await apiService.createCategory(listData);
      
      setShowAddCategory(false);
      categories.push(newCategory);
      setCategories(categories);
    } catch (err) {
      setError('Failed to create list');
      console.error('Error creating list:', err);
    }
  };

  const handleDeleteTask = async (taskId: number) => {
    
    try {
      await apiService.deleteTask(taskId);
      await loadTasks(); 
    } catch (err) {
      setError('Failed to delete task');
      console.error('Error deleting task:', err);
    }
  };

  const handleTaskUpdated = (updatedTask: TaskItem) => {
    setCategories(prevCategories => 
      prevCategories.map(category => ({
        ...category,
        tasks: category.tasks.map(task => 
          task.id === updatedTask.id ? updatedTask : task
        )
      }))
    );
  };


  return (
    <div className="dashboard">
      <header className="dashboard-header">
        <h1>📋 My Tasks</h1>
        <div className="button-group">
          {(!showAddCategory && <button 
            className="add-button"
            onClick={() => setShowAddForm(!showAddForm)}
          >
            {showAddForm || showAddCategory ? 'Cancel' : '+ Add Task'}
          </button>
          )}
          {(!showAddForm && <button 
            className="add-list-button"
            onClick={() => setShowAddCategory(!showAddCategory)}
          >
            {showAddCategory || showAddForm ? 'Cancel' : '+ Add Category'}
          </button>
          )}
        </div>
      </header>

      {error && (
        <div className="error-message">
          {error}
          <button onClick={() => setError(null)} className="dismiss-error">✕</button>
        </div>
      )}

      {showAddForm && currentList && (
        <div className="add-task-container">
          <AddTaskForm
            categoryId={currentList.id}
            onSubmit={handleAddTask}
            onCancel={() => setShowAddForm(false)}
            categories={categories}
          />
        </div>
      )}

      {showAddCategory && (
        <div className="add-list-container">
          <AddCategoryForm 
            onSubmit={handleAddCategory} 
            onCancel={() => setShowAddCategory(false)} 
          />
        </div>
      )}

      <TaskBoard
        lists={categories}
        onTaskUpdated={handleTaskUpdated}
        onTaskDeleted={handleDeleteTask}
        error={error}
        setError={setError}
      />
    </div>
  );
};
