using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagerApp.Data;
using TaskManagerApp.Models;
using TaskManagerApp.Services;

namespace TaskManagerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = "Data Source=tasks.db";
            builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite(connectionString));

            builder.Services.AddScoped<ITaskService, TaskService>();
            builder.Services.AddHostedService<BackGroundService.AutoSaveService>();
            var app = builder.Build();

            app.MapGet("/tasks", async (ITaskService taskService) =>
            {
                var tasks = await taskService.GetAllTasksAsync();
                return Results.Ok(tasks);
            });

            app.MapGet("/tasks/{id}",async (int id , ITaskService taskservice) =>
            {
                var task = await taskservice.GetTaskByIdAsync(id);
                return task is not null ? Results.Ok(task) : Results.NotFound($"Task with id {id} not found.");
            });
            // this route maps incoming json to task object because post command requires json body
            app.MapPost("/tasks",async (TaskItem task , ITaskService taskservice) =>
            {
                if (task.id <= 0)
                {
                    return Results.BadRequest("Task id must be greater than 0.");
                }

                if (task.Priority < 0)
                {
                    return Results.BadRequest("Priority is incorrect");
                }

                if (task.Deadline < DateTime.UtcNow)
                {
                    return Results.BadRequest("Incorrect deadline");
                }

                if (task.isCompleted)
                {
                    return Results.BadRequest("Task cannot be completed at creation time.");
                }
                await taskservice.AddTask(task);
                return Results.Created($"/tasks/{task.id}", task);
            });

            app.MapPut("/tasks/{id}",async (int id,TaskItem updatedtask,ITaskService taskservice) =>
            {
                try
                {
                    await taskservice.UpdateTask(updatedtask, id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound($"Task with id {id} not found.");
                }
            });

            app.MapDelete("/tasks/{id}", async (int id , ITaskService taskservice) =>
            {
                try
                {
                    await taskservice.DeleteTask(id);
                    return Results.NoContent();
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound($"Task with id {id} not found.");
                }
            });

            app.MapGet("/export/json",async (ITaskService taskservice) =>
            {
                await taskservice.ExportToJsonAsync();
                return Results.Ok("Tasks exported to tasks.json");
            });

            app.MapGet("/export/xml", async (ITaskService taskservice) =>
            {
                await taskservice.ExportToXmlAsync();
                return Results.Ok("Tasks exported to tasks.xml");
            });
            app.Run();
        }
    }
}
