using Domain;

namespace AnswerCube.DAL.EF;

public class Repository : IRepository
{
    private AnswerCubeDbContext _context;

    public Repository(AnswerCubeDbContext context)
    {
        _context = context;
    }


    public List<Open_Question> GetOpenSlides()
    {
        return _context.OpenQuestions.ToList();
    }
}