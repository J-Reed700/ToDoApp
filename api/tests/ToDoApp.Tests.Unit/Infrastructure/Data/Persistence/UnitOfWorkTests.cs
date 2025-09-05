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
    public async Task SaveChangesAsync_ShouldCallContextSaveChangesAsync()
    {
        // Arrange
        var expectedResult = 5;
        _mockContext
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _unitOfWork.SaveChangesAsync(CancellationToken.None);

        // Assert
        result.ShouldBe(expectedResult);
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task BeginTransactionAsync_ShouldStartNewTransaction()
    {
        // Arrange
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        // Act
        await _unitOfWork.BeginTransactionAsync(CancellationToken.None);

        // Assert
        _mockDatabase.Verify(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task BeginTransactionAsync_WhenTransactionAlreadyExists_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        await _unitOfWork.BeginTransactionAsync(CancellationToken.None);

        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidOperationException>(
            () => _unitOfWork.BeginTransactionAsync(CancellationToken.None));

        exception.Message.ShouldBe("Transaction already started");
    }

    [Test]
    public async Task CommitTransactionAsync_WithoutTransaction_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidOperationException>(
            () => _unitOfWork.CommitTransactionAsync(CancellationToken.None));

        exception.Message.ShouldBe("No transaction to commit");
    }

    [Test]
    public async Task CommitTransactionAsync_WithTransaction_ShouldCommitSuccessfully()
    {
        // Arrange
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        await _unitOfWork.BeginTransactionAsync(CancellationToken.None);

        // Act
        await _unitOfWork.CommitTransactionAsync(CancellationToken.None);

        // Assert
        _mockContext.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockTransaction.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockTransaction.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Test]
    public async Task CommitTransactionAsync_WhenSaveChangesFails_ShouldRollbackAndRethrow()
    {
        // Arrange
        var expectedException = new Exception("Save failed");
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        _mockContext
            .Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        await _unitOfWork.BeginTransactionAsync(CancellationToken.None);

        // Act & Assert
        var thrownException = await Should.ThrowAsync<Exception>(
            () => _unitOfWork.CommitTransactionAsync(CancellationToken.None));

        thrownException.ShouldBe(expectedException);
        _mockTransaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockTransaction.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Test]
    public async Task CommitTransactionAsync_WhenCommitFails_ShouldRollbackAndRethrow()
    {
        // Arrange
        var expectedException = new Exception("Commit failed");
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        _mockTransaction
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(expectedException);

        await _unitOfWork.BeginTransactionAsync(CancellationToken.None);

        // Act & Assert
        var thrownException = await Should.ThrowAsync<Exception>(
            () => _unitOfWork.CommitTransactionAsync(CancellationToken.None));

        thrownException.ShouldBe(expectedException);
        _mockTransaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockTransaction.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Test]
    public async Task RollbackTransactionAsync_WithoutTransaction_ShouldThrowInvalidOperationException()
    {
        // Act & Assert
        var exception = await Should.ThrowAsync<InvalidOperationException>(
            () => _unitOfWork.RollbackTransactionAsync(CancellationToken.None));

        exception.Message.ShouldBe("No transaction to rollback");
        _mockTransaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task RollbackTransactionAsync_WithTransaction_ShouldRollbackSuccessfully()
    {
        // Arrange
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        await _unitOfWork.BeginTransactionAsync(CancellationToken.None);

        // Act
        await _unitOfWork.RollbackTransactionAsync(CancellationToken.None);

        // Assert
        _mockTransaction.Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockTransaction.Verify(x => x.DisposeAsync(), Times.Once);
    }

    [Test]
    public void Dispose_ShouldDisposeTransaction()
    {
        // Arrange
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        _unitOfWork.BeginTransactionAsync(CancellationToken.None).Wait();

        // Act
        _unitOfWork.Dispose();

        // Assert
        _mockTransaction.Verify(x => x.Dispose(), Times.Once);
    }

    [Test]
    public void Dispose_WithoutTransaction_ShouldNotThrow()
    {
        // Act & Assert - Should not throw any exception
        Should.NotThrow(() => _unitOfWork.Dispose());
    }

    [Test]
    public void Dispose_CalledMultipleTimes_ShouldOnlyDisposeOnce()
    {
        // Arrange
        _mockDatabase
            .Setup(x => x.BeginTransactionAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(_mockTransaction.Object);

        _unitOfWork.BeginTransactionAsync(CancellationToken.None).Wait();

        // Act
        _unitOfWork.Dispose();
        _unitOfWork.Dispose(); // Second call

        // Assert
        _mockTransaction.Verify(x => x.Dispose(), Times.Once);
    }
}
