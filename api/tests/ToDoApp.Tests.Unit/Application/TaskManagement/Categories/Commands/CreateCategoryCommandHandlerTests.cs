using ToDoApp.Application.Categories.Commands.CreateCategory;
using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.Categories.Commands;

[TestFixture]
public class CreateCategoryCommandHandlerTests
{
    private Mock<ITaskCategoryRepository> _mockCategoryRepository;
    private CreateCategoryCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockCategoryRepository = new Mock<ITaskCategoryRepository>();
        _handler = new CreateCategoryCommandHandler(_mockCategoryRepository.Object);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCreateCategorySuccessfully()
    {
        var command = new CreateCategoryCommand
        {
            CategoryName = "Work Projects"
        };

        var expectedEntity = new TaskCategory
        {
            Id = 1,
            CategoryName = command.CategoryName,
            Created = DateTimeOffset.Now
        };

        _mockCategoryRepository
            .Setup(x => x.CreateAsync(It.IsAny<TaskCategory>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntity);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.CategoryName.ShouldBe(command.CategoryName);
        _mockCategoryRepository.Verify(x => x.CreateAsync(It.IsAny<TaskCategory>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_RepositoryThrowsException_ShouldPropagateException()
    {
        var command = new CreateCategoryCommand
        {
            CategoryName = "Test Category"
        };

        _mockCategoryRepository
            .Setup(x => x.CreateAsync(It.IsAny<TaskCategory>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        await Should.ThrowAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}