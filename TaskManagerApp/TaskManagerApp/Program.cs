using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;
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
            var app = builder.Build();
            app.Run();
        }
    }
}
