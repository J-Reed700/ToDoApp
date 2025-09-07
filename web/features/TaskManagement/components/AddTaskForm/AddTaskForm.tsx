import React, { useState } from 'react';
import { CreateTaskRequest } from '../../services/types';
import { Priority, Status, TaskCategory } from '../../domain/types';
import './AddTaskForm.css';

interface AddTaskFormProps {
  categoryId: number;
  onSubmit: (task: CreateTaskRequest) => void;
  onCancel: () => void;
  categories: TaskCategory[];
}

export const AddTaskForm: React.FC<AddTaskFormProps> = ({ categoryId: defaultCategoryId, onSubmit, onCancel, categories }) => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [priority, setPriority] = useState<CreateTaskRequest['priority']>(Priority.Medium);
  const [status, setStatus] = useState<CreateTaskRequest['status']>(Status.ToDo);
  const [dueDate, setDueDate] = useState('');
  const [selectedCategoryId, setSelectedCategoryId] = useState(defaultCategoryId);

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!title.trim()) return;

    onSubmit({
      categoryId: selectedCategoryId,
      title: title.trim(),
      description: description.trim() || undefined,
      priority,
      status,
      dueDate: dueDate || undefined,
    });

    setTitle('');
    setDescription('');
    setPriority(Priority.Medium);
    setDueDate('');
    setSelectedCategoryId(defaultCategoryId);
  };

  return (
    <form className="add-task-form" onSubmit={handleSubmit}>
      <div className="form-row">
        <input
          type="text"
          placeholder="Task title..."
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          className="title-input"
          autoFocus
        />
        <label htmlFor="priority">Priority: </label>
        <select 
          value={priority} 
          onChange={(e) => setPriority(Number(e.target.value) as Priority)}
          className="priority-select"
        >
          <option value={Priority.Low}>Low</option>
          <option value={Priority.Medium}>Medium</option>
          <option value={Priority.High}>High</option>
          <option value={Priority.Critical}>Critical</option>
        </select>
        <label htmlFor="status">Status: </label>
        <select 
          value={status} 
          onChange={(e) => setStatus(Number(e.target.value) as Status)}
          className="status-select"
        >
          <option value={Status.ToDo}>Not Started</option>
          <option value={Status.InProgress}>In Progress</option>
          <option value={Status.Completed}>Completed</option>
          <option value={Status.OnHold}>On Hold</option>
          <option value={Status.Cancelled}>Cancelled</option>
        </select>
      </div>
      
      <textarea
        placeholder="Description (optional)..."
        value={description}
        onChange={(e) => setDescription(e.target.value)}
        className="description-input"
        rows={2}
      />
      
      <div className="form-row">
        <label htmlFor="dueDate">Due Date: </label>
        <input
          type="date"
          value={dueDate}
          onChange={(e) => setDueDate(e.target.value)}
          className="date-input"
        />
        <label htmlFor="categoryId">Category: </label>
        <select
            value={selectedCategoryId}
            onChange={(e) => setSelectedCategoryId(Number(e.target.value))}
            className="category-select"
            >
            {categories.map((category) => (
                <option key={category.id} value={category.id}>
                    {category.categoryName}
                </option>
            ))}
        </select>
        <div className="form-actions">
          <button type="button" onClick={onCancel} className="cancel-button">
            Cancel
          </button>
          <button type="submit" className="submit-button" disabled={!title.trim()}>
            Add Task
          </button>
        </div>
      </div>
    </form>
  );
};
