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


    public List<Slide> GetOpenSlides()
    {
        return _context.Slides.Where(s => s.SlideType == SlideType.OpenQuestion).ToList();
    }

    public List<Slide> GetListSlides()
    {
        return _context.Slides
            .Where(s => s.SlideType == SlideType.SingleChoice || s.SlideType == SlideType.MultipleChoice).ToList();
    }

    public List<Slide> GetSingleChoiceSlides()
    {
        return _context.Slides.Where(s => s.SlideType == SlideType.SingleChoice).ToList();
    }

    public List<Slide> GetMultipleChoiceSlides()
    {
        return _context.Slides.Where(s => s.SlideType == SlideType.MultipleChoice).ToList();
    }

    public SlideList ReadSlideList(int id)
    {
        return _context.SlideLists.Find(id);
    }

    public List<Slide> GetInfoSlides()
    {
        return _context.Slides.Where(s => s.SlideType == SlideType.Info).ToList();
    }

    public LinearFlow GetLinearFlow()
    {
        return _context.LinearFlows
            .Include(lf => lf.SlideList) // Ensure the SlideList is loaded
            .ThenInclude(sl => sl.Slides) // Then load the Slides of each SlideList
            .First();
    }

    public Slide GetSlideFromFlow(int flowId, int number)
    {
        SlideList slideList = getSlideList();
        List<Slide> slides = slideList.Slides.ToList();

        return slides[number - 1];
    }

    public SlideList getSlideList()
    {
        return _context.SlideLists
            .Include(sl => sl.Slides) // This will load the Slides of each SlideList
            .First();
    }

    public Boolean AddAnswer(List<string> answers, int id)
    {
        Slide slide = _context.Slides.Where(s => s.Id == id).First();

        Answer uploadAnswer = new Answer(answers, slide);
        if (answers == null)
        {
            return false;
        }
        else
        {
            _context.Answers.Add(uploadAnswer);
            _context.SaveChanges();

            return true;
        }

        return default;
    }

    
}