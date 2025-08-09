# TaskManagerApp

## Overview

TaskManagerApp is a minimal API built with .NET 6+ designed to manage task items with full CRUD functionality. The project demonstrates key backend development concepts including dependency injection, asynchronous programming, data persistence with Entity Framework Core, serialization (JSON/XML), background services, and reflection. This project was developed as a learning exercise to solidify modern .NET backend skills and API design principles.

---

## Features

- **Minimal API**: Lightweight, controller-free API using .NET 6 minimal API features.
- **CRUD Endpoints**:
  - Create, read, update, delete tasks (`/tasks` endpoints).
- **Data Persistence**:
  - Uses EF Core with SQLite to store tasks persistently.
- **Serialization**:
  - Export and import tasks in both JSON and XML formats.
- **Background Service**:
  - Periodic autosave of tasks to JSON and XML files every 5 minutes using a hosted background service.
- **Reflection Module**:
  - Inspects and returns metadata about service methods and properties dynamically.
- **Validation and Error Handling**:
  - Input validation and proper HTTP status codes.
- **Swagger/OpenAPI**:
  - Interactive API documentation and testing via Swagger UI.

---

## Technologies & Concepts Learned

- **.NET 6 Minimal APIs**: Building HTTP endpoints without traditional MVC controllers.
- **Dependency Injection (DI)**: Injecting services into endpoints and background services.
- **Entity Framework Core (EF Core)**: Working with `DbContext`, migrations, and LINQ.
- **Asynchronous Programming**: Using `async`/`await` throughout the API.
- **Serialization**: JSON with `System.Text.Json` and XML with `XmlSerializer`.
- **Background Services**: Implementing `BackgroundService` for recurring tasks with proper scoped service resolution.
- **Reflection in C#**: Dynamically inspecting service classes to retrieve method and property info.
- **Error Handling**: Exception handling best practices and meaningful HTTP responses.
- **API Documentation**: Using Swagger for easy API exploration and testing.

---

## Project Structure

/TaskManagerApp
|-- Program.cs # API endpoint definitions and DI setup
|-- /Data
| |-- TaskDbContext.cs # EF Core DbContext configuration
|-- /Models
| |-- TaskItem.cs # Task entity definition
|-- /Services
| |-- ITaskService.cs # Service interface for task operations
| |-- TaskService.cs # Implementation of task CRUD, export/import logic
|-- /BackgroundService
| |-- AutoSaveService.cs # Hosted background service for autosaving tasks
|-- /Reflection
| |-- PluginInspector.cs # Reflection-based metadata provider
|-- README.md # Project documentation


---

## How to Run

1. Apply EF Core migrations and update the database:

   ```bash
   dotnet ef database update
2. dotnet run

3. https://localhost:7227/swagger

## Usage Examples

Create a task :

POST /tasks
Content-Type: application/json

{
  "id": 1,
  "title": "Sample Task",
  "description": "This is a sample task",
  "priority": 2,
  "deadline": "2025-12-31T23:59:59Z",
  "isCompleted": false
}

Get all tasks: 
GET /tasks

Update a task:

PUT /tasks/1
Content-Type: application/json

{
  "id": 1,
  "title": "Updated Task Title",
  "description": "Updated description",
  "priority": 1,
  "deadline": "2025-11-30T12:00:00Z",
  "isCompleted": true
}

Delete a task:

DELETE /tasks/1

Export tasks(json): 

GET /export/json

Export tasks(XML): 

GET /export/xml

Import tasks(json): 
POST /import/json
Content-Type: application/json

[
  {
    "id": 1,
    "title": "Task 1",
    "description": "Description 1",
    "priority": 2,
    "deadline": "2025-12-31T23:59:59Z",
    "isCompleted": false
  }
]

Notes
Background autosave service runs every 5 minutes, exporting current tasks to JSON and XML files.

Reflection endpoint provides metadata about service methods and properties for introspection.

Proper validation is implemented on all inputs to ensure data integrity.

The project is a solid demonstration of modern .NET backend development practices.

