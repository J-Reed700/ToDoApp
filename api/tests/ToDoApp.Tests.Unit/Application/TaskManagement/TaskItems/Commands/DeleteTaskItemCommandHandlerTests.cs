using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Application.TaskItems.Commands.DeleteTaskItem;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.TaskItems.Commands;

[TestFixture]
public class DeleteTaskItemCommandHandlerTests
{
    private Mock<ITaskItemRepository> _mockTaskItemRepository;
    private DeleteTaskItemCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockTaskItemRepository = new Mock<ITaskItemRepository>();
        _handler = new DeleteTaskItemCommandHandler(_mockTaskItemRepository.Object);
    }

    [Test]
    public async Task Handle_ValidId_ShouldDeleteTaskItemSuccessfully()
    {
        var taskId = 1L;
        var existingTask = new TaskItem
        {
            Id = taskId,
            Title = "Task to Delete",
            CategoryId = 1,
            Status = Status.ToDo
        };

        var command = new DeleteTaskItemCommand(taskId);

        _mockTaskItemRepository
            .Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);

        _mockTaskItemRepository
            .Setup(x => x.DeleteAsync(existingTask, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        await _handler.Handle(command, CancellationToken.None);

        _mockTaskItemRepository.Verify(x => x.DeleteAsync(existingTask, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_NonExistentId_ShouldThrowNotFoundException()
    {
        var taskId = 999L;
        var command = new DeleteTaskItemCommand(taskId);

        _mockTaskItemRepository
            .Setup(x => x.GetByIdAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((TaskItem)null!);

        var exception = await Should.ThrowAsync<Ardalis.GuardClauses.NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        exception.Message.ShouldContain("999");
        _mockTaskItemRepository.Verify(x => x.DeleteAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}