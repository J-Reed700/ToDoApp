using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using ToDoApp.Application.Common.Interfaces.Data;
using ToDoApp.Infrastructure.Data.Persistence;

namespace ToDoApp.Tests.Unit.Infrastructure.Data.Persistence;

[TestFixture]
public class UnitOfWorkTests
{
    private Mock<IApplicationDbContext> _mockContext;
    private Mock<DatabaseFacade> _mockDatabase;
    private Mock<IDbContextTransaction> _mockTransaction;
    private UnitOfWork _unitOfWork;

    [SetUp]
    public void SetUp()
    {
        _mockContext = new Mock<IApplicationDbContext>();
        _mockDatabase = new Mock<DatabaseFacade>(Mock.Of<DbContext>());
        _mockTransaction = new Mock<IDbContextTransaction>();
        
        _mockContext.Setup(x => x.Database).Returns(_mockDatabase.Object);
        _unitOfWork = new UnitOfWork(_mockContext.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _unitOfWork?.Dispose();
    }

    [Test]
    public async Task SaveChangesAsync_ShouldCallContextSaveChanges()
    {
        var expectedResult = 5;
        _mockContext
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        var result = await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        result.ShouldBe(expectedResult);
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task BeginTransactionAsync_ShouldCreateTransaction()
    {
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        await _unitOfWork.BeginTransactionAsync(CancellationToken.None);

        _mockDatabase.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task CommitTransactionAsync_WithActiveTransaction_ShouldCommit()
    {
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        await _unitOfWork.BeginTransactionAsync(CancellationToken.None);
        await _unitOfWork.CommitTransactionAsync(CancellationToken.None);

        _mockTransaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task RollbackTransactionAsync_WithoutTransaction_ShouldThrowException()
    {
        await Should.ThrowAsync<InvalidOperationException>(
            () => _unitOfWork.RollbackTransactionAsync(CancellationToken.None));
    }
}