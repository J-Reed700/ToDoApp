using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Tests.Unit.Domain.TaskManagement.Entities;

[TestFixture]
public class TaskItemTests
{
    [Test]
    public void TaskItem_WhenCreated_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var taskItem = new TaskItem();

        // Assert
        taskItem.Id.ShouldBe(0);
        taskItem.Title.ShouldBe(string.Empty);
        taskItem.Description.ShouldBeNull();
        taskItem.Status.ShouldBe(default(ToDoApp.Domain.Enums.Status));
        taskItem.Priority.ShouldBe(default(ToDoApp.Domain.Enums.Priority));
        taskItem.DueDate.ShouldBeNull();
        taskItem.Comments.ShouldNotBeNull();
        taskItem.Comments.ShouldBeEmpty();
    }

    [Test]
    public void TaskItem_WhenCreatedWithValues_ShouldSetPropertiesCorrectly()
    {
        // Arrange
        var categoryId = 1L;
        var title = "Test Task";
        var description = "Test Description";
        var status = Status.InProgress;
        var priority = Priority.High;
        var dueDate = DateTime.Now.AddDays(7);

        // Act
        var taskItem = new TaskItem
        {
            CategoryId = categoryId,
            Title = title,
            Description = description,
            Status = status,
            Priority = priority,
            DueDate = dueDate
        };

        // Assert
        taskItem.CategoryId.ShouldBe(categoryId);
        taskItem.Title.ShouldBe(title);
        taskItem.Description.ShouldBe(description);
        taskItem.Status.ShouldBe(status);
        taskItem.Priority.ShouldBe(priority);
        taskItem.DueDate.ShouldBe(dueDate);
    }

    [Test]
    public void TaskItem_Comments_ShouldBeInitializedAsEmptyList()
    {
        // Arrange & Act
        var taskItem = new TaskItem();

        // Assert
        taskItem.Comments.ShouldNotBeNull();
        taskItem.Comments.ShouldBeOfType<List<TaskItemComment>>();
        taskItem.Comments.Count.ShouldBe(0);
    }

    [Test]
    public void TaskItem_Category_ShouldAcceptTaskCategoryNavigation()
    {
        // Arrange
        var taskItem = new TaskItem();
        var category = new TaskCategory { CategoryName = "Work" };

        // Act
        taskItem.Category = category;

        // Assert
        taskItem.Category.ShouldBe(category);
        taskItem.Category.CategoryName.ShouldBe("Work");
    }

    [Theory]
    [TestCase(Priority.Low)]
    [TestCase(Priority.Medium)]
    [TestCase(Priority.High)]
    [TestCase(Priority.Critical)]
    public void TaskItem_Priority_ShouldAcceptAllValidPriorityValues(Priority priority)
    {
        // Arrange
        var taskItem = new TaskItem();

        // Act
        taskItem.Priority = priority;

        // Assert
        taskItem.Priority.ShouldBe(priority);
    }

    [Theory]
    [TestCase(Status.ToDo)]
    [TestCase(Status.InProgress)]
    [TestCase(Status.Completed)]
    [TestCase(Status.OnHold)]
    [TestCase(Status.Cancelled)]
    public void TaskItem_Status_ShouldAcceptAllValidStatusValues(Status status)
    {
        // Arrange
        var taskItem = new TaskItem();

        // Act
        taskItem.Status = status;

        // Assert
        taskItem.Status.ShouldBe(status);
    }
}
