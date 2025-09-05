using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    private readonly ITaskCategoryRepository _repository;

    public CreateCategoryCommandValidator(ITaskCategoryRepository repository)
    {
        _repository = repository;

        RuleFor(v => v.CategoryName)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return !await _repository.GetAllQuery(title)
            .AnyAsync(l => l.CategoryName == title, cancellationToken);
    }
}
