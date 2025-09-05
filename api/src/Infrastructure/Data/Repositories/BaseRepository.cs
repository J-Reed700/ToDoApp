using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Domain.Common;
using ToDoApp.Infrastructure.Data.Persistence;

namespace ToDoApp.Infrastructure.Repositories;

public abstract class BaseRepository<T>: IRepository<T> where T : BaseEntity 
{
    private readonly IUnitOfWork _unitOfWork;

    public BaseRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<T> CreateAsync(T obj, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            Create(obj, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return obj;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<T> UpdateAsync(T obj, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            Update(obj, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return obj;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(T obj, CancellationToken cancellationToken)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            Delete(obj, cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    protected abstract void Create(T task, CancellationToken cancellationToken);

    protected abstract void Update(T task, CancellationToken cancellationToken);

    protected abstract void Delete(T task, CancellationToken cancellationToken);
}