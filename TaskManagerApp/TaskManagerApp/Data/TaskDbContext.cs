using Microsoft.EntityFrameworkCore;
using TaskManagerApp.Models;

namespace TaskManagerApp.Data
{
    public class TaskDbContext : DbContext
    {
        // this constructor helps to configure dbcontext from program.cs 

        public TaskDbContext(DbContextOptions<TaskDbContext> options)
    : base(options)
        {
        }

        public DbSet<TaskItem> TaskItems { get; set; }
    }
}
