using AnswerCube.BL.Domain;
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


    public List<OpenQuestion> GetOpenSlides()
    {
        return _context.OpenQuestions.ToList();
    }
    
    public List<ListQuestion> GetListSlides()
    {
        return _context.ListQuestions.ToList();
    }

    public List<ListQuestion> GetSingleChoiceSlides()
    {
        return _context.ListQuestions.Where(q => q.TypeSlide == TypeSlide.SingleChoice).ToList();
    }
    
    public List<ListQuestion> GetMultipleChoiceSlides()
    {
        return _context.ListQuestions.Where(q => q.TypeSlide == TypeSlide.MultipleChoice).ToList();
    }
    
    public List<Info> GetInfoSlides()
    {
        return _context.InfoSlide.ToList();
    }

    // AbstractSlide ReadSlideById(int id)
    // {
    //     return _context.Slides.Find(id);
    // }

    public SlideList ReadSlideList(int id)
    {
        return _context.SlideLists.Find(id);
    }
    
    public List<SlideList> ReadSlideLists()
    {
        return _context.SlideLists.ToList();
    }

    
}