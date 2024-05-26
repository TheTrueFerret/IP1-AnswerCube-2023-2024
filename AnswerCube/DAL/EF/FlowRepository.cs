using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnswerCube.DAL.EF;

public class FlowRepository : IFlowRepository
{
    private readonly AnswerCubeDbContext _context;
    private readonly IOrganizationRepository _organizationRepository;
    
    public FlowRepository(AnswerCubeDbContext context, IOrganizationRepository organizationRepository)
    {
        _context = context;
        _organizationRepository = organizationRepository;
    }
    
    #region Slide

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

    public Slide ReadSlideById(int id)
    {
        return _context.Slides.Include(s => s.ConnectedSlideLists).ThenInclude(cs => cs.SlideList)
            .First(s => s.Id == id);
    }

    public Slide GetSlideFromFlow(int flowId, int number)
    {
        SlideList slideList = getSlideList();
        List<SlideConnection> slideConnections = slideList.ConnectedSlides.ToList();
        Slide slide = slideConnections[number].Slide;

        return slide;
    }

    public Slide ReadSlideFromSlideListByIndex(int index, int slideListId)
    {
        SlideList slideList = getSlideList();
        List<SlideConnection> slideConnections = slideList.ConnectedSlides.ToList();
        Slide slide = slideConnections[index].Slide;

        return slide;
    }

    public List<Slide> ReadSlidesFromSlideList(SlideList slideList)
    {
        List<SlideConnection> slideConnections = _context.SlideConnections
            .Where(sc => sc.SlideList.Id == slideList.Id)
            .OrderBy(sc => sc.SlideOrder)
            .Include(sc => sc.Slide).ToList();

        List<Slide> slides = new List<Slide>();
        foreach (var slideConnection in slideConnections)
        {
            slides = _context.Slides.Where(s => s.Id == slideConnection.Slide.Id).ToList();
        }

        return slides;
    }

    public Slide ReadActiveSlideByInstallationId(int id)
    {
        Installation installation = _context.Installations.Where(i => i.Id == id).First();
        SlideList slideList = _context.SlideLists.Where(sl => sl.Id == installation.ActiveSlideListId)
            .Include(sl => sl.ConnectedSlides).First();

        SlideConnection slideConnections = _context.SlideConnections
            .Where(sc => sc.SlideList.Id == slideList.Id)
            .Where(sc => sc.SlideOrder == installation.CurrentSlideIndex).Single();

        Slide slide = _context.Slides.Where(s => s.Id == slideConnections.SlideId).Single();
        return slide;
    }

