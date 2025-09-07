import React from 'react';
import { DndContext, DragEndEvent, DragOverlay, DragStartEvent } from '@dnd-kit/core';
import { TaskItem, Status, TaskCategory } from '../../features/TaskManagement/domain/types';
import { TaskColumn } from '../../features/TaskManagement/components/TaskColumn/TaskColumn';
import { TaskCard } from '../../features/TaskManagement/components/TaskCard/TaskCard';
import { useApiService } from '../../context/apiServiceContext';
import './TaskBoard.css';

interface TaskBoardProps {
  categories: TaskCategory[];
  onTaskUpdated: (updatedTask: TaskItem) => void;
  onTaskDeleted: (taskId: number) => void;
  error: string | null;
  setError: (error: string | null) => void;
}

const statusConfig = {
  [Status.ToDo]: { title: 'To Do', color: '#6B7280' },
  [Status.InProgress]: { title: 'In Progress', color: '#3B82F6' },
  [Status.Completed]: { title: 'Done', color: '#10B981' },
  [Status.OnHold]: { title: 'On Hold', color: '#F59E0B' },
  [Status.Cancelled]: { title: 'Cancelled', color: '#EF4444' }
};

export const TaskBoard: React.FC<TaskBoardProps> = ({
  categories: lists,
  onTaskUpdated,
  onTaskDeleted,
  error,
  setError
}) => {
  const [activeTask, setActiveTask] = React.useState<TaskItem | null>(null);
  const apiService = useApiService();
  
  const allTasks = lists.flatMap(list => list.tasks || []);
  
  const tasksByStatus = Object.values(Status).reduce((acc, status) => {
    if (typeof status === 'number') {
      acc[status] = allTasks.filter(task => task.status === status);
    }
    return acc;
  }, {} as Record<Status, TaskItem[]>);

  const handleDragStart = (event: DragStartEvent) => {
    const taskId = event.active.id as number;
    const task = allTasks.find(t => t.id === taskId);
    setActiveTask(task || null);
  };

  const handleDragEnd = async (event: DragEndEvent) => {
    const { over } = event;
    
    if (!over || !activeTask) {
      setActiveTask(null);
      return;
    }

    const newStatus = parseInt(over.id as string) as Status;
    
    if (activeTask.status === newStatus) {
      setActiveTask(null);
      return;
    }

    try {
      const updatedTask = await apiService.updateTask(activeTask.id, { status: newStatus });
      onTaskUpdated(updatedTask);
      setError(null);
    } catch (err) {
      setError('Failed to update task status');
      console.error('Error updating task:', err);
    } finally {
      setActiveTask(null);
    }
  };

  return (
    <DndContext onDragStart={handleDragStart} onDragEnd={handleDragEnd}>
      <div className="task-board">
        <div className="board-header">
          <h2>Task Board</h2>
          <div className="task-stats">
            <span>{allTasks.length} tasks total</span>
          </div>
        </div>
        
        {error && (
          <div className="board-error">
            {error}
            <button onClick={() => setError(null)} className="dismiss-error">✕</button>
          </div>
        )}

        <div className="board-columns">
          {Object.entries(statusConfig).map(([statusKey, config]) => {
            const status = parseInt(statusKey) as Status;
            const tasks = tasksByStatus[status] || [];
            
            return (
              <TaskColumn
                key={status}
                status={status}
                title={config.title}
                color={config.color}
                tasks={tasks}
                onTaskUpdated={onTaskUpdated}
                onTaskDeleted={onTaskDeleted}
                availableCategories={lists}
              />
            );
          })}
        </div>

        <DragOverlay>
          {activeTask ? (
            <TaskCard
              task={activeTask}
              onTaskUpdated={onTaskUpdated}
              onTaskDeleted={onTaskDeleted}
              isDragging={true}
            />
          ) : null}
        </DragOverlay>
      </div>
    </DndContext>
  );
};
