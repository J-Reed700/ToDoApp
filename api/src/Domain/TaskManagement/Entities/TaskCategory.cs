namespace ToDoApp.Domain.Entities;

public class TaskCategory : BaseAuditableEntity
{
    public string? CategoryName { get; set; }
    public IList<TaskItem> Tasks { get; private set; } = new List<TaskItem>();
}
