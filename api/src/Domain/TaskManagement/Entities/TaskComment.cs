namespace ToDoApp.Domain.Entities;

public class TaskItemComment : BaseAuditableEntity
{
    public long TaskId { get; set; }
    public string? Comment { get; set; }
    public TaskItem Task { get; set; } = null!;
}