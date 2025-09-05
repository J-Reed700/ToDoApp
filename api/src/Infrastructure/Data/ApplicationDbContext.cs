using System.Reflection;
using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDoApp.Infrastructure.Data;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<TaskCategory> Categories => Set<TaskCategory>();

    public DbSet<TaskItem> Items => Set<TaskItem>();

    public DbSet<TaskItemComment> TaskComments => Set<TaskItemComment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

}
