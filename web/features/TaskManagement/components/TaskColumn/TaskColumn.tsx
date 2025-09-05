import React from 'react';
import { useDroppable } from '@dnd-kit/core';
import { TaskItem, Status, TaskCategory } from '../../domain/types';
import { TaskCard } from '../TaskCard/TaskCard';
import './TaskColumn.css';

interface TaskColumnProps {
  status: Status;
  title: string;
  color: string;
  tasks: TaskItem[];
  onTaskUpdated: (updatedTask: TaskItem) => void;
  onTaskDeleted: (taskId: number) => void;
  availableCategories: TaskCategory[];
}

export const TaskColumn: React.FC<TaskColumnProps> = ({
  status,
  title,
  color,
  tasks,
  onTaskUpdated,
  onTaskDeleted,
  availableCategories
}) => {
  const { isOver, setNodeRef } = useDroppable({
    id: status.toString()
  });

  return (
    <div 
      ref={setNodeRef} 
      className={`task-column ${isOver ? 'drag-over' : ''} ${tasks.length === 0 ? 'empty-column' : ''}`}
    >
      <div className="column-header" style={{ borderTopColor: color }}>
        <h3 className="column-title">{title}</h3>
        <span className="task-count">{tasks.length}</span>
      </div>
      
      <div className="column-content">
        {tasks.length === 0 ? (
          <div className="empty-state">
            <p>No tasks</p>
          </div>
        ) : (
          tasks.map(task => (
            <TaskCard
              key={task.id}
              task={task}
              onTaskUpdated={onTaskUpdated}
              onTaskDeleted={onTaskDeleted}
              availableCategories={availableCategories}
            />
          ))
        )}
      </div>
      
      {isOver && (
        <div className="drop-indicator">
          Drop here to move to {title}
        </div>
      )}
    </div>
  );
};
