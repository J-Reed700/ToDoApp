namespace ToDoApp.Domain.Entities;

public class TaskComment : BaseAuditableEntity
{
    public long TaskId { get; set; }
    public string? Comment { get; set; }
    public TaskItem Task { get; set; } = null!;
}