using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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

    public List<Slide> GetInfoSlides()
    {
        return _context.Slides.Where(s => s.SlideType == SlideType.InfoSlide).ToList();
    }

    public LinearFlow GetLinearFlow()
    {
        return _context.LinearFlows
            .Include(lf => lf.SlideList) // Ensure the SlideList is loaded
            .ThenInclude(sl => sl.Slides) // Then load the Slides of each SlideList
            .First();
    }

    public Slide ReadSlideById(int id)
    {
        return _context.Slides.SingleOrDefault(s => s.Id == id);
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
    
    public SlideList ReadSlideListById(int id)
    {
        SlideList slideList = _context.SlideLists
            .Where(sl => sl.Id == id)
            .Include(sl => sl.Slides).First();
        
        if (slideList == null)
        {
            // Handle the case where no SlideList with the given ID was found
            throw new Exception("SlideList not found with the provided ID");
        }

        return slideList;
    }

    public Boolean AddAnswer(List<string> answers, int id)
    {
        Slide slide = _context.Slides.First(s => s.Id == id);

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

    public Slide ReadSlideFromSlideListByIndex(int index, int slideListId)
    {
        SlideList slideList = getSlideList();
        List<Slide> slides = slideList.Slides.ToList();
        
        return slides[index];
    }


    public Boolean StartInstallation(int id, SlideList slideList)
    {
        Installation installation = _context.Installations.First(i => i.Id == id);
        installation.Active = true;
        installation.ActiveSlideListId = slideList.Id;
        installation.Slides = slideList.Slides;
        installation.CurrentSlideIndex = 0;
        installation.MaxSlideIndex = slideList.Slides.Count;
        _context.SaveChanges();
        return true;
    }


    public Boolean UpdateInstallation(int id)
    {
        Installation installation = _context.Installations.First(i => i.Id == id);
        if (installation.MaxSlideIndex > installation.CurrentSlideIndex)
        {
            installation.CurrentSlideIndex++;
            _context.SaveChanges();
            return true;
        }
        return false;
    }
    
    public int[] GetIndexAndSlideListFromInstallations(int id)
    {
        Installation installation = _context.Installations.Where(i => i.Id == id).First();
        if (installation.MaxSlideIndex > installation.CurrentSlideIndex)
        {
            int[] idArray = new int[]
            {
                installation.CurrentSlideIndex,
                installation.ActiveSlideListId
            };
            return idArray;
        }
        else
        {
            return null;
        }
    }


    public Slide ReadActiveSlideByInstallationId(int id)
    {
        Installation installation = _context.Installations.Where(i => i.Id == id).Include(i => i.Slides).First();
        SlideList slideList = _context.SlideLists.Where(sl => sl.Id == installation.ActiveSlideListId).Include(sl => sl.Slides).First();
        Slide slide = new Slide();
        if (installation.MaxSlideIndex > installation.CurrentSlideIndex)
        {
            slide = installation.Slides[installation.CurrentSlideIndex];
        }
        else
        {
            installation.CurrentSlideIndex = 0;
            slide = installation.Slides[installation.CurrentSlideIndex];
        }
        return slide;
    }
}