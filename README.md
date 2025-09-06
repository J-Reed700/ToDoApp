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
git clone https://github.com/J-Reed700/ToDoApp
cd ToDoApp
```

### Running the Backend

**Command Line:**
Navigate to the API folder and start the server:
```bash
cd api
dotnet build
dotnet run --project src/Presentation
```

**Visual Studio:**
1. Open `api/ToDoApp.sln` in Visual Studio
2. Set `Presentation` as the startup project
3. Choose the "Visual Studio" profile from the dropdown (this will open Swagger automatically)
4. Press F5 or click the "Start" button

The backend will be available at `http://localhost:5000`
API documentation is accessible at `http://localhost:5000/swagger`

### Running the Frontend

In a new terminal, navigate to the web folder and start the development server:
```bash
cd web
npm install
npm run dev
```

The frontend will be available at `http://localhost:3000`

### Usage

1. Open your browser and navigate to `http://localhost:3000`
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
- **Dev Port**: 3000 (configurable in `vite.config.ts`)

## Troubleshooting

### Common Issues

**"dotnet: command not found"**
- Install .NET 9 SDK from Microsoft's website
- Restart your terminal after installation

Check installed SDKs:
```bash
dotnet --list-sdks
```

**If dotnet is not installed at all:**

**Windows:**
```bash
# Option 1: Using winget (Windows Package Manager)
winget install Microsoft.DotNet.SDK.9

# Option 2: Using Chocolatey (if you have it)
choco install dotnet-9.0-sdk

# Option 3: Direct download
# Go to https://dotnet.microsoft.com/download/dotnet/9.0
# Download ".NET 9.0 SDK" for Windows x64
# Run the .exe installer
```

**Mac:**
```bash
# Option 1: Using Homebrew (most popular)
brew install --cask dotnet-sdk

# Option 2: Direct download (recommended for beginners)
# Go to https://dotnet.microsoft.com/download/dotnet/9.0
# Download ".NET 9.0 SDK" for macOS
# Choose "macOS x64" for Intel Macs or "macOS Arm64" for Apple Silicon (M1/M2/M3/M4)
# Run the .pkg installer

# Option 3: Using install script
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version latest --channel 9.0
# Add to your shell profile (.zshrc for zsh, .bash_profile for bash):
echo 'export PATH="$PATH:$HOME/.dotnet"' >> ~/.zshrc
source ~/.zshrc
```

If .NET 9 not installed (but older versions exist):

**Windows:**
```bash
winget install Microsoft.DotNet.SDK.9
```

**Mac:**
```bash
# Option 1: Using Homebrew (recommended)
brew install --cask dotnet-sdk

# Option 2: Direct download
# Go to https://dotnet.microsoft.com/download/dotnet/9.0
# Download .NET 9.0 SDK for macOS (choose x64 for Intel or Arm64 for Apple Silicon)

# Option 3: Using install script
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version latest --channel 9.0
export PATH="$PATH:$HOME/.dotnet"
```




**"npm: command not found"**
- Install Node.js from nodejs.org
- Restart your terminal after installation

**Frontend can't connect to backend**
- Ensure the backend is running on port 5000
- Verify the API_BASE_URL in `web/services/api.ts` is correct
- On Windows, if localhost doesn't work, try changing the API URL to `http://127.0.0.1:5000/api`
- Check Windows Firewall settings if connection issues persist

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

## Scalability Considerations

### Current Architecture Assumptions

This implementation makes several assumptions suitable for a demo/prototype environment:

- **SQLite Database**: Great for development and small deployments, but has limitations for concurrent users and data volume
- **In-Memory Storage**: Task data is stored locally without distributed caching
- **Single Instance Deployment**: No load balancing or horizontal scaling capabilities
- **Synchronous Processing**: All operations are handled immediately without background job queues
- **Basic Error Handling**: Simple error responses without comprehensive logging or monitoring

### Production Scalability Roadmap

#### Database & Data Layer
- **Migrate to PostgreSQL or SQL Server**: Replace SQLite with a production-grade RDBMS that supports connection pooling, replication, and concurrent access
- **Implement Database Indexing**: Add strategic indexes on frequently queried fields
- **Read Replicas**: Deploy read-only database replicas to distribute query load and improve read performance
- **Connection Pooling**: Use pgBouncer or similar to efficiently manage database connections
- **Data Archiving Strategy**: Automatically archive completed tasks older than X months to maintain query performance

