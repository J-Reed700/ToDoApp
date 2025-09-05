using Microsoft.EntityFrameworkCore.Infrastructure;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Application.Common.Interfaces.Data;

public interface IApplicationDbContext
{
    DbSet<TaskCategory> Categories { get; }

    DbSet<TaskItem> Items { get; }
    
    DbSet<TaskItemComment> TaskComments { get; }

    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
