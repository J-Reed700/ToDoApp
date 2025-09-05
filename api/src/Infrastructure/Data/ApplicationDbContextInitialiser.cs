using ToDoApp.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ToDoApp.Domain.Enums;

namespace ToDoApp.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static void InitialiseDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        initialiser.Initialize();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void Initialize()
    {
        try
        {
            // Ensure the database is created
            _context.Database.EnsureCreated();

            // Check if data already exists
            if (_context.Categories != null && !_context.Categories.Any())
            {
                // Seed data
                _context.Categories.Add(new TaskCategory
                {
                    CategoryName = "Task Management MVP",
                    Tasks =
                    {
                        new TaskItem { 
                            Title = "Create a Task Board 📃",
                            Status = Status.Completed,
                            Priority = Priority.Medium,
                        },
                        new TaskItem { 
                            Title = "Create a Task Column ✅",
                            Status = Status.Completed,
                            Priority = Priority.Medium,
                        },
                        new TaskItem { 
                            Title = "Sip some coffee ☕",
                            Status = Status.OnHold,
                            Priority = Priority.High,
                            Comments =
                            {
                                new TaskItemComment { 
                                    Comment = "Waiting on the coffee beans to arrive",
                                },
                            },
                        },
                        new TaskItem { 
                            Title = "Create Production Ready Code 📃",
                            Status = Status.ToDo,
                            Priority = Priority.High,
                        },
                        new TaskItem { 
                            Title = "Create a Task Card 📃",
                            Status = Status.InProgress,
                            Priority = Priority.High,
                        },
                        new TaskItem { 
                            Title = "Create a Task Modal 🏆",
                            Status = Status.InProgress,
                            Priority = Priority.Critical,
                            Comments =
                            {
                                new TaskItemComment { 
                                    Comment = "Stick with it!",
                                },
                                new TaskItemComment { 
                                    Comment = "You can do it!",
                                },
                                new TaskItemComment { 
                                    Comment = "Keep going!",
                                },
                            },

                        },
                        new TaskItem { 
                            Title = "Celebrate 🎉",
                            Status = Status.Completed,
                            Priority = Priority.Critical,
                        },
                    }
                });

                _context.SaveChanges();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
}