#### API Performance & Reliability
- **Pagination**: Implement cursor-based pagination for task lists to handle large datasets efficiently
- **Rate Limiting**: Use Redis-backed rate limiting to prevent abuse and ensure fair resource usage
- **Response Caching**: Cache frequently accessed data (categories, user preferences) using Redis or ElastiCache
- **API Versioning**: Implement proper versioning strategy to support mobile apps and maintain backward compatibility
- **Bulk Operations**: Add endpoints for bulk task creation/updates to reduce API calls

#### Infrastructure & Deployment
- **Containerized Deployment**: Migrate from local hosting to containerized deployment using ECS Fargate or Kubernetes
- **Auto-Scaling**: Configure horizontal pod autoscaling based on CPU, memory, and request metrics
- **Load Balancing**: Use Application Load Balancer to distribute traffic across multiple API instances
- **CDN Integration**: Serve static assets (React build) through CloudFront or similar CDN
- **Multi-Region Deployment**: Deploy across multiple AWS regions for improved latency and disaster recovery

#### Background Processing
- **Async Job Queue**: Implement background job processing using SQS/Redis Queue for:
  - Email notifications for due tasks
  - Bulk data exports
  - Database cleanup operations
  - Third-party integrations
- **Event-Driven Architecture**: Use EventBridge or similar for decoupled communication between services

#### Monitoring & Observability
- **Application Performance Monitoring**: Integrate APM tools like DataDog or New Relic for real-time performance insights
- **Structured Logging**: Implement comprehensive logging with correlation IDs for request tracing
- **Health Checks**: Add detailed health check endpoints for load balancer integration
- **Metrics Dashboard**: Track key business and technical metrics (task completion rates, API response times, error rates)

#### Security at Scale
- **API Gateway**: Use AWS API Gateway or similar for centralized authentication, throttling, and monitoring
- **JWT Token Management**: Implement proper token refresh mechanisms and secure token storage
- **Input Validation**: Enhanced validation and sanitization to prevent injection attacks at scale
- **Audit Logging**: Track all data modifications for compliance and debugging

### Implementation Priority

1. **Phase 1 (0-1k users)**: Database migration, basic caching, pagination
2. **Phase 2 (1k-10k users)**: Auto-scaling, load balancing, monitoring
3. **Phase 3 (10k+ users)**: Read replicas, advanced caching, background jobs
4. **Phase 4 (Enterprise)**: Multi-region, microservices, advanced analytics


### Next Feature Implementations

Beyond infrastructure scaling, here are the core features I'd prioritize adding to enhance functionality:

#### Immediate Priorities (Next 1-3 Months)
- **Database Migration to PostgreSQL**: First step - move away from SQLite to support concurrent users and better query performance
- **Authentication** Implement user authentication system.
- **Response Pagination**: Implement cursor-based pagination for task lists to handle users with hundreds of tasks
- **Database Indexing**: Add indexes on frequently queried columns (user_id, category_id, status, due_date) for faster queries
- **Basic Rate Limiting**: Implement simple in-memory rate limiting to prevent API abuse (before Redis)
- **Email Notifications**: Simple email service for due date reminders and task assignments
  - Begin with direct SMTP integration (SendGrid, AWS SES)
  - Later migrate to event-driven architecture for reliability

#### Short-term Features (3-6 Months)  
- **Redis Integration**: Start with session storage and basic caching of frequently accessed data
- **User Authentication & Authorization**: Replace current basic setup with proper JWT-based auth
- **Task Due Date Reminders**: Background job to send daily/weekly email summaries
- **Bulk Task Operations**: Allow users to update multiple tasks at once (mark complete, change category, etc.)
- **Third-party Integrations**: Calendar sync (Google Calendar, Outlook), Slack notifications
- **Analytics Dashboard**: Task completion trends, productivity insights
- **API Webhooks**: Allow external systems to react to task changes
- **Custom Fields**: Let users add custom metadata to tasks
- **Recurring Tasks**: Support for daily/weekly/monthly recurring task creation
- **Time Tracking**: Basic time logging functionality for tasks

#### Medium-term Enhancements (6-12 Months)
- **Advanced Email System**: Move email sending to event bus architecture with dead letter queues for failed deliveries
- **Task Templates**: Allow users to create reusable task templates for common workflows
- **File Attachments**: Support file uploads and attachments to tasks
- **Task Dependencies**: Enable tasks to depend on completion of other tasks
- **Team Collaboration**: Share tasks and categories between users
- **Advanced Filtering**: Date ranges, priority filters, custom sorting options
- **Mobile-Responsive UI**: Enhanced mobile experience with offline capabilities

### Far in future (1-2 years)
- **Read Only Replica**: Implement Read only replica
