using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Interfaces.Data;

public interface ITaskCommentRepository : IRepository<TaskItemComment>
{
    Task<TaskItemComment> GetByIdAsync(long id, CancellationToken cancellationToken);
    IQueryable<TaskItemComment> GetAllByTaskIdQuery(long taskId);
}