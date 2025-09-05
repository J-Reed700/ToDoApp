using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.TaskManagement.TaskComments.Commands.CreateTaskComment;

public class CreateTaskItemCommentCommand : IRequest<TaskItemCommentDto>
{
    public long TaskId { get; init; }
    public string? Comment { get; init; }
}

public class CreateTaskItemCommentCommandHandler : IRequestHandler<CreateTaskItemCommentCommand, TaskItemCommentDto>
{
    private readonly ITaskCommentRepository _toDoTaskCommentRepository;
    public CreateTaskItemCommentCommandHandler(ITaskCommentRepository toDoTaskCommentRepository)
    {
        _toDoTaskCommentRepository = toDoTaskCommentRepository;
    }

    public async Task<TaskItemCommentDto> Handle(CreateTaskItemCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = new TaskItemComment
        {
            TaskId = request.TaskId,
            Comment = request.Comment
        };

        var createdEntity = await _toDoTaskCommentRepository.CreateAsync(entity, cancellationToken);
        return TaskItemCommentDto.MapFrom(createdEntity);
    }
}