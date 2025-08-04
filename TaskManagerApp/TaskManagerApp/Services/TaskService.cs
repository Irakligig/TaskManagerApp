using Microsoft.EntityFrameworkCore;
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
            _context.Remove(new TaskItem {id = id});
            await _context.SaveChangesAsync();
        }

        public async Task ExportToJsonAsync()
        {
            
        }

        public async Task ExportToXmlAsync()
        {
            throw new NotImplementedException();
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
            oldtask = task;
            if (oldtask == null)
            {
                throw new KeyNotFoundException($"Task with id {id} not found.");
            }
            _context.Update(oldtask);
        }
    }
}
