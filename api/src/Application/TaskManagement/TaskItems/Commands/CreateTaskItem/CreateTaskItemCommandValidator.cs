using FluentValidation;

namespace ToDoApp.Application.TaskItems.Commands.CreateTaskItem;

public class CreateTaskItemCommandValidator : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty()
            .WithMessage("Title is required.")
            .MaximumLength(200)
            .NotEmpty();
    }
}
