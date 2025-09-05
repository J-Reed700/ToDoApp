# ToDoApp - Full Stack Task Management

A modern task management application built with React TypeScript frontend and .NET backend.

## Overview

ToDoApp is a complete task management solution that provides:

- Create and organize tasks with titles, descriptions, and due dates
- Set task priorities (Low, Medium, High, Critical)
- Track task status (None, In Progress, Completed, On Hold, Cancelled)
- Organize tasks into categories (Work, Personal, etc.)
- Drag & drop tasks between status columns
- Add comments to tasks for collaboration
- Responsive design for desktop and mobile

## Architecture

This full-stack application consists of:

- **Frontend**: React with TypeScript, Vite, and modern UI components
- **Backend**: .NET Core with Clean Architecture, FastEndpoints, and SQLite database
- **Testing**: Comprehensive unit tests for both frontend and backend

## Getting Started

### Prerequisites

Ensure you have the following installed:

- **Node.js** (version 18 or higher) - [Download](https://nodejs.org/)
- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Git** - [Download](https://git-scm.com/)

### Installation

Clone the repository:
```bash
git clone <your-repo-url>
cd ToDoApp
```

### Running the Backend

Navigate to the API folder and start the server:
```bash
cd api
dotnet build
dotnet run --project src/Presentation
```

The backend will be available at `http://localhost:5000`
API documentation is accessible at `http://localhost:5000/swagger`

### Running the Frontend

In a new terminal, navigate to the web folder and start the development server:
```bash
cd web
npm install
npm run dev
```

The frontend will be available at `http://localhost:5173`

### Usage

1. Open your browser and navigate to `http://localhost:5173`
2. Begin creating tasks and organizing them into categories

## Project Structure

```
ToDoApp/
├── api/                    # Backend (.NET Core)
│   ├── src/
│   │   ├── Domain/         # Business entities and rules
│   │   ├── Application/    # Business logic and use cases
│   │   ├── Infrastructure/ # Database and external services
│   │   └── Presentation/   # Web API endpoints
│   └── tests/              # Unit tests
│
├── web/                    # Frontend (React TypeScript)
│   ├── components/         # Shared UI components
│   ├── features/           # Feature-specific components
│   ├── services/           # API communication
│   ├── types/              # TypeScript type definitions
│   └── test/               # Unit tests
│
└── docker-compose.yml      # Docker deployment config
```

## Available Commands

### Backend Commands
```bash
# Build the project
dotnet build

# Run the application
dotnet run --project src/Presentation

# Run tests
dotnet test

# Clean build artifacts
dotnet clean
```

### Frontend Commands
```bash
# Install dependencies
npm install

# Start development server
npm run dev

# Build for production
npm run build

# Run tests
npm test

# Run tests with UI
npm run test:ui

# Lint code
npm run lint

# Format code
npm run format
```

## Running Tests

### Backend Tests
```bash
cd api
dotnet test
```
Runs 109 unit tests covering domain logic, business rules, and data operations.

### Frontend Tests  
```bash
cd web
npm test
```
Runs 35 unit tests covering components, services, and user interactions.

## Docker Deployment

To run the entire application with Docker:

```bash
# Build and start both frontend and backend
docker-compose up --build

# Access the app at http://localhost:3000
```

## Configuration

### Backend Configuration
- **Database**: SQLite (automatically created)
- **API Port**: 5000 (configurable in `appsettings.json`)
- **CORS**: Configured for frontend origins

### Frontend Configuration
- **API Base URL**: `http://localhost:5000/api` (configurable in `services/api.ts`)
- **Dev Port**: 5173 (configurable in `vite.config.ts`)

## Troubleshooting

### Common Issues

**"dotnet: command not found"**
- Install .NET 9 SDK from Microsoft's website
- Restart your terminal after installation

**"npm: command not found"**
- Install Node.js from nodejs.org
- Restart your terminal after installation

**Frontend can't connect to backend**
- Ensure the backend is running on port 5000
- Verify the API_BASE_URL in `web/services/api.ts` is correct

**Database errors**
- Delete the database file and restart the backend
- The application will automatically create a new database

### Getting Help

1. Check the console for error messages from both frontend and backend
2. Use browser developer tools (F12) to view frontend errors
3. Visit the API documentation at `http://localhost:5000/swagger` when the backend is running

## Next Steps

Once you have the application running:

1. Create your first category (e.g., "Work Tasks")
2. Add tasks with different priorities
3. Use drag and drop to move tasks between status columns
4. Add comments to tasks
5. Explore the API documentation at `/swagger`

## Contributing

Contributions are welcome:
- Add new features
- Fix bugs
- Improve documentation
- Add more tests

The codebase follows clean architecture principles and modern development practices.
