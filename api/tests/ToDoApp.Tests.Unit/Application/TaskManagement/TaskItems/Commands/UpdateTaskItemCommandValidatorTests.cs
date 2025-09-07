using FluentValidation.TestHelper;
using ToDoApp.Application.TaskItems.Commands.UpdateTaskItem;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.TaskItems.Commands;

[TestFixture]
public class UpdateTaskItemCommandValidatorTests
{
    private UpdateTaskItemCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new UpdateTaskItemCommandValidator();
    }

    [Test]
    public void Validator_ValidCommand_ShouldNotHaveValidationErrors()
    {
        var command = new UpdateTaskItemCommand
        {
            Id = 1,
            Title = "Valid Task Title",
            Description = "Valid description",
            CategoryId = 1,
            Priority = Priority.Medium,
            Status = Status.ToDo,
            DueDate = DateTime.Now.AddDays(7)
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_EmptyTitle_ShouldHaveValidationError()
    {
        var command = new UpdateTaskItemCommand
        {
            Id = 1,
            Title = string.Empty
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

}