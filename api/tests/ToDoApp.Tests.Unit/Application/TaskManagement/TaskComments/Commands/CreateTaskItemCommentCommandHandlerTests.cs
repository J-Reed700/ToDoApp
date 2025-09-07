using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Application.TaskManagement.TaskComments.Commands.CreateTaskComment;
using TaskItemCommentDto = ToDoApp.Application.TaskManagement.TaskComments.TaskItemCommentDto;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.TaskComments.Commands;

[TestFixture]
public class CreateTaskItemCommentCommandHandlerTests
{
    private Mock<ITaskCommentRepository> _mockRepository;
    private CreateTaskItemCommentCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _mockRepository = new Mock<ITaskCommentRepository>();
        _handler = new CreateTaskItemCommentCommandHandler(_mockRepository.Object);
    }

    [Test]
    public async Task Handle_ValidCommand_ShouldCreateCommentSuccessfully()
    {
        var command = new CreateTaskItemCommentCommand
        {
            TaskId = 1,
            Comment = "This is a test comment"
        };

        var expectedEntity = new TaskItemComment
        {
            Id = 1,
            TaskId = command.TaskId,
            Comment = command.Comment,
            Created = DateTimeOffset.Now
        };

        _mockRepository
            .Setup(x => x.CreateAsync(It.IsAny<TaskItemComment>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedEntity);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.ShouldNotBeNull();
        result.Comment.ShouldBe(command.Comment);
        _mockRepository.Verify(x => x.CreateAsync(It.IsAny<TaskItemComment>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Handle_RepositoryThrowsException_ShouldPropagateException()
    {
        var command = new CreateTaskItemCommentCommand
        {
            TaskId = 1,
            Comment = "Test comment"
        };

        _mockRepository
            .Setup(x => x.CreateAsync(It.IsAny<TaskItemComment>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException("Database error"));

        await Should.ThrowAsync<InvalidOperationException>(
            () => _handler.Handle(command, CancellationToken.None));
    }
}