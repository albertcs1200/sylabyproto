namespace protipo_sprint_tareas.Models;

public enum TaskStatus
{
    ToDo,
    InProgress,
    Testing,
    Done
}

public enum TaskPriority
{
    Baja,
    Media,
    Alta,
    Critica
}

public class SprintTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TaskStatus Status { get; set; }
    public TaskPriority Priority { get; set; }
    public string Assignee { get; set; } = string.Empty;
    public int StoryPoints { get; set; }
    public DateTime DueDate { get; set; }
}
