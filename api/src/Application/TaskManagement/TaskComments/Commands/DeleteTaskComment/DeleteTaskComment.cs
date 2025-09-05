using ToDoApp.Application.Common.Interfaces.Data;

namespace ToDoApp.Application.TaskManagement.TaskComments.Commands.DeleteTaskComment;

public record DeleteTaskItemCommentCommand(long Id) : IRequest;


public class DeleteTaskItemCommentCommandHandler : IRequestHandler<DeleteTaskItemCommentCommand>
{
    private readonly ITaskCommentRepository _toDoTaskCommentRepository;
    public DeleteTaskItemCommentCommandHandler(ITaskCommentRepository toDoTaskCommentRepository)
    {
        _toDoTaskCommentRepository = toDoTaskCommentRepository;
    }

    public async Task Handle(DeleteTaskItemCommentCommand request, CancellationToken cancellationToken)
    {
        var entity = await _toDoTaskCommentRepository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        await _toDoTaskCommentRepository.DeleteAsync(entity, cancellationToken);
    }
}