namespace AnswerCube.DAL.EF;

public class Repository
{
    private AnswerCubeDbContext _context;

    public Repository(AnswerCubeDbContext context)
    {
        _context = context;
    }
    
    
}