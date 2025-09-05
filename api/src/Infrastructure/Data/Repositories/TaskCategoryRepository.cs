using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ToDoApp.Infrastructure.Repositories;

public class TaskCategoryRepository : BaseRepository<TaskCategory>, ITaskCategoryRepository
{
    private readonly IApplicationDbContext _context;

    public TaskCategoryRepository(IApplicationDbContext context, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _context = context;
    }
    public async Task<TaskCategory> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.Categories.FindAsync(new object[] { id }, cancellationToken) ?? throw new NotFoundException("Task category not found");
    }
    public IQueryable<TaskCategory> GetAllQuery(string? categoryName = null)
    {
        if (categoryName != null)
        {
            return _context.Categories.Where(c => c.CategoryName == categoryName).AsQueryable();
        }
        return _context.Categories.AsQueryable();
    }

    protected override void Create(TaskCategory category, CancellationToken cancellationToken)
    {
        _context.Categories.Add(category);
    }

    protected override void Update(TaskCategory category, CancellationToken cancellationToken)
    {
        var entity = _context.Categories.Find(category.Id);

        Guard.Against.NotFound(category.Id, entity);

        entity.CategoryName = category.CategoryName;

    }
    protected override void Delete(TaskCategory category, CancellationToken cancellationToken)
    {
        _context.Categories.Remove(category);
    }   

}
