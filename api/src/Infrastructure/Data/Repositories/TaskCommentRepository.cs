using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Domain.Entities;
using ToDoApp.Infrastructure.Data.Persistence;
using ToDoApp.Infrastructure.Repositories;

namespace ToDoApp.Infrastructure.Data.Repositories;

public class TaskCommentRepository : BaseRepository<TaskItemComment>, ITaskCommentRepository
{
    private readonly IApplicationDbContext _context;

    public TaskCommentRepository(IApplicationDbContext context, IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _context = context;
    }

    public async Task<TaskItemComment> GetByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.TaskComments.FindAsync(new object[] { id }, cancellationToken) ?? throw new NotFoundException("Task comment not found");
    }

    public IQueryable<TaskItemComment> GetAllByTaskIdQuery(long taskId)
    {
        return _context.TaskComments.Where(x => x.TaskId == taskId);
    }

    protected override void Create(TaskItemComment comment, CancellationToken cancellationToken)
    {
        _context.TaskComments.Add(comment);
    }

    protected override void Update(TaskItemComment comment, CancellationToken cancellationToken)
    {
        var entity = _context.TaskComments.Find(comment.Id);

        Guard.Against.NotFound(comment.Id, entity);

        entity.Comment = comment.Comment;
    }

    protected override void Delete(TaskItemComment comment, CancellationToken cancellationToken)
    {
        _context.TaskComments.Remove(comment);
    }
}