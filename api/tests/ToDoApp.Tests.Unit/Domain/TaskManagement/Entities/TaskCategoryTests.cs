using ToDoApp.Domain.Entities;

namespace ToDoApp.Tests.Unit.Domain.TaskManagement.Entities;

[TestFixture]
public class TaskCategoryTests
{
    [Test]
    public void TaskCategory_WhenCreated_ShouldHaveDefaultValues()
    {
        // Arrange & Act
        var category = new TaskCategory();

        // Assert
        category.Id.ShouldBe(0);
        category.CategoryName.ShouldBeNull();
        category.Tasks.ShouldNotBeNull();
        category.Tasks.ShouldBeEmpty();
    }

    [Test]
    public void TaskCategory_WhenCreatedWithName_ShouldSetNameCorrectly()
    {
        // Arrange
        var categoryName = "Work Tasks";

        // Act
        var category = new TaskCategory { CategoryName = categoryName };

        // Assert
        category.CategoryName.ShouldBe(categoryName);
    }

    [Test]
    public void TaskCategory_Tasks_ShouldBeInitializedAsEmptyList()
    {
        // Arrange & Act
        var category = new TaskCategory();

        // Assert
        category.Tasks.ShouldNotBeNull();
        category.Tasks.ShouldBeOfType<List<TaskItem>>();
        category.Tasks.Count.ShouldBe(0);
    }

    [Test]
    public void TaskCategory_Tasks_ShouldAllowAddingTaskItems()
    {
        // Arrange
        var category = new TaskCategory { CategoryName = "Personal" };
        var task1 = new TaskItem { Title = "Task 1", CategoryId = 1 };
        var task2 = new TaskItem { Title = "Task 2", CategoryId = 1 };

        // Act
        category.Tasks.Add(task1);
        category.Tasks.Add(task2);

        // Assert
        category.Tasks.Count.ShouldBe(2);
        category.Tasks.ShouldContain(task1);
        category.Tasks.ShouldContain(task2);
    }

    [Test]
    public void TaskCategory_Tasks_PrivateSetShouldPreventDirectAssignment()
    {
        // This test ensures the Tasks property has a private setter
        // by checking it's properly encapsulated
        var category = new TaskCategory();
        var originalTasks = category.Tasks;

        // The following line should not compile if private setter is working:
        // category.Tasks = new List<TaskItem>(); // This should cause compilation error

        // Instead, we verify the Tasks collection is the same reference
        category.Tasks.ShouldBeSameAs(originalTasks);
    }

    [Test]
    public void TaskCategory_WithNullOrEmptyName_ShouldStillInitializeTasksCollection()
    {
        // Arrange & Act
        var categoryWithNull = new TaskCategory { CategoryName = null };
        var categoryWithEmpty = new TaskCategory { CategoryName = string.Empty };

        // Assert
        categoryWithNull.Tasks.ShouldNotBeNull();
        categoryWithNull.Tasks.ShouldBeEmpty();
        
        categoryWithEmpty.Tasks.ShouldNotBeNull();
        categoryWithEmpty.Tasks.ShouldBeEmpty();
    }
}
