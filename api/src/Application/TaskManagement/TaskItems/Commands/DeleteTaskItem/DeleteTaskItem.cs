using ToDoApp.Application.Common.Interfaces.Data;

namespace ToDoApp.Application.TaskItems.Commands.DeleteTaskItem;

public record DeleteTaskItemCommand(long Id) : IRequest;

public class DeleteTaskItemCommandHandler : IRequestHandler<DeleteTaskItemCommand>
{
    private readonly ITaskItemRepository _toDoTaskRepository;

    public DeleteTaskItemCommandHandler(ITaskItemRepository toDoTaskRepository)
    {
        _toDoTaskRepository = toDoTaskRepository;
    }

    public async Task Handle(DeleteTaskItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _toDoTaskRepository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        await _toDoTaskRepository.DeleteAsync(entity, cancellationToken);
    }

}
