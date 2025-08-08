using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Xml.Serialization;
using TaskManagerApp.Data;
using TaskManagerApp.Models;

namespace TaskManagerApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly TaskDbContext _context;
        public TaskService(TaskDbContext context)
        {
            _context = context;
        }
        public async Task AddTask(TaskItem task)
        {
            _context.Add(task);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)
            {
                throw new KeyNotFoundException($"Task with id {id} not found.");
            }

            _context.TaskItems.Remove(task);

            await _context.SaveChangesAsync();
        }

        public async Task ExportToJsonAsync()
        {
            var allTasks = await GetAllTasksAsync();
            var json = JsonSerializer.Serialize(allTasks, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            await File.WriteAllTextAsync("tasks.json", json);
        }

        public async Task ExportToXmlAsync()
        {
            var allTasks = await GetAllTasksAsync();

            var xs = new XmlSerializer(typeof(IEnumerable<TaskItem>));

            using (Stream s = File.Create("tasks.xml"))
            {
                xs.Serialize(s, allTasks);
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
        {
            var taskitems = await _context.TaskItems.ToListAsync();

            return taskitems;
        }

        public async Task<TaskItem> GetTaskByIdAsync(int id)
        {
            var taskitems = await _context.TaskItems.FindAsync(id);
            return taskitems ?? throw new KeyNotFoundException($"Task with id {id} not found.");
        }

        public async Task UpdateTask(TaskItem task, int id)
        {
            var oldtask = await _context.TaskItems.FindAsync(id);

            if (oldtask == null)
            {
                throw new KeyNotFoundException($"Task with id {id} not found.");
            }

            oldtask.Title = task.Title;
            oldtask.Description = task.Description;
            oldtask.Priority = task.Priority;
            oldtask.Deadline = task.Deadline;
            oldtask.isCompleted = task.isCompleted;
            await _context.SaveChangesAsync();
        }
    }
}
