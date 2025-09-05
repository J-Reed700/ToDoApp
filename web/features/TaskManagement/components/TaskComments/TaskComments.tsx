import React, { useState, useEffect } from 'react';
import { TaskComment } from '../../domain/types';
import { CreateCommentRequest } from '../../services/types';
import { useApiService } from '../../../../context/apiServiceContext';
import './TaskComments.css';

interface TaskCommentsProps {
  taskId: number;
}

export const TaskComments: React.FC<TaskCommentsProps> = ({ taskId }) => {
  const [comments, setComments] = useState<TaskComment[]>([]);
  const [newComment, setNewComment] = useState('');
  const [editingComment, setEditingComment] = useState<number | null>(null);
  const [editText, setEditText] = useState('');
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const apiService = useApiService();

  useEffect(() => {
    loadComments();
  }, [taskId]);

  const loadComments = async () => {
    setIsLoading(true);
    try {
      const taskComments = await apiService.getTaskComments(taskId);
      setComments(taskComments);
      setError(null);
    } catch (err) {
      setError('Failed to load comments');
      console.error('Error loading comments:', err);
    } finally {
      setIsLoading(false);
    }
  };

  const handleAddComment = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newComment.trim()) return;

    try {
      const commentRequest: CreateCommentRequest = {
        taskId,
        comment: newComment.trim(),
      };
      
      const comment = await apiService.createComment(commentRequest);
      setNewComment('');
      setComments(comments.concat(comment));
      setError(null);
    } catch (err) {
      setError('Failed to add comment');
      console.error('Error creating comment:', err);
    }
  };

  const handleEditComment = async (commentId: number) => {
    if (!editText.trim()) return;

    try {
      await apiService.updateComment(commentId, {
        id: commentId,
        comment: editText.trim(),
      });
      setEditingComment(null);
      setEditText('');
      setComments(comments.map(comment => comment.id === commentId ? { ...comment, comment: editText.trim() } : comment));
      setError(null);
    } catch (err) {
      setError('Failed to update comment');
      console.error('Error updating comment:', err);
    }
  };

  const handleDeleteComment = async (commentId: number) => {
    try {
      await apiService.deleteComment(commentId);
      setComments(comments.filter(comment => comment.id !== commentId));
      setError(null);
    } catch (err) {
      setError('Failed to delete comment');
      console.error('Error deleting comment:', err);
    }
  };

  const startEdit = (comment: TaskComment) => {
    setEditingComment(comment.id);
    setEditText(comment.comment);
  };

  const cancelEdit = () => {
    setEditingComment(null);
    setEditText('');
  };

  if (isLoading) {
    return <div className="comments-loading">Loading comments...</div>;
  }

  return (
    <div className="task-comments">
      <h4 className="comments-title">Comments ({comments.length})</h4>
      
      {error && (
        <div className="comments-error">
          {error}
          <button onClick={() => setError(null)} className="dismiss-error">✕</button>
        </div>
      )}

      {/* Add new comment form */}
      <form onSubmit={handleAddComment} className="add-comment-form">
        <textarea
          value={newComment}
          onChange={(e) => setNewComment(e.target.value)}
          placeholder="Add a comment..."
          className="comment-input"
          rows={3}
        />
        <button 
          type="submit" 
          disabled={!newComment.trim()}
          className="add-comment-button"
        >
          Add Comment
        </button>
      </form>

      <div className="comments-list">
        {comments.length === 0 ? (
          <p className="no-comments">No comments yet. Be the first to comment!</p>
        ) : (
          comments.map((comment) => (
            <div key={comment.id} className="comment-item">
              {editingComment === comment.id ? (
                // Edit mode
                <div className="comment-edit">
                  <textarea
                    value={editText}
                    onChange={(e) => setEditText(e.target.value)}
                    className="comment-edit-input"
                    rows={3}
                  />
                  <div className="comment-edit-actions">
                    <button 
                      onClick={() => handleEditComment(comment.id)}
                      className="save-comment-button"
                      disabled={!editText.trim()}
                    >
                      Save
                    </button>
                    <button 
                      onClick={cancelEdit}
                      className="cancel-comment-button"
                    >
                      Cancel
                    </button>
                  </div>
                </div>
              ) : (
                <div className="comment-content">
                  <p className="comment-text">{comment.comment}</p>
                  <div className="comment-meta">
                    {comment.created && (
                      <span className="comment-date">
                        {new Date(comment.created).toLocaleDateString()}
                      </span>
                    )}
                    {comment.createdBy && (
                      <span className="comment-author">by {comment.createdBy}</span>
                    )}
                  </div>
                  <div className="comment-actions">
                    <button 
                      onClick={() => startEdit(comment)}
                      className="edit-comment-button"
                      title="Edit comment"
                    >
                      ✏️
                    </button>
                    <button 
                      onClick={() => handleDeleteComment(comment.id)}
                      className="delete-comment-button"
                      title="Delete comment"
                    >
                      🗑️
                    </button>
                  </div>
                </div>
              )}
            </div>
          ))
        )}
      </div>
    </div>
  );
};
