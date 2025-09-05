using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;
using ToDoApp.Application.TaskManagement.TaskItems;

namespace ToDoApp.Application.TaskItems.Commands.CreateTaskItem;

public record CreateTaskItemCommand : IRequest<TaskItemDto>
{
    public long CategoryId { get; init; }

    public string? Title { get; init; }

    public string? Description { get; init; }

    public Priority Priority { get; init; }

    public Status Status { get; init; }

    public DateTime? DueDate { get; init; }
}

public class CreateTaskItemCommandHandler : IRequestHandler<CreateTaskItemCommand, TaskItemDto>
{
    private readonly ITaskItemRepository _toDoTaskRepository;
    private readonly ITaskCategoryRepository _categoryRepository;

    public CreateTaskItemCommandHandler(ITaskItemRepository toDoTaskRepository, ITaskCategoryRepository categoryRepository)
    {
        _toDoTaskRepository = toDoTaskRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<TaskItemDto> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        Guard.Against.NotFound(request.CategoryId, category);

        var entity = new TaskItem
        {
            CategoryId = request.CategoryId,
            Title = request.Title ?? string.Empty,
            Description = request.Description ?? string.Empty,
            Status = request.Status,
            Priority = request.Priority,
            DueDate = request.DueDate
        };

        var createdEntity = await _toDoTaskRepository.CreateAsync(entity, cancellationToken);
        return TaskItemDto.MapFrom(createdEntity);
    }
}
