# ToDo App Frontend

Simple, sleek React frontend for the ToDo application.

## Features

- ✅ View all tasks in a clean, modern interface
- ➕ Add new tasks with title, description, priority, and due date
- 🔄 Toggle task status (None → In Progress → Completed)
- 🗑️ Delete tasks with confirmation
- 📊 View task statistics (completed, in progress, total)
- 📱 Fully responsive design

## Setup

1. Make sure your backend API is running (typically on https://localhost:7071)
2. Update the API_BASE_URL in `services/api.ts` if needed
3. Install dependencies and start the development server

## API Endpoints Used

- `GET /api/TodoLists` - Get all lists with tasks
- `POST /api/TodoLists` - Create default list if none exists
- `POST /api/TodoTasks` - Create new task
- `PUT /api/TodoTasks/{id}` - Update task
- `DELETE /api/TodoTasks/{id}` - Delete task

## Components

- **Dashboard** - Main view with task list and controls
- **TaskItem** - Individual task with status toggle and delete
- **AddTaskForm** - Form for creating new tasks
- **API Service** - Handles all backend communication

The app automatically creates a default "My Tasks" list if none exists.
