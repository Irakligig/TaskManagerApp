using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Data;

namespace TaskManagerApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<TaskDbContext>(options =>
    options.UseSqlite("Data Source=tasks.db"));

            var app = builder.Build();
            app.Run();
        }
    }
}
