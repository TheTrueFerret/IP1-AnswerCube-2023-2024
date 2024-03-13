using AnswerCube.BL.Domain.Slide;
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
    
    public List<List_Question> getListSlides()
    {
        return _context.ListQuestions.ToList();
    }

    public List<List_Question> GetSingleChoiceSlides()
    {
        return _context.ListQuestions.Where(q => !q.IsMultipleChoice).ToList();
    }
    
    public List<List_Question> GetMultipleChoiceSlides()
    {
        return _context.ListQuestions.Where(q => q.IsMultipleChoice).ToList();
    }

    public List<Info> GetInfoSlides()
    {
        return _context.InfoSlide.ToList();
    }
}