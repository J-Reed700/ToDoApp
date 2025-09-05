using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Domain.Entities;
using ToDoApp.Application.TaskManagement.TaskComments;

namespace ToDoApp.Application.TaskManagement.TaskComments.Commands.UpdateTaskComment;

public record UpdateTaskItemCommentCommand(long Id, string Comment) : IRequest<TaskItemCommentDto>;

public class UpdateTaskItemCommentCommandHandler : IRequestHandler<UpdateTaskItemCommentCommand, TaskItemCommentDto>
{
    private readonly ITaskCommentRepository _toDoTaskCommentRepository;
    public UpdateTaskItemCommentCommandHandler(ITaskCommentRepository toDoTaskCommentRepository)
    {
        _toDoTaskCommentRepository = toDoTaskCommentRepository;
    }

    public async Task<TaskItemCommentDto> Handle(UpdateTaskItemCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = new TaskItemComment
        {
            Id = request.Id,
            Comment = request.Comment
        };

        var updatedEntity = await _toDoTaskCommentRepository.UpdateAsync(entity, cancellationToken);
        return TaskItemCommentDto.MapFrom(updatedEntity);
    }
}