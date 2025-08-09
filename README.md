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
|-- Program.cs # API endpoint definitions and dependency injection setup
|-- /Data
| |-- TaskDbContext.cs # EF Core DbContext configuration for task entities
|-- /Models
| |-- TaskItem.cs # Task entity definition
|-- /Services
| |-- ITaskService.cs # Interface defining task operations
| |-- TaskService.cs # Business logic and task CRUD implementation
|-- /BackgroundService
| |-- AutoSaveService.cs # Hosted service for automatic task saving in the background
|-- /Reflection
| |-- PluginInspector.cs # Reflection-based metadata provider for plugin inspection
| |-- ParameterDto.cs # Data transfer object for method parameters
| |-- MethodInfoDto.cs # DTO for method information
| |-- PropertyDto.cs # DTO for property information
| |-- ReflectionInfo.cs # Aggregated reflection metadata container
|-- README.md # This documentation file


---

## How to Run

1. Apply EF Core migrations and update the database:

   ```bash
   dotnet ef database update
2. dotnet run

3. https://localhost:7227/swagger

## Usage Examples

Create a Task:

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

Retrieve All Tasks:

GET /tasks

Update a Task:

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

Delete a Task:

DELETE /tasks/1

Export Tasks (JSON):

GET /export/json

Export Tasks (XML):

GET /export/xml

Import Tasks (JSON):

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

## Notes

- The background autosave service runs every 5 minutes, automatically exporting current tasks to JSON and XML files for persistence.
- The reflection endpoint provides dynamic metadata about service methods and properties, enabling introspection of the API's capabilities.
- All inputs undergo thorough validation to ensure data integrity and consistency throughout the application lifecycle.
- This project exemplifies best practices in modern .NET backend development, including clean architecture, asynchronous programming, and robust API design.


