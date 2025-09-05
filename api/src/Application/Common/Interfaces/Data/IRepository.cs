using ToDoApp.Domain.Common;

namespace ToDoApp.Application.Common.Interfaces.Data;

public interface IRepository<T> where T : BaseEntity
{
    Task<T> CreateAsync(T task, CancellationToken cancellationToken);
    Task<T> UpdateAsync(T task, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(T task, CancellationToken cancellationToken);
}