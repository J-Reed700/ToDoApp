using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Application.TaskManagement.TaskItems;

namespace ToDoApp.Application.TaskItems.Commands.UpdateTaskItem;

public record UpdateTaskItemCommand : IRequest<TaskItemDto>
{
    public long Id { get; init; }

    public string? Title { get; init; }
    public string? Description { get; init; }

    public long CategoryId { get; init; }

    public Status Status { get; init; }

    public Priority Priority { get; init; }

    public DateTime? DueDate { get; init; }

}

public class UpdateTaskItemCommandHandler : IRequestHandler<UpdateTaskItemCommand, TaskItemDto>
{
    private readonly ITaskItemRepository _taskRepository;

    public UpdateTaskItemCommandHandler(ITaskItemRepository toDoTaskRepository)
    {
        _taskRepository = toDoTaskRepository;
    }

    public async Task<TaskItemDto> Handle(UpdateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TaskItem
        {
            Id = request.Id,
            Title = request.Title ?? string.Empty,
            Description = request.Description ?? string.Empty,
            CategoryId = request.CategoryId,
            Status = request.Status,
            Priority = request.Priority,
            DueDate = request.DueDate
        };

        var updatedEntity = await _taskRepository.UpdateAsync(entity, cancellationToken);
        return TaskItemDto.MapFrom(updatedEntity);
    }
}
