using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Application.TaskItems.Commands.CreateTaskItem;
using ToDoApp.Application.TaskManagement.TaskItems;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.TaskItems.Commands;

[TestFixture]
public class CreateTaskItemCommandHandlerTests
{
    private Mock<ITaskItemRepository> _mockTaskItemRepository;
    private Mock<ITaskCategoryRepository> _mockCategoryRepository;
    private CreateTaskItemCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockTaskItemRepository = new Mock<ITaskItemRepository>();
        _mockCategoryRepository = new Mock<ITaskCategoryRepository>();
        _handler = new CreateTaskItemCommandHandler(_mockTaskItemRepository.Object, _mockCategoryRepository.Object);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCreateTaskItemSuccessfully()
    {
        var categoryId = 1L;
        var category = new TaskCategory { Id = categoryId, CategoryName = "Work" };
        var command = new CreateTaskItemCommand
        {
            CategoryId = categoryId,
            Title = "Test Task",
            Description = "Test Description",
            Priority = Priority.Medium,
            DueDate = DateTime.Now.AddDays(7)
        };

        var createdTask = new TaskItem
        {
            Id = 1,
            CategoryId = command.CategoryId,
            Title = command.Title,
            Description = command.Description,
            Priority = command.Priority,
            DueDate = command.DueDate,
            Status = Status.ToDo
        };

        _mockCategoryRepository
            .Setup(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _mockTaskItemRepository
            .Setup(x => x.CreateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Title.ShouldBe(command.Title);
        result.Priority.ShouldBe(command.Priority);
        _mockTaskItemRepository.Verify(x => x.CreateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_NonExistentCategory_ShouldThrowNotFoundException()
    {
        var categoryId = 999L;
        var command = new CreateTaskItemCommand
        {
            CategoryId = categoryId,
            Title = "Test Task"
        };

        _mockCategoryRepository
            .Setup(x => x.GetByIdAsync(categoryId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("Category not found"));

        await Should.ThrowAsync<NotFoundException>(
            () => _handler.Handle(command, CancellationToken.None));

        _mockTaskItemRepository.Verify(x => x.CreateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}