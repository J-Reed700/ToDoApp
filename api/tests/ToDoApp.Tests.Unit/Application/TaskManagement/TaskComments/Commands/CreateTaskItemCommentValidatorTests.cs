using FluentValidation.TestHelper;
using ToDoApp.Application.TaskManagement.TaskComments.Commands.CreateTaskComment;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.TaskComments.Commands;

[TestFixture]
public class CreateTaskItemCommentValidatorTests
{
    private CreateTaskItemCommentValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateTaskItemCommentValidator();
    }

    [Test]
    public void Validator_ValidCommand_ShouldNotHaveValidationErrors()
    {
        var command = new CreateTaskItemCommentCommand
        {
            TaskId = 1,
            Comment = "This is a valid comment"
        };

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_EmptyComment_ShouldHaveValidationError()
    {
        var command = new CreateTaskItemCommentCommand
        {
            TaskId = 1,
            Comment = string.Empty
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Comment);
    }

    [Test]
    public void Validator_NullComment_ShouldHaveValidationError()
    {
        var command = new CreateTaskItemCommentCommand
        {
            TaskId = 1,
            Comment = null
        };

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Comment);
    }
}