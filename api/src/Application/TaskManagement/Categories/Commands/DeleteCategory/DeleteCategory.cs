using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.Lists.Commands.DeleteList;

public record DeleteListCommand(long Id) : IRequest;

public class DeleteListCommandHandler : IRequestHandler<DeleteListCommand>
{
    private readonly ITaskCategoryRepository _repository;

    public DeleteListCommandHandler(ITaskCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(DeleteListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        await _repository.DeleteAsync(entity, cancellationToken);
    }
}
