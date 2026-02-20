
using Microsoft.EntityFrameworkCore.Storage;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    private IDbContextTransaction? _transaction;
    public UnitOfWork(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
        
    }
    public async Task BeginTransactionAsync()
    {
        _transaction = await _appDbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        await _appDbContext.SaveChangesAsync();
        if (_transaction != null)
            await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
            await _transaction.RollbackAsync();
    }
    public void Dispose()
    {
        _transaction?.Dispose();
    }

  
}