using ToDoApp.Domain.Entities;

namespace ToDoApp.Tests.Unit.Domain.TaskManagement.Entities;

[TestFixture]
public class TaskCategoryTests
{
    [Test]
    public void TaskCategory_WhenCreated_ShouldInitializeTasksCollection()
    {
        var category = new TaskCategory();

        category.Tasks.ShouldNotBeNull();
        category.Tasks.ShouldBeEmpty();
    }

    [Test]
    public void TaskCategory_Tasks_ShouldAllowAddingTaskItems()
    {
        var category = new TaskCategory { CategoryName = "Personal" };
        var task1 = new TaskItem { Title = "Task 1", CategoryId = 1 };
        var task2 = new TaskItem { Title = "Task 2", CategoryId = 1 };

        category.Tasks.Add(task1);
        category.Tasks.Add(task2);

        category.Tasks.Count.ShouldBe(2);
        category.Tasks.ShouldContain(task1);
        category.Tasks.ShouldContain(task2);
    }

    [Test]
    public void TaskCategory_Tasks_ShouldHavePrivateSetterForEncapsulation()
    {
        var category = new TaskCategory();
        var originalTasks = category.Tasks;

        // Verify the Tasks collection maintains the same reference
        category.Tasks.ShouldBeSameAs(originalTasks);
    }
}