    public bool CreateSlide(SlideType type, string question, string[]? options, int slideListId, string? mediaUrl)
    {
        if (options == null || options.Length <= 0)
        {
            //Adding an OpenQuestion or Info Slide
            Slide slide = new Slide
            {
                SlideType = type,
                Text = question,
                AnswerList = null,
                MediaUrl = mediaUrl
            };
            SlideList slideList =
                _context.SlideLists.Include(sl => sl.ConnectedSlides).First(sl => sl.Id == slideListId);
            SlideConnection newSlideConnection = new SlideConnection
            {
                SlideId = slide.Id,
                Slide = slide,
                SlideList = slideList,
                SlideListId = slideList.Id
            };
            slideList.ConnectedSlides!.Add(newSlideConnection);
            _context.SlideConnections.Add(newSlideConnection);
            _context.Slides.Add(slide);
            _context.SaveChanges();
            return true;
        }
        else
        {
            //Adding a MultipleChoice,SingleChoice,Range or InfoSlide
            Slide slide = new Slide
            {
                SlideType = type,
                Text = question,
                AnswerList = options.ToList(),
                MediaUrl = mediaUrl
            };
            SlideList slideList =
                _context.SlideLists.Include(sl => sl.ConnectedSlides).First(sl => sl.Id == slideListId);
            SlideConnection newSlideConnection = new SlideConnection
            {
                SlideId = slide.Id,
                Slide = slide,
                SlideList = slideList,
                SlideListId = slideList.Id
            };
            slideList.ConnectedSlides!.Add(newSlideConnection);
            _context.SlideConnections.Add(newSlideConnection);
            _context.Slides.Add(slide);
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public IEnumerable<Slide> ReadSlidesBySlideListId(int slideListId)
    {
        return _context.Slides.Where(s => s.Id == slideListId).ToList();
    }

    public void UpdateSlide(string text, List<string>? answers, int slideId)
    {
        Slide slide = _context.Slides.Include(sl => sl.ConnectedSlideLists).ThenInclude(cs => cs.SlideList)
            .First(sl => sl.Id == slideId);

        if (slide == null)
        {
            throw new Exception("Slide not found in the database");
        }

        if (answers != null)
        {
            slide.AnswerList = answers;
        }
        slide.Text = text;

        _context.Slides.Update(slide);
        _context.SaveChanges();
    }

    public bool RemoveSlideFromSlideList(int slideId, int slidelistid)
    {
        Slide slide = _context.Slides.First(s => s.Id == slideId);
        SlideList slideList = _context.SlideLists.First(sl => sl.Id == slidelistid);
        SlideConnection slideConnection = new SlideConnection();

        if (slide != null || slideList != null)
        {
            slideConnection = _context.SlideConnections.First(sc =>
                sc.SlideId == slideId && sc.SlideListId == slidelistid);
        }

        _context.SlideConnections.Remove(slideConnection);
        _context.SaveChanges();
        return true;
    }

    #endregion

    #region SlideList

    public SlideList getSlideList()
    {
        return _context.SlideLists
            .Include(sl => sl.ConnectedSlides) // This will load the Slides of each SlideList
            .First();
    }

    public SlideList ReadSlideListById(int id)
    {
        SlideList slideList = _context.SlideLists.First(sl => sl.Id == id);

        if (slideList.ConnectedSlides == null)
        {
            // Handle the case where no SlideList with the given ID was found
            throw new Exception("SlideList not found with the provided ID");
        }

        return slideList;
    }

    public SlideList ReadSlideListByTitle(string title)
    {
        return _context.SlideLists.FirstOrDefault(sl => sl.Title == title);
    }

    public bool CreateSlideList(string title, string description, int flowId)
    {
        SlideList slideList = new SlideList
        {
            SubTheme = new SubTheme(title, description),
            FlowId = flowId,
            Title = title
        };
        _context.SlideLists.Add(slideList);
        _context.SaveChanges();

        var flow = _context.Flows.FirstOrDefault(f => f.Id == flowId);
        if (flow != null)
        {
            flow.SlideLists.Add(slideList);
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public bool RemoveSlideListFromFlow(int slideListId, int flowId)
    {
        SlideList slideList = _context.SlideLists.FirstOrDefault(sl => sl.Id == slideListId);
        Flow flow = _context.Flows.FirstOrDefault(f => f.Id == flowId);

        if (slideList != null || flow != null)
        {
            _context.SlideLists.Remove(slideList);
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public List<Slide> ReadSlideList()
    {
        return _context.Slides.ToList();
    }

    public SlideList ReadSlideListWithFlowById(int slideListId)
    {
        var slideList = _context.SlideLists
            .Include(sl => sl.Flow).ThenInclude(fl => fl.Project)
            .Include(sl => sl.SubTheme)
            .Include(sl => sl.ConnectedSlides)
            .ThenInclude(cs => cs.Slide)
            .First(slideList => slideList.Id == slideListId); // This will load the Slides of each SlideList

        if (slideList == null)
        {
            throw new Exception("No SlideList found in the database");
        }

        return slideList;
    }

    public IEnumerable<SlideList> GetSlideListsByFlowId(int flowId)
    {
        return _context.SlideLists.Include(sl => sl.ConnectedSlides)!.ThenInclude(cs => cs.Slide)
            .Where(sl => sl.Flow.Id == flowId).ToList();
    }

    /*public void UpdateSlideList(SlideList slideList)
    {
       _context.SlideLists.Include(sl =>sl.Flow).Include(sl => sl.ConnectedSlides).Include(Sl => slideList.SubTheme);
       _context.SlideLists.Update(slideList);
       _context.SaveChanges();
    }*/

    public void UpdateSlideList(string title, string description, int slideListId)
    {
        SlideList slideList = _context.SlideLists
            .Include(sl => sl.SubTheme)
            .First(sl => sl.Id == slideListId);

        if (slideList == null)
        {
            throw new Exception("SlideList not found (jammer gng) in the database");
        }

        if (slideList.SubTheme != null)
        {
            slideList.SubTheme.Name = title;
            slideList.SubTheme.Description = description;
        }

        slideList.Title = title;

        _context.SlideLists.Update(slideList);
        _context.SaveChanges();
    }
    
    public SlideList ReadSlideListByInstallationId(int installationId)
    {
        Installation installation = _context.Installations.Single(i => i.Id == installationId);
        SlideList slideList = _context.SlideLists.Include(sl => sl.SubTheme).Single(sl => sl.Id == installation.ActiveSlideListId);
        return slideList;
    }

    #endregion

    #region Flow

    public bool CreateFlow(string name, string desc, bool circularFlow, int projectId)
    {
        if (name.Length <= 0)
        {
            return false;
        }

        Flow flow = new Flow
        {
            Name = name,
            Description = desc,
            CircularFlow = circularFlow,
            Project = _context.Projects.First(p => p.Id == projectId)
        };
        _context.Projects.First(p => p.Id == projectId).Flows.Add(flow);
        _context.SaveChanges();
        return true;
    }

    public Flow ReadFlowById(int flowId)
    {
        return _context.Flows.Include(f => f.Project).FirstOrDefault(f => f.Id == flowId);
    }

    public Flow ReadFlowWithProjectById(int flowId)
    {
        return _context.Flows.Include(f => f.Project).Include(f => f.SlideLists).ThenInclude(s => s.ConnectedSlides)
            .FirstOrDefault(f => f.Id == flowId);
    }

    public void UpdateFlow(Flow model)
    {
        _context.Flows.Update(model);
        _context.SaveChanges();
    }

    public List<Flow> ReadFlowsByUserId(string userId)
    {
        List<Organization> organizations = _organizationRepository.ReadOrganizationByUserId(userId);
        List<Flow> flows = new List<Flow>();
        var projectIds = organizations.SelectMany(o => o.Projects).ToList();

        flows = _context.Flows
            .Where(flow => projectIds.Contains(flow.Project))
            .ToList();
        return flows;
    }

    public Flow ReadFlowByInstallationId(int installationId)
    {
        Installation installation = _context.Installations.Single(i => i.Id == installationId);
        return _context.Flows.Include(f => f.SlideLists).ThenInclude(sl => sl.SubTheme)
            .Single(f => f.Id == installation.FlowId);
    }

    #endregion
}