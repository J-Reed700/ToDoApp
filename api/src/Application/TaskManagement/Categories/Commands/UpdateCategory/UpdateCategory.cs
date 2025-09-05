using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Lists.Commands.UpdateList;

public record UpdateListCommand : IRequest
{
    public long Id { get; init; }

    public string? Title { get; init; }
}

public class UpdateListCommandHandler : IRequestHandler<UpdateListCommand>
{
    private readonly ITaskCategoryRepository _repository;

    public UpdateListCommandHandler(ITaskCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(UpdateListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TaskCategory
        {
            Id = request.Id,
            CategoryName = request.Title
        };

        await _repository.UpdateAsync(entity, cancellationToken);

    }
}
