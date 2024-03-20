using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using Domain;
using Microsoft.EntityFrameworkCore;

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
        return _context.ListQuestions.Where(q => !q.IsMultipleChoice).ToList();
    }

    public List<ListQuestion> GetMultipleChoiceSlides()
    {
        return _context.ListQuestions.Where(q => q.IsMultipleChoice).ToList();
    }

    public SlideList ReadSlideList(int id)
    {
        return _context.SlideLists.Find(id);
    }

    public List<Info> GetInfoSlides()
    {
        return _context.InfoSlides.ToList();
    }

    public LinearFlow GetLinearFlow()
    {
        return _context.LinearFlows
            .Include(lf => lf.SlideList) // Ensure the SlideList is loaded
            .ThenInclude(sl => sl.Slides) // Then load the Slides of each SlideList
            .First();
    }

    public AbstractSlide GetSlideFromFlow(int flowId, int number)
    {
        List<OpenQuestion> open = _context.OpenQuestions.ToList();
        List<ListQuestion> listQuestions = _context.ListQuestions.ToList();
        List<Info> info = _context.InfoSlides.ToList();

        List<AbstractSlide> slides = new List<AbstractSlide>();
        slides.Add(open[0]);
        slides.Add(listQuestions[0]);
        slides.Add(info[0]);
        slides.Add(info[1]);
        slides.Add(listQuestions[1]);
        slides.Add(listQuestions[8]);
        slides.Add(open[1]);

        return slides[number - 1];
    }
}