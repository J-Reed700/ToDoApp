import React, { useState } from 'react';
import { useDraggable } from '@dnd-kit/core';
import { 
  TaskItem, 
  Priority, 
  TaskCategory } from '../../domain/types';
import { TaskModal } from '../TaskModal/TaskModal';
import { formatDisplayDate } from '../../utils/dateUtils';
import './TaskCard.css';

interface TaskCardProps {
  task: TaskItem;
  onTaskUpdated: (updatedTask: TaskItem) => void;
  onTaskDeleted: (taskId: number) => void;
  isDragging?: boolean;
  availableCategories?: TaskCategory[];
}

const PriorityColors = {
  [Priority.None]: '#6B7280',
  [Priority.Low]: '#10B981',
  [Priority.Medium]: '#F59E0B',
  [Priority.High]: '#F97316',
  [Priority.Critical]: '#EF4444'
};

const PriorityLabels = {
  [Priority.None]: 'None',
  [Priority.Low]: 'Low',
  [Priority.Medium]: 'Medium',
  [Priority.High]: 'High',
  [Priority.Critical]: 'Critical'
};

export const TaskCard: React.FC<TaskCardProps> = ({
  task,
  onTaskUpdated,
  onTaskDeleted,
  isDragging = false,
  availableCategories = []
}) => {
  const [showTaskModal, setShowTaskModal] = useState(false);
  

  const taskCategory = availableCategories.find(cat => cat.id === task.categoryId);
  const categoryName = taskCategory?.categoryName || 'Unknown Category';
  
  const {
    attributes,
    listeners,
    setNodeRef,
    transform,
    isDragging: isBeingDragged
  } = useDraggable({
    id: task.id,
    data: { task }
  });

  const handleClick = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (!isBeingDragged) {
      setShowTaskModal(true);
    }
  };

  const handleDelete = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    if (confirm('Are you sure you want to delete this task?')) {
      onTaskDeleted(task.id);
    }
  };

  const handleEdit = (e: React.MouseEvent) => {
    e.preventDefault();
    e.stopPropagation();
    setShowTaskModal(true);
  };

  const cardClasses = [
    'task-card',
    isBeingDragged ? 'dragging' : '',
    isDragging ? 'other-dragging' : ''
  ].filter(Boolean).join(' ');

  const style = transform ? {
    transform: `translate3d(${transform.x}px, ${transform.y}px, 0)`,
  } : undefined;

  return (
    <>
      <div
        ref={setNodeRef}
        className={cardClasses}
        style={style}
        {...attributes}
      >
        <div className="card-content" onClick={handleClick}>
          <div className="card-header">
            <h4 className="card-title">{task.title}</h4>
            <button
              className="edit-button"
              onClick={handleEdit}
              title="Edit task"
            >
              ✏️
            </button>
            <button
              className="delete-button"
              onClick={handleDelete}
              title="Delete task"
            >
              ✕
            </button>
          </div>
          
          {task.description && (
            <p className="card-description">{task.description}</p>
          )}
          
          <div className="card-footer">
            <div className="card-meta">
              <span className="category-name">
                📂 {categoryName}
              </span>
              
              {task.priority !== Priority.None && (
                <span
                  className="priority-badge"
                  style={{ backgroundColor: PriorityColors[task.priority] }}
                >
                  {PriorityLabels[task.priority]}
                </span>
              )}
              
              {task.dueDate && (
                <span className="due-date">
                  📅 {formatDisplayDate(task.dueDate)}
                </span>
              )}
            </div>
          </div>
        </div>
        
        <div 
          className="drag-handle" 
          title="Drag to move"
          {...listeners}
        >
          ⋮⋮
        </div>
      </div>

      {showTaskModal && (
        <TaskModal
          task={task}
          showTaskModal={showTaskModal}
          setShowTaskModal={setShowTaskModal}
          onTaskUpdated={onTaskUpdated}
          availableCategories={availableCategories}
        />
      )}
    </>
  );
};
