namespace TaskManagerApp.Models
{
    public class TaskItem
    {
        public int id { get; set; }

        public string? Title { get; set; }

        public string? Description { get; set; }

        public int Priority { get; set; }

        public DateTime Deadline { get; set; }

        public bool isCompleted { get; set; }
    }
}
