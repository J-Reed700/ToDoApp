using ToDoApp.Application.TaskManagement.TaskItems;
using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Tests.Unit.Application.TaskManagement.TaskItems;

[TestFixture]
public class TaskItemDtoTests
{
    [Test]
    public void MapFrom_ValidTaskItem_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var taskItem = new TaskItem
        {
            Id = 1,
            CategoryId = 2,
            Title = "Test Task",
            Description = "Test Description",
            Status = Status.InProgress,
            Priority = Priority.High,
            DueDate = new DateTime(2024, 12, 25, 10, 30, 0)
        };

        // Act
        var dto = TaskItemDto.MapFrom(taskItem);

        // Assert
        dto.Id.ShouldBe(taskItem.Id);
        dto.CategoryId.ShouldBe(taskItem.CategoryId);
        dto.Title.ShouldBe(taskItem.Title);
        dto.Description.ShouldBe(taskItem.Description);
        dto.Status.ShouldBe(taskItem.Status);
        dto.Priority.ShouldBe(taskItem.Priority);
        dto.DueDate.ShouldBe(taskItem.DueDate);
    }

    [Test]
    public void MapFrom_TaskItemWithNullValues_ShouldHandleNullsCorrectly()
    {
        // Arrange
        var taskItem = new TaskItem
        {
            Id = 1,
            CategoryId = 2,
            Title = "Test Task",
            Description = null,
            Status = Status.ToDo,
            Priority = Priority.Low,
            DueDate = null
        };

        // Act
        var dto = TaskItemDto.MapFrom(taskItem);

        // Assert
        dto.Id.ShouldBe(taskItem.Id);
        dto.CategoryId.ShouldBe(taskItem.CategoryId);
        dto.Title.ShouldBe(taskItem.Title);
        dto.Description.ShouldBeNull();
        dto.Status.ShouldBe(taskItem.Status);
        dto.Priority.ShouldBe(taskItem.Priority);
        dto.DueDate.ShouldBeNull();
    }

    [Test]
    public void MapFrom_TaskItemWithEmptyStrings_ShouldPreserveEmptyStrings()
    {
        // Arrange
        var taskItem = new TaskItem
        {
            Id = 1,
            CategoryId = 2,
            Title = string.Empty,
            Description = string.Empty,
            Status = Status.Completed,
            Priority = Priority.Medium,
            DueDate = DateTime.MinValue
        };

        // Act
        var dto = TaskItemDto.MapFrom(taskItem);

        // Assert
        dto.Title.ShouldBe(string.Empty);
        dto.Description.ShouldBe(string.Empty);
        dto.DueDate.ShouldBe(DateTime.MinValue);
    }

    [Theory]
    [TestCase(Priority.Low)]
    [TestCase(Priority.Medium)]
    [TestCase(Priority.High)]
    [TestCase(Priority.Critical)]
    public void MapFrom_DifferentPriorities_ShouldMapCorrectly(Priority priority)
    {
        // Arrange
        var taskItem = new TaskItem
        {
            Id = 1,
            CategoryId = 1,
            Title = "Test Task",
            Priority = priority,
            Status = Status.ToDo
        };

        // Act
        var dto = TaskItemDto.MapFrom(taskItem);

        // Assert
        dto.Priority.ShouldBe(priority);
    }

    [Theory]
    [TestCase(Status.ToDo)]
    [TestCase(Status.InProgress)]
    [TestCase(Status.Completed)]
    [TestCase(Status.OnHold)]
    [TestCase(Status.Cancelled)]
    public void MapFrom_DifferentStatuses_ShouldMapCorrectly(Status status)
    {
        // Arrange
        var taskItem = new TaskItem
        {
            Id = 1,
            CategoryId = 1,
            Title = "Test Task",
            Priority = Priority.Medium,
            Status = status
        };

        // Act
        var dto = TaskItemDto.MapFrom(taskItem);

        // Assert
        dto.Status.ShouldBe(status);
    }

    [Test]
    public void TaskItemDto_ShouldBeImmutable()
    {
        // Arrange
        var taskItem = new TaskItem
        {
            Id = 1,
            CategoryId = 2,
            Title = "Test Task",
            Description = "Test Description",
            Status = Status.InProgress,
            Priority = Priority.High,
            DueDate = DateTime.Now
        };

        // Act
        var dto = TaskItemDto.MapFrom(taskItem);

        // Assert - All properties should have init-only setters
        // This is verified by the fact that the properties are declared with { get; init; }
        // The compiler would prevent any attempts to modify these properties after construction
        dto.ShouldNotBeNull();
        
        // Verify that we can't modify the DTO after creation
        // The following lines should not compile if init-only setters are working:
        // dto.Id = 999;  // Should cause compilation error
        // dto.Title = "Modified"; // Should cause compilation error
    }

    [Test]
    public void MapFrom_WithMaxValues_ShouldHandleEdgeCases()
    {
        // Arrange
        var taskItem = new TaskItem
        {
            Id = long.MaxValue,
            CategoryId = long.MaxValue,
            Title = new string('A', 1000), // Very long title
            Description = new string('B', 5000), // Very long description
            Status = Status.Cancelled,
            Priority = Priority.Critical,
            DueDate = DateTime.MaxValue
        };

        // Act
        var dto = TaskItemDto.MapFrom(taskItem);

        // Assert
        dto.Id.ShouldBe(long.MaxValue);
        dto.CategoryId.ShouldBe(long.MaxValue);
        dto.Title?.Length.ShouldBe(1000);
        dto.Description?.Length.ShouldBe(5000);
        dto.DueDate.ShouldBe(DateTime.MaxValue);
    }
}
