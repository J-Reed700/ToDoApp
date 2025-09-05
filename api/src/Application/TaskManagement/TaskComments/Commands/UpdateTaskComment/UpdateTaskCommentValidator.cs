using FluentValidation;

namespace ToDoApp.Application.TaskManagement.TaskComments.Commands.UpdateTaskComment;

public class UpdateTaskItemCommentValidator : AbstractValidator<UpdateTaskItemCommentCommand>
{
    public UpdateTaskItemCommentValidator()
    {
        RuleFor(v => v.Comment)
            .NotEmpty()
            .WithMessage("Comment is required.");
    }
}