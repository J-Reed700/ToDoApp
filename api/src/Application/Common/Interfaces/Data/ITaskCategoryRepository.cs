using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Interfaces.Data;

public interface ITaskCategoryRepository : IRepository<TaskCategory>
{
    Task<TaskCategory> GetByIdAsync(long id, CancellationToken cancellationToken);
    IQueryable<TaskCategory> GetAllQuery(string? categoryName = null);
}