namespace AnswerCube.DAL.EF;

public class UnitOfWork
{
    private readonly AnswerCubeDbContext _dbContext;

    public UnitOfWork(AnswerCubeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void BeginTransaction()
    {
        _dbContext.Database.BeginTransaction();
    }
    
    public void Commit()
    {
        _dbContext.SaveChanges();
        _dbContext.Database.CommitTransaction();
    }

    public async void CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
        await _dbContext.Database.CommitTransactionAsync();
    }
}