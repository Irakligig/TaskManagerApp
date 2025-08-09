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
            //xmlserilizer requires a concrete type to know how to serialize
            var allTasks = (await GetAllTasksAsync()).ToList();

            var xs = new XmlSerializer(typeof(List<TaskItem>));

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

        public async Task ImportFromJsonAsync(string json)
        {
            var deserilizedtasks = JsonSerializer.Deserialize<IEnumerable<TaskItem>>(json);

            if (deserilizedtasks == null)
            {
                throw new ArgumentException("Invalid JSON format.");
            }

            foreach (var task in deserilizedtasks)
            {
                var existingTask = await _context.TaskItems.FindAsync(task.id);

                if (existingTask != null)
                {
                    existingTask.Title = task.Title;
                    existingTask.Description = task.Description;
                    existingTask.Priority = task.Priority;
                    existingTask.Deadline = task.Deadline;
                    existingTask.isCompleted = task.isCompleted;
                }
                else
                {
                    _context.TaskItems.Add(task);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task ImportFromXMlAsync(Stream xmlStream)
        {
            var serializer = new XmlSerializer(typeof(List<TaskItem>));

            List<TaskItem>? tasks = null;

            try
            {
                tasks = (List<TaskItem>)serializer.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Invalid XML data.", ex);
            }

            if (tasks == null) throw new ArgumentException("No tasks found in XML.");

            foreach (var task in tasks)
            {
                var existingTask = await _context.TaskItems.FindAsync(task.id);

                if (existingTask == null)
                {
                    _context.TaskItems.Add(task);
                }
                else
                {
                    existingTask.Title = task.Title;
                    existingTask.Description = task.Description;
                    existingTask.Priority = task.Priority;
                    existingTask.Deadline = task.Deadline;
                    existingTask.isCompleted = task.isCompleted;
                }
            }

            await _context.SaveChangesAsync();
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
