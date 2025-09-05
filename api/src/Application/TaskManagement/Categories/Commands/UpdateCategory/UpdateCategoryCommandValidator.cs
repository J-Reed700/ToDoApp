using ToDoApp.Application.Common.Interfaces;

namespace ToDoApp.Application.Lists.Commands.UpdateList;

public class UpdateListCommandValidator : AbstractValidator<UpdateListCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateListCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty()
            .MaximumLength(200)
            .MustAsync(BeUniqueTitle)
                .WithMessage("'{PropertyName}' must be unique.")
                .WithErrorCode("Unique");
    }

    public async Task<bool> BeUniqueTitle(UpdateListCommand model, string title, CancellationToken cancellationToken)
    {
        return !await _context.Categories
            .Where(l => l.Id != model.Id)
            .AnyAsync(l => l.CategoryName == title, cancellationToken);
    }
}
