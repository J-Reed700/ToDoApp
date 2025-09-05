using ToDoApp.Domain.Common;
using ToDoApp.Infrastructure.Data.Persistence;
using ToDoApp.Infrastructure.Repositories;

namespace ToDoApp.Tests.Unit.Infrastructure.Data.Repositories;

// Test entity for testing BaseRepository
public class TestEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}

// Concrete implementation of BaseRepository for testing
public class TestRepository : BaseRepository<TestEntity>
{
    private readonly List<TestEntity> _entities = new();
    
    public TestRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    protected override void Create(TestEntity entity, CancellationToken cancellationToken)
    {
        entity.Id = _entities.Count + 1;
        _entities.Add(entity);
    }

    protected override void Update(TestEntity entity, CancellationToken cancellationToken)
    {
        var existingEntity = _entities.FirstOrDefault(e => e.Id == entity.Id);
        if (existingEntity != null)
        {
            existingEntity.Name = entity.Name;
        }
    }

    protected override void Delete(TestEntity entity, CancellationToken cancellationToken)
    {
        _entities.RemoveAll(e => e.Id == entity.Id);
    }

    // For testing purposes
    public List<TestEntity> GetAllEntities() => _entities;
}

[TestFixture]
public class BaseRepositoryTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork;
    private TestRepository _repository;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _repository = new TestRepository(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task CreateAsync_ShouldBeginAndCommitTransaction()
    {
        // Arrange
        var entity = new TestEntity { Name = "Test Entity" };

        // Act
        var result = await _repository.CreateAsync(entity, CancellationToken.None);

        // Assert
        result.ShouldBe(entity);
        result.Id.ShouldBe(1); // Should be assigned by the Create method
        
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task CreateAsync_WhenCreateFails_ShouldRollbackTransaction()
    {
        // Arrange
        var entity = new TestEntity { Name = "Test Entity" };
        var expectedException = new Exception("Create failed");
        
        // Create a repository that throws on Create
        var failingRepository = new Mock<BaseRepository<TestEntity>>(_mockUnitOfWork.Object) { CallBase = true };
        failingRepository.Protected()
            .Setup("Create", ItExpr.IsAny<TestEntity>(), ItExpr.IsAny<CancellationToken>())
            .Throws(expectedException);

        // Act & Assert
        var thrownException = await Should.ThrowAsync<Exception>(
            () => failingRepository.Object.CreateAsync(entity, CancellationToken.None));

        thrownException.ShouldBe(expectedException);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task CreateAsync_WhenCommitFails_ShouldRollbackTransaction()
    {
        // Arrange
        var entity = new TestEntity { Name = "Test Entity" };
        var expectedException = new Exception("Commit failed");
        
        _mockUnitOfWork
            .Setup(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var thrownException = await Should.ThrowAsync<Exception>(
            () => _repository.CreateAsync(entity, CancellationToken.None));

        thrownException.ShouldBe(expectedException);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task UpdateAsync_ShouldBeginAndCommitTransaction()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Updated Entity" };

        // Act
        var result = await _repository.UpdateAsync(entity, CancellationToken.None);

        // Assert
        result.ShouldBe(entity);
        
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task UpdateAsync_WhenUpdateFails_ShouldRollbackTransaction()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Updated Entity" };
        var expectedException = new Exception("Update failed");
        
        var failingRepository = new Mock<BaseRepository<TestEntity>>(_mockUnitOfWork.Object) { CallBase = true };
        failingRepository.Protected()
            .Setup("Update", ItExpr.IsAny<TestEntity>(), ItExpr.IsAny<CancellationToken>())
            .Throws(expectedException);

        // Act & Assert
        var thrownException = await Should.ThrowAsync<Exception>(
            () => failingRepository.Object.UpdateAsync(entity, CancellationToken.None));

        thrownException.ShouldBe(expectedException);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task DeleteAsync_ShouldBeginAndCommitTransaction()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Entity to Delete" };

        // Act
        var result = await _repository.DeleteAsync(entity, CancellationToken.None);

        // Assert
        result.ShouldBeTrue();
        
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task DeleteAsync_WhenDeleteFails_ShouldRollbackTransaction()
    {
        // Arrange
        var entity = new TestEntity { Id = 1, Name = "Entity to Delete" };
        var expectedException = new Exception("Delete failed");
        
        var failingRepository = new Mock<BaseRepository<TestEntity>>(_mockUnitOfWork.Object) { CallBase = true };
        failingRepository.Protected()
            .Setup("Delete", ItExpr.IsAny<TestEntity>(), ItExpr.IsAny<CancellationToken>())
            .Throws(expectedException);

        // Act & Assert
        var thrownException = await Should.ThrowAsync<Exception>(
            () => failingRepository.Object.DeleteAsync(entity, CancellationToken.None));

        thrownException.ShouldBe(expectedException);
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task MultipleOperations_ShouldEachManageTheirOwnTransactions()
    {
        // Arrange
        var entity1 = new TestEntity { Name = "Entity 1" };
        var entity2 = new TestEntity { Id = 1, Name = "Updated Entity" };
        var entity3 = new TestEntity { Id = 2, Name = "Entity to Delete" };

        // Act
        await _repository.CreateAsync(entity1, CancellationToken.None);
        await _repository.UpdateAsync(entity2, CancellationToken.None);
        await _repository.DeleteAsync(entity3, CancellationToken.None);

        // Assert
        _mockUnitOfWork.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
        _mockUnitOfWork.Verify(x => x.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
        _mockUnitOfWork.Verify(x => x.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
