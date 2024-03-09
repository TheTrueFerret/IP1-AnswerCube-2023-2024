namespace AnswerCube.DAL.EF;

public class Repository : IRepository
{
    private AnswerCubeDbContext _context;

    public Repository(AnswerCubeDbContext context)
    {
        _context = context;
    }
    
    
}