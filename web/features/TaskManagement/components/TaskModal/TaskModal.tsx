import { TaskItem, TaskCategory } from "../../domain/types";
import { Dialog, DialogPanel, DialogTitle } from '@headlessui/react';
import { Status, Priority } from "../../domain/types";
import { useState, useEffect } from 'react';
import { toDateInputValue, formatDisplayDate, toDate } from '../../utils/dateUtils';
import { useApiService } from '../../../../context/apiServiceContext';
import { TaskComments } from '../TaskComments/TaskComments';
import './TaskModal.css';

interface TaskModalProps {
    task: TaskItem;
    showTaskModal: boolean;
    setShowTaskModal: (showTaskModal: boolean) => void;
    onTaskUpdated?: (updatedTask: TaskItem) => void;
    availableCategories?: TaskCategory[];
}

export const TaskModal: React.FC<TaskModalProps> = ({ task, showTaskModal, setShowTaskModal, onTaskUpdated, availableCategories = [] }) => {
    const [isEditing, setIsEditing] = useState(false);
    const [categories, setCategories] = useState<TaskCategory[]>(availableCategories);
    const [formData, setFormData] = useState({
        title: task.title,
        description: task.description || '',
        status: task.status,
        priority: task.priority,
        categoryId: task.categoryId,
        dueDate: toDateInputValue(task.dueDate)
    });
    const [isSaving, setIsSaving] = useState(false);
    const [error, setError] = useState<string | null>(null);

    const apiService = useApiService();

    useEffect(() => {
        if (availableCategories.length === 0) {
            // Load categories if not provided
            const loadCategories = async () => {
                try {
                    const cats = await apiService.getTaskLists();
                    setCategories(cats);
                } catch (err) {
                    console.error('Failed to load categories:', err);
                }
            };
            loadCategories();
        }
    }, [availableCategories, apiService]);

    const handleInputChange = (field: string, value: string | number) => {
        setFormData(prev => ({
            ...prev,
            [field]: value
        }));
        setError(null);
    };


    const handleSave = async () => {
        if (!formData.title.trim()) {
            setError('Title is required');
            return;
        }

        setIsSaving(true);
        setError(null);

        try {
            
            const updates: Partial<TaskItem> = {
                title: formData.title.trim(),
                description: formData.description.trim(),
                status: formData.status,
                priority: formData.priority,
                categoryId: formData.categoryId,
                dueDate: formData.dueDate ? toDate(formData.dueDate) : toDate(task.dueDate)
            };
            
            

            const updatedTask = await apiService.updateTask(task.id, updates);
            
            
            if (onTaskUpdated) {
                onTaskUpdated(updatedTask);
            }
            
            setIsEditing(false);
            setShowTaskModal(false);

        } catch (err) {
            setError('Failed to save task. Please try again.');
            console.error('Error updating task:', err);
        } finally {
            setIsSaving(false);
        }
    };

    const handleCancel = () => {
        setFormData({
            title: task.title,
            description: task.description || '',
            status: task.status,
            priority: task.priority,
            categoryId: task.categoryId,
            dueDate: toDateInputValue(task.dueDate)
        });
        setIsEditing(false);
        setError(null);
    };


    return (
        <Dialog open={showTaskModal} onClose={() => setShowTaskModal(false)}>
            <div className="modal-backdrop" aria-hidden="true" />
            
            <div className="modal-container">
                <DialogPanel className="modal-panel">
                    <DialogTitle className="modal-title">
                        {isEditing ? 'Edit Task' : 'Task Details'}
                    </DialogTitle>
                    
                    {error && (
                        <div className="modal-error">
                            {error}
                        </div>
                    )}
                    
                    <div className="modal-content">
                        <div className="modal-field">
                            <h4 className="modal-field-label">Title:</h4>
                            {isEditing ? (
                                <input
                                    type="text"
                                    value={formData.title}
                                    onChange={(e) => handleInputChange('title', e.target.value)}
                                    className="modal-input"
                                    placeholder="Enter task title"
                                />
                            ) : (
                                <p className="modal-field-value">{task.title}</p>
                            )}
                        </div>
                        
                        <div className="modal-field">
                            <h4 className="modal-field-label">Description:</h4>
                            {isEditing ? (
                                <textarea
                                    value={formData.description}
                                    onChange={(e) => handleInputChange('description', e.target.value)}
                                    className="modal-textarea"
                                    placeholder="Enter task description"
                                    rows={3}
                                />
                            ) : (
                                <p className="modal-field-value" style={{ textTransform: 'none' }}>
                                    {task.description || 'No description'}
                                </p>
                            )}
                        </div>

                        <div className="modal-field">
                            <h4 className="modal-field-label">Category:</h4>
                            {isEditing ? (
                                <select
                                    value={formData.categoryId}
                                    onChange={(e) => handleInputChange('categoryId', parseInt(e.target.value))}
                                    className="modal-select"
                                >
                                    {categories.map((category) => (
                                        <option key={category.id} value={category.id}>
                                            {category.categoryName}
                                        </option>
                                    ))}
                                </select>
                            ) : (
                                <p className="modal-field-value">
                                    {categories.find(c => c.id === task.categoryId)?.categoryName || 'No category'}
                                </p>
                            )}
                        </div>
                        
                        <div className="modal-field">
                            <h4 className="modal-field-label">Status:</h4>
                            {isEditing ? (
                                <select
                                    value={formData.status}
                                    onChange={(e) => handleInputChange('status', parseInt(e.target.value))}
                                    className="modal-select"
                                >
                                    {Object.entries(Status).filter(([key]) => isNaN(Number(key))).map(([key, value]) => (
                                        <option key={value} value={value}>
                                            {key}
                                        </option>
                                    ))}
                                </select>
                            ) : (
                                <p className="modal-field-value">{Status[task.status]}</p>
                            )}
                        </div>
                        
                        <div className="modal-field">
                            <h4 className="modal-field-label">Priority:</h4>
                            {isEditing ? (
                                <select
                                    value={formData.priority}
                                    onChange={(e) => handleInputChange('priority', parseInt(e.target.value))}
                                    className="modal-select"
                                >
                                    {Object.entries(Priority).filter(([key]) => isNaN(Number(key))).map(([key, value]) => (
                                        <option key={value} value={value}>
                                            {key}
                                        </option>
                                    ))}
                                </select>
                            ) : (
                                <p className="modal-field-value">{Priority[task.priority]}</p>
                            )}
                        </div>
                        
                        <div className="modal-field">
                            <h4 className="modal-field-label">Due Date:</h4>
                            {isEditing ? (
                                <input
                                    type="date"
                                    value={formData.dueDate}
                                    onChange={(e) => handleInputChange('dueDate', e.target.value)}
                                    className="modal-input"
                                />
                            ) : (
                                <p className="modal-field-value" style={{ textTransform: 'none' }}>
                                    {formatDisplayDate(task.dueDate)}
                                </p>
                            )}
                        </div>
                    </div>
                    
                                    
                    <div className="modal-actions">
                        {isEditing ? (
                            <>
                                <button 
                                    onClick={handleSave}
                                    disabled={isSaving}
                                    className="modal-save-button"
                                >
                                    {isSaving ? 'Saving...' : 'Save'}
                                </button>
                                <button 
                                    onClick={handleCancel}
                                    disabled={isSaving}
                                    className="modal-cancel-button"
                                >
                                    Cancel
                                </button>
                            </>
                        ) : (
                            <>
                                <button 
                                    onClick={() => setIsEditing(true)}
                                    className="modal-edit-button"
                                >
                                    Edit
                                </button>
                                <button 
                                    onClick={() => setShowTaskModal(false)}
                                    className="modal-close-button"
                                >
                                    Close
                                </button>
                            </>
                        )}
                    </div>

                    {!isEditing && <TaskComments taskId={task.id} />}

                </DialogPanel>
            </div>
        </Dialog>
    );
};