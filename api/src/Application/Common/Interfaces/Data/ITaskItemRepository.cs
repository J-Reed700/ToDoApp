using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Interfaces.Data;

public interface ITaskItemRepository : IRepository<TaskItem>
{
    Task<TaskItem> GetByIdAsync(long id, CancellationToken cancellationToken);
    Task<List<TaskItem>> GetAllAsync(CancellationToken cancellationToken);
    Task<List<TaskItem>> GetAllByCategoryIdAsync(long categoryId, CancellationToken cancellationToken);
    IQueryable<TaskItem> GetAllQuery();
    IQueryable<TaskItem> GetAllByCategoryIdQuery(long categoryId);
}