using FluentValidation;

namespace ToDoApp.Application.TaskManagement.TaskComments.Commands.CreateTaskComment;

public class CreateTaskItemCommentValidator : AbstractValidator<CreateTaskItemCommentCommand>
{
    public CreateTaskItemCommentValidator()
    {
        RuleFor(v => v.Comment)
            .NotEmpty()
            .WithMessage("Comment is required.");
    }
}
