using ToDoApp.Application.Common.Interfaces;
using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Categories.Commands.CreateCategory;

public record CreateCategoryCommand : IRequest<TaskCategory>
{
    public string CategoryName { get; init; } = string.Empty;
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, TaskCategory>
{
    private readonly ITaskCategoryRepository _repository;

    public CreateCategoryCommandHandler(ITaskCategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskCategory> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = new TaskCategory
        {
            CategoryName = request.CategoryName,
        };

        return await _repository.CreateAsync(entity, cancellationToken);
    }
}
