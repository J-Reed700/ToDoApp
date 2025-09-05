using Microsoft.EntityFrameworkCore;
using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Infrastructure.Data.Persistence;
using ToDoApp.Domain.Entities;

namespace ToDoApp.Infrastructure.Repositories;

public class TaskItemRepository : BaseRepository<TaskItem>, ITaskItemRepository
{
    private readonly IApplicationDbContext _context;

    public TaskItemRepository(IApplicationDbContext context, IUnitOfWork unitOfWork) : base( unitOfWork)
    {
        _context = context;
    }

    public async Task<TaskItem> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Items.FindAsync(new object[] { id }, cancellationToken) ?? throw new NotFoundException("Task item not found");
    }

    public IQueryable<TaskItem> GetAllByCategoryIdQuery(long categoryId)
    {
        return _context.Items.Where(x => x.CategoryId == categoryId);
    }

    public IQueryable<TaskItem> GetAllQuery()
    {
        return _context.Items;
    }

    public async Task<List<TaskItem>> GetAllByCategoryIdAsync(long categoryId, CancellationToken cancellationToken)
    {
        return await GetAllByCategoryIdQuery(categoryId).ToListAsync(cancellationToken);
    }

    public async Task<List<TaskItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await GetAllQuery().ToListAsync(cancellationToken);
    }

    protected override void Create(TaskItem task, CancellationToken cancellationToken)
    {
        _context.Items.Add(task);
    }

    protected override void Update(TaskItem task, CancellationToken cancellationToken)
    {
        var entity = _context.Items.Find(task.Id);

        Guard.Against.NotFound(task.Id, entity);

        if (task.Title != null)
            entity.Title = task.Title;
        
        if (task.Description != null)
            entity.Description = task.Description;
        
        entity.Status = task.Status;
        entity.Priority = task.Priority;
        entity.CategoryId = task.CategoryId;
        
        if (task.DueDate.HasValue)
            entity.DueDate = task.DueDate;
            
        _context.Items.Update(entity);
    }

    protected override void Delete(TaskItem task, CancellationToken cancellationToken)
    {
        _context.Items.Remove(task);
    }
}