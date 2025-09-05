using ToDoApp.Domain.Enums;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.TaskManagement.TaskItems;

public class TaskItemDto
{
    public long Id { get; init; }

    public long CategoryId { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public Status Status { get; init; }

    public Priority Priority { get; init; }

    public DateTime? DueDate { get; init; }

    public static TaskItemDto MapFrom(TaskItem taskItem)
    {
        return new TaskItemDto
        {
            Id = taskItem.Id,
            CategoryId = taskItem.CategoryId,
            Title = taskItem.Title,
            Description = taskItem.Description,
            Status = taskItem.Status,
            Priority = taskItem.Priority,
            DueDate = taskItem.DueDate
        };
    }
}