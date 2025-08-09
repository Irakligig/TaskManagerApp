using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagerApp.Data;
using TaskManagerApp.Models;
using TaskManagerApp.Reflection;
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
            builder.Services.AddSingleton<PluginInspector>();

            //required for swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


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

            app.MapPost("/import/json",async (ITaskService taskservice,HttpRequest request) =>
            {
                using (var reader = new StreamReader(request.Body))
                {
                    var json = await reader.ReadToEndAsync();
                    try
                    {
                        await taskservice.ImportFromJsonAsync(json);
                        return Results.Ok("Tasks imported from JSON successfully.");
                    }
                    catch (ArgumentException A)
                    {
                        return Results.BadRequest(A.Message);
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(ex.Message);
                    }

                }
            });

            app.MapPost("/import/xml",async (ITaskService taskService,HttpRequest request) =>
            {
                try
                {
                    await taskService.ImportFromXMlAsync(request.Body);
                    return Results.Created("/tasks", null);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            });


            app.MapGet("/reflection/plugininfo",async (PluginInspector pluginInspector) =>
            {
                var info = pluginInspector.GetMetaInfo();
                return Results.Ok(info);
            });
            app.Run();
        }
    }
}
