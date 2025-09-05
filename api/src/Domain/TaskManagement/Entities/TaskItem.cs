namespace ToDoApp.Domain.Entities;

public class TaskItem : BaseAuditableEntity
{
    public long CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Status Status { get; set; }
    public Priority Priority { get; set; }
    public DateTime? DueDate { get; set; }
    public List<TaskItemComment> Comments { get; set; } = new List<TaskItemComment>();
    public TaskCategory Category { get; set; } = null!;
}
