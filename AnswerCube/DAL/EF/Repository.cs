using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;

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
        Installation installation = _context.Installations.Where(i => i.Id == id).First();
        if (installation.CurrentSlideIndex < installation.MaxSlideIndex)
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
        SlideList slideList = _context.SlideLists.Where(sl => sl.Id == installation.ActiveSlideListId)
            .Include(sl => sl.Slides).First();

        LinkedListNode<Slide> slide;

        if (installation.MaxSlideIndex > installation.CurrentSlideIndex)
        {
            slide = slideList.Slides.First;
            for (int i = 0; i < installation.CurrentSlideIndex; i++)
            {
                slide = slide.Next;
            }
        }
        else
        {
            installation.CurrentSlideIndex = 0;
            slide = slideList.Slides.First;
        }

        return slide.Value;

        /* if (installation.MaxSlideIndex > installation.CurrentSlideIndex)
         {
             slide = installation.Slides[installation.CurrentSlideIndex];
         }
         else
         {
             installation.CurrentSlideIndex = 0;
             slide = installation.Slides[installation.CurrentSlideIndex];
         }
         return slide;*/
    }

    public List<IdentityRole> ReadAllAvailableRoles(IList<string> userRoles)
    {
        // Filter de rollen op basis van de rollen die de user al heeft
        var availableRoles = _context.Roles
            .Where(role => role.Name != null && !userRoles.Contains(role.Name))
            .ToList();

        return availableRoles;
    }

    public List<AnswerCubeUser> ReadAllUsers()
    {
        return _context.Users.ToList();
    }

    public bool ReadDeelplatformBeheerderByEmail(string userEmail)
    {
        return _context.DeelplatformbeheerderEmails.Any(d => d.Email == userEmail);
    }

    public bool CreateDeelplatformBeheerderByEmail(string userEmail)
    {
        DeelplatformbeheerderEmail deelplatformbeheerderEmail = new DeelplatformbeheerderEmail { Email = userEmail };
        ;
        _context.DeelplatformbeheerderEmails.Add(deelplatformbeheerderEmail);
        _context.SaveChanges();
        return true;
    }

    public bool DeleteDeelplatformBeheerderByEmail(string userEmail)
    {
        DeelplatformbeheerderEmail deelplatformbeheerderEmail =
            _context.DeelplatformbeheerderEmails.First(d => d.Email == userEmail);
        if (deelplatformbeheerderEmail == null || deelplatformbeheerderEmail.Email != userEmail)
        {
            return false;
        }

        _context.DeelplatformbeheerderEmails.Remove(deelplatformbeheerderEmail);
        _context.SaveChanges();
        return true;
    }

    public List<Organization> ReadOrganizationByUserId(string userId)
    {
        return _context.UserOrganizations.Where(uo => uo.UserId == userId)
            .Select(uo => uo.Organization).ToList();
    }

    public Organization ReadOrganizationById(int organizationId)
    {
        return _context.Organizations.Where(o => o.Id == organizationId).Include(o => o.Projects).First();
    }

    public bool DeleteProject(int id)
    {
        var project = _context.Projects.First(p => p.Id == id);
        if (project == null || project.Id != id)
        {
            return false;
        }

        _context.Projects.Remove(_context.Projects.First(p => p.Id == id));
        _context.SaveChanges();

        return true;
    }

    public Project ReadProjectById(int projectid)
    {
        return _context.Projects.First(p => p.Id == projectid);
    }

    public Project CreateProject(int organizationId)
    {
        Organization organization = _context.Organizations.First(o => o.Id == organizationId);
        
        Project project = new Project { Organization = organization };
        _context.Projects.Add(project);
        _context.SaveChanges();
        return project;
    }
}