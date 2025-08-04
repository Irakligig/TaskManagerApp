using TaskManagerApp.Models;

namespace TaskManagerApp.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> GetAllTasksAsync();
        Task<TaskItem> GetTaskByIdAsync(int id);
        Task AddTask(TaskItem task);
        Task UpdateTask(TaskItem task, int id);
        Task DeleteTask(int id);
        Task ExportToJsonAsync();
        Task ExportToXmlAsync();

    }
}
