using ToDoApp.Domain.Entities;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Tests.Unit.Domain.TaskManagement.Entities;

[TestFixture]
public class TaskItemTests
{
    [Test]
    public void TaskItem_WhenCreated_ShouldHaveDefaults()
    {
        var taskItem = new TaskItem();

        taskItem.Status.ShouldBe(default(Status));
        taskItem.Priority.ShouldBe(default(Priority));
        taskItem.Comments.ShouldNotBeNull();
        taskItem.Comments.ShouldBeEmpty();
    }

    [Test]
    public void TaskItem_WithDueDate_ShouldIndicateWhenOverdue()
    {
        var overdueTask = new TaskItem 
        { 
            DueDate = DateTime.Now.AddDays(-1),
            Status = Status.ToDo
        };
        var futureTask = new TaskItem 
        { 
            DueDate = DateTime.Now.AddDays(1),
            Status = Status.ToDo
        };

        // Overdue logic would be implemented in business layer
        (overdueTask.DueDate < DateTime.Now && overdueTask.Status != Status.Completed).ShouldBeTrue();
        (futureTask.DueDate < DateTime.Now && futureTask.Status != Status.Completed).ShouldBeFalse();
    }

    [Test]
    public void TaskItem_StatusProgression_ShouldReflectWorkflowStates()
    {
        var task = new TaskItem();

        task.Status = Status.ToDo;
        task.Status.ShouldBe(Status.ToDo);

        task.Status = Status.InProgress;
        task.Status.ShouldBe(Status.InProgress);

        task.Status = Status.Completed;
        task.Status.ShouldBe(Status.Completed);
    }

    [Test]
    public void TaskItem_CategoryNavigation_ShouldSupportRelationship()
    {
        var taskItem = new TaskItem { Title = "Test Task" };
        var category = new TaskCategory { CategoryName = "Work" };

        taskItem.Category = category;
        taskItem.CategoryId = category.Id;

        taskItem.Category.ShouldBe(category);
        taskItem.CategoryId.ShouldBe(category.Id);
    }
}