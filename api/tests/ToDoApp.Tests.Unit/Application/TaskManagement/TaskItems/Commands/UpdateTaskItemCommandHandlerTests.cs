using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Application.TaskItems.Commands.UpdateTaskItem;
using ToDoApp.Application.TaskManagement.TaskItems;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.TaskItems.Commands;

[TestFixture]
public class UpdateTaskItemCommandHandlerTests
{
    private Mock<ITaskItemRepository> _mockTaskItemRepository;
    private UpdateTaskItemCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockTaskItemRepository = new Mock<ITaskItemRepository>();
        _handler = new UpdateTaskItemCommandHandler(_mockTaskItemRepository.Object);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldUpdateTaskItemSuccessfully()
    {
        var command = new UpdateTaskItemCommand
        {
            Id = 1,
            Title = "Updated Task",
            Description = "Updated Description",
            CategoryId = 2,
            Priority = Priority.High,
            Status = Status.InProgress,
            DueDate = DateTime.Now.AddDays(5)
        };

        var existingTask = new TaskItem
        {
            Id = 1,
            Title = "Original Task",
            CategoryId = 1,
            Priority = Priority.Low,
            Status = Status.ToDo
        };

        var updatedTask = new TaskItem
        {
            Id = 1,
            Title = command.Title,
            Description = command.Description,
            CategoryId = command.CategoryId,
            Priority = command.Priority,
            Status = command.Status,
            DueDate = command.DueDate
        };

        _mockTaskItemRepository
            .Setup(x => x.GetByIdAsync(1L, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);

        _mockTaskItemRepository
            .Setup(x => x.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(updatedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(command.Title);
        result.Priority.ShouldBe(command.Priority);
        _mockTaskItemRepository.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_UpdateAsyncThrowsException_ShouldPropagateException()
    {
        var command = new UpdateTaskItemCommand { Id = 999, Title = "Test" };

        _mockTaskItemRepository
            .Setup(x => x.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        await Should.ThrowAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));

        _mockTaskItemRepository.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}

