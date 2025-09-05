using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Infrastructure.Data;
using ToDoApp.Infrastructure.Data.Interceptors;
using ToDoApp.Infrastructure.Data.Persistence;
using ToDoApp.Infrastructure.Data.Repositories;
using ToDoApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.Sqlite;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static void AddInfrastructureServices(this IHostApplicationBuilder builder)
    {
        var dataSource = builder.Configuration.GetConnectionString("DataSource");
        Guard.Against.Null(dataSource, message: "Connection string 'DataSource' not found.");

        builder.Services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        
         // keep the connection alive for the lifetime of the application 
         // This is due to the fact that the SQLite in-memory database will close the connection when the DbContext is disposed.
        var keepAliveConnection = new SqliteConnection(dataSource);
        keepAliveConnection.Open();

        builder.Services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlite(keepAliveConnection);
            options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));
        });


        builder.Services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

        builder.Services.AddScoped<ApplicationDbContextInitialiser>();

        // Add repositories
        builder.Services.AddScoped<ITaskItemRepository, TaskItemRepository>();
        builder.Services.AddScoped<ITaskCommentRepository, TaskCommentRepository>();
        builder.Services.AddScoped<ITaskCategoryRepository, TaskCategoryRepository>();

        builder.Services.AddSingleton(TimeProvider.System);
    }
}
