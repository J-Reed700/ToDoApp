import React, { useState } from 'react';
import { CreateCategoryRequest } from '../../services/types';
import './AddCategoryForm.css';

interface AddCategoryFormProps {
    onSubmit: (list: CreateCategoryRequest) => void;
    onCancel: () => void;
}

export const AddCategoryForm: React.FC<AddCategoryFormProps> = ({ onSubmit, onCancel }) => {
  const [categoryName, setCategoryName] = useState('');

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!categoryName.trim()) return;

    onSubmit({ categoryName: categoryName.trim() });
    
    setCategoryName('');
  };

  return (
    <form className="add-category-form" onSubmit={handleSubmit}>
      <h2 className="form-title">Add Category</h2>
      
      <div className="form-row">
        <input
          type="text"
          placeholder="Category name..."
          value={categoryName}
          onChange={(e) => setCategoryName(e.target.value)}
          className="category-input"
          autoFocus
        />
        <div className="form-actions">
          <button type="button" onClick={onCancel} className="cancel-button">
            Cancel
          </button>
          <button type="submit" className="submit-button" disabled={!categoryName.trim()}>
            Add Category
          </button>
        </div>
      </div>
    </form>
  );
};