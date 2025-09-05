using FluentValidation.TestHelper;
using ToDoApp.Application.TaskItems.Commands.CreateTaskItem;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.TaskItems.Commands;

[TestFixture]
public class CreateTaskItemCommandValidatorTests
{
    private CreateTaskItemCommandValidator _validator;

    [SetUp]
    public void SetUp()
    {
        _validator = new CreateTaskItemCommandValidator();
    }

    [Test]
    public void Validator_ValidCommand_ShouldNotHaveValidationErrors()
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = "Valid Task Title",
            Description = "Valid description",
            Priority = Priority.Medium,
            Status = Status.ToDo,
            DueDate = DateTime.Now.AddDays(7)
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Test]
    public void Validator_EmptyTitle_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = string.Empty,
            Priority = Priority.Medium,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required.");
    }

    [Test]
    public void Validator_NullTitle_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = null,
            Priority = Priority.Medium,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required.");
    }

    [Test]
    public void Validator_WhitespaceOnlyTitle_ShouldHaveValidationError()
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = "   ",
            Priority = Priority.Medium,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validator_TitleExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var longTitle = new string('a', 201); // 201 characters, exceeds max of 200
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = longTitle,
            Priority = Priority.Medium,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validator_TitleAtMaxLength_ShouldNotHaveValidationError()
    {
        // Arrange
        var maxLengthTitle = new string('a', 200); // Exactly 200 characters
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = maxLengthTitle,
            Priority = Priority.Medium,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validator_ValidTitleWithMinimumLength_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = "A", // Single character title
            Priority = Priority.Low,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Title);
    }

    [Test]
    public void Validator_NullDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = "Valid Title",
            Description = null,
            Priority = Priority.Medium,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Test]
    public void Validator_EmptyDescription_ShouldNotHaveValidationError()
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = "Valid Title",
            Description = string.Empty,
            Priority = Priority.Medium,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Theory]
    [TestCase(Priority.Low)]
    [TestCase(Priority.Medium)]
    [TestCase(Priority.High)]
    [TestCase(Priority.Critical)]
    public void Validator_ValidPriority_ShouldNotHaveValidationError(Priority priority)
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = "Valid Title",
            Priority = priority,
            Status = Status.ToDo
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Priority);
    }

    [Theory]
    [TestCase(Status.ToDo)]
    [TestCase(Status.InProgress)]
    [TestCase(Status.Completed)]
    [TestCase(Status.OnHold)]
    [TestCase(Status.Cancelled)]
    public void Validator_ValidStatus_ShouldNotHaveValidationError(Status status)
    {
        // Arrange
        var command = new CreateTaskItemCommand
        {
            CategoryId = 1,
            Title = "Valid Title",
            Priority = Priority.Medium,
            Status = status
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.Status);
    }
}
