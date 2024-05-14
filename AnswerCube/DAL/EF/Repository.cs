using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AnswerCube.DAL.EF;

public class Repository : IRepository
{
    private readonly ILogger<Repository> _logger;
    private readonly AnswerCubeDbContext _context;

    public Repository(AnswerCubeDbContext context, ILogger<Repository> logger)
    {
        _context = context;
        _logger = logger;
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
        return _context.Slides.First(s => s.Id == id);
    }

    public Slide GetSlideFromFlow(int flowId, int number)
    {
        SlideList slideList = getSlideList();
        List<SlideConnection> slideConnections = slideList.ConnectedSlides.ToList();
        Slide slide = slideConnections[number].Slide;

        return slide;
    }

    public SlideList getSlideList()
    {
        return _context.SlideLists
            .Include(sl => sl.ConnectedSlides) // This will load the Slides of each SlideList
            .First();
    }

    public SlideList ReadSlideListById(int id)
    {
        SlideList slideList = _context.SlideLists
            .Where(sl => sl.Id == id)
            .Include(sl => sl.ConnectedSlides).First();

        if (slideList.ConnectedSlides == null)
        {
            // Handle the case where no SlideList with the given ID was found
            throw new Exception("SlideList not found with the provided ID");
        }

        return slideList;
    }

    public SlideList ReadSLideListByTitle(string title)
    {
        return _context.SlideLists.FirstOrDefault(sl => sl.Title == title);
    }

    public bool AddAnswer(List<string> answers, int id)
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
        List<SlideConnection> slideConnections = slideList.ConnectedSlides.ToList();
        Slide slide = slideConnections[index].Slide;

        return slide;
    }


    public bool StartInstallation(int id, SlideList slideList)
    {
        Installation installation = _context.Installations.First(i => i.Id == id);
        installation.Active = true;
        installation.ActiveSlideListId = slideList.Id;


        installation.Slides = new List<Slide>();
        foreach (var slide in ReadSlidesFromSlideList(slideList))
        {
            installation.Slides.Add(slide);
        }

        installation.CurrentSlideIndex = 0;
        installation.MaxSlideIndex = slideList.ConnectedSlides.Count;
        _context.SaveChanges();
        return true;
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


    public bool UpdateInstallation(int id)
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
            return new int[] { };
        }
    }


    public Slide ReadActiveSlideByInstallationId(int id)
    {
        Installation installation = _context.Installations.Where(i => i.Id == id).Include(i => i.Slides).First();
        SlideList slideList = _context.SlideLists.Where(sl => sl.Id == installation.ActiveSlideListId)
            .Include(sl => sl.ConnectedSlides).First();

        SlideConnection slideConnections = _context.SlideConnections
            .Where(sc => sc.SlideList.Id == slideList.Id)
            .Where(sc => sc.SlideOrder == installation.CurrentSlideIndex).Single();

        Slide slide = _context.Slides.Where(s => s.Id == slideConnections.SlideId).Single();
        return slide;
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
        //TODO: Make sure to delete all the organizations and projects that are linked to the user

        _context.DeelplatformbeheerderEmails.Remove(deelplatformbeheerderEmail);
        _context.UserOrganizations.RemoveRange(_context.UserOrganizations.Where(uo => uo.UserId == userEmail));
        _context.SaveChanges();
        return true;
    }

    public List<Organization> ReadOrganizationByUserId(string userId)
    {
        return _context.Organizations.Include(o => o.Projects).ThenInclude(p => p.Flows)
            .Include(o => o.UserOrganizations).ThenInclude(uo => uo.User)
            .Where(o => o.UserOrganizations.Any(uo => uo.UserId == userId)).ToList();
    }

    public Organization ReadOrganizationById(int organizationId)
    {
        return _context.Organizations.Where(o => o.Id == organizationId).Include(o => o.Projects)
            .ThenInclude(p => p.Flows)
            .Include(o => o.UserOrganizations).ThenInclude(uo => uo.User).First();
    }

    public bool DeleteProject(int id)
    {
        var project = _context.Projects.Include(p => p.Flows).First(p => p.Id == id);
        if (project == null || project.Id != id)
        {
            return false;
        }

        if (project.Flows.Count > 0)
        {
            foreach (var flow in project.Flows)
            {
                _context.Flows.Remove(flow);
            }
        }


        _context.Projects.Remove(_context.Projects.First(p => p.Id == id));
        _context.SaveChanges();

        return true;
    }

    public Project ReadProjectById(int projectid)
    {
        return _context.Projects.Include(p => p.Organization).Include(p => p.Flows)
            .FirstOrDefault(p => p.Id == projectid);
    }

    public async Task<Project> CreateProject(int organizationId, string title, string description, bool isActive)
    {
        Organization organization = _context.Organizations.First(o => o.Id == organizationId);

        Project project = new Project
        {
            Organization = organization,
            Title = title,
            Description = description,
            IsActive = isActive
        };
        _context.Projects.Add(project);
        await _context.SaveChangesAsync();
        return project;
    }

    public async Task<bool> UpdateProject(Project updatedProject)
    {
        // Fetch the existing project from the database
        var existingProject = _context.Projects.FirstOrDefault(p => p.Id == updatedProject.Id);

        if (existingProject == null)
        {
            // Handle the case where the project does not exist
            return false;
        }

        // Update the specific properties
        existingProject.Title = updatedProject.Title;
        existingProject.Description = updatedProject.Description;
        existingProject.IsActive = updatedProject.IsActive;

        // Save the changes
        _context.Projects.Update(existingProject);
        await _context.SaveChangesAsync();

        return true;
    }

    public List<Answer> GetAnswers()
    {
        var answers = _context.Answers
            .Include(a => a.Slide)
            .ToList();
        return answers;
    }

    public bool CreateSlide(SlideType type, string question, string[]? options, int slideListId)
    {
        if (options == null || options.Length <= 0)
        {
            //Adding an OpenQuestion or Info Slide
            Slide slide = new Slide
            {
                SlideType = type,
                Text = question,
                AnswerList = null
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
                AnswerList = options.ToList()
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

    public bool CreateSlideList(string title, string description, int flowId)
    {
        SlideList slideList = new SlideList
        {
            SubTheme = new SubTheme(title, description),
            FlowId = flowId,
            Title = title,
        };
        _context.SlideLists.Add(slideList);
        _context.SaveChanges();

        var flow = _context.Flows.FirstOrDefault(f => f.Id == flowId);
        if (flow != null)
        {
            flow.SlideList.Add(slideList);
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

    public Project ReadProjectWithFlowsById(int projectId)
    {
        return _context.Projects.Include(p => p.Flows).FirstOrDefault(p => p.Id == projectId);
    }

    public Flow ReadFlowById(int flowId)
    {
        return _context.Flows.Include(f => f.Project).FirstOrDefault(f => f.Id == flowId);
    }

    public Flow ReadFlowWithProjectById(int flowId)
    {
        return _context.Flows.Include(f => f.Project).FirstOrDefault(f => f.Id == flowId);
    }


    public SlideList GetSlideListWithFlowById(int slideListId)
    {
        var slideList = _context.SlideLists
            .Include(sl => sl.Flow).ThenInclude(flow => flow.Project)
            .Include(sl => sl.ConnectedSlides)
            .ThenInclude(cs => cs.Slide)
            .FirstOrDefault(slideList => slideList.Id == slideListId); // This will load the Slides of each SlideList

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

    public IEnumerable<Slide> ReadSlidesBySlideListId(int slideListId)
    {
        return _context.Slides.Where(s => s.Id == slideListId).ToList();
    }

    public void UpdateFlow(Flow model)
    {
        _context.Flows.Update(model);
        _context.SaveChanges();
    }

    public Organization CreateNewOrganization(string email, string name)
    {
        Organization organization = new Organization(name, email);
        _context.Organizations.Add(organization);
        _context.SaveChanges();
        return organization;
    }

    public void SaveBeheerderAndOrganization(string email, string organizationName)
    {
        _context.DeelplatformbeheerderEmails.Add(new DeelplatformbeheerderEmail
            { Email = email, DeelplatformNaam = organizationName });
        _context.SaveChanges();
    }

    public bool CreateUserOrganization(AnswerCubeUser user)
    {
        Organization organization = _context.Organizations.First(o => o.Name == _context.DeelplatformbeheerderEmails
            .First(d => d.Email == user.Email).DeelplatformNaam);

        if (organization != null)
        {
            _context.UserOrganizations.Add(new UserOrganization()
            {
                Organization = organization,
                OrganizationId = organization.Id,
                User = user,
                UserId = user.Id
            });
            _context.SaveChanges();
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<UserOrganization> ReadAllDeelplatformBeheerders()
    {
        return _context.UserOrganizations.Include(uo => uo.Organization).Include(uo => uo.User).ToList();
    }

    public void CreateNewUserOrganization(AnswerCubeUser user, Organization organization)
    {
        _context.UserOrganizations.Add(new UserOrganization()
        {
            Organization = organization,
            OrganizationId = organization.Id,
            User = user,
            UserId = user.Id
        });
        _context.SaveChanges();
    }

    public bool RemoveSlideFromList(int slideId, int slidelistid)
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

    public bool RemoveDpbFromOrganization(string userId, int organisationid)
    {
        UserOrganization userOrganization = _context.UserOrganizations
            .First(uo => uo.UserId == userId && uo.OrganizationId == organisationid);
        if (userOrganization == null)
        {
            return false;
        }

        _context.UserOrganizations.Remove(userOrganization);
        _context.SaveChanges();
        return true;
    }

    public bool SearchDeelplatformByName(string deelplatformName)
    {
        return _context.Organizations.Any(o => o.Name == deelplatformName);
    }

    public List<Forum> ReadForums()
    {
        return _context.Forums.Include(f => f.Organization).Include(f => f.Ideas).ToList();
    }

    public Forum ReadForum(int forumId)
    {
        _logger.LogInformation("Reading forum with id: " + forumId);
        foreach (var VARIABLE in _context.Forums)
        {
            _logger.LogInformation("Forum id: " + VARIABLE.Id);
        }

        return _context.Forums.Include(f => f.Ideas).ThenInclude(i => i.Reactions).Include(f => f.Organization)
            .First(f => f.Id == forumId);
    }

    public int ReadForumByIdeaId(int ideaId)
    {
        return _context.Ideas.First(i => i.Id == ideaId).ForumId;
    }

    public bool CreateReaction(int ideaId, string reaction)
    {
        Idea idea = _context.Ideas.First(i => i.Id == ideaId);
        //TODO:Add user if exists
        Reaction newReaction = new Reaction
        {
            Text = reaction,
            IdeaId = ideaId,
            Idea = idea,
            Date = DateTime.UtcNow,
        };
        _context.Reactions.Add(newReaction);
        _context.SaveChanges();
        return true;
    }

    public bool CreateIdea(int forumId, string title, string content)
    {
        Forum forum = _context.Forums.First(f => f.Id == forumId);
        // Create the new idea
        Idea newIdea = new Idea
        {
            Title = title,
            Content = content,
            ForumId = forumId,
            Forum = forum
        };

        _context.Ideas.Add(newIdea);
        _context.SaveChanges();
        _logger.LogInformation(newIdea.Id.ToString());
        return true;
    }

    public int ReadForumByReactionId(int reactionId)
    {
        return _context.Reactions.Include(reaction => reaction.Idea).First(r => r.Id == reactionId).Idea.ForumId;
    }

    public bool LikeReaction(int reactionId)
    {
        Reaction reaction = _context.Reactions.First(r => r.Id == reactionId);
        reaction.Likes++;
        _context.SaveChanges();
        return true;
    }

    public bool DislikeReaction(int reactionId)
    {
        Reaction reaction = _context.Reactions.First(r => r.Id == reactionId);
        reaction.Dislikes++;
        _context.SaveChanges();
        return true;
    }

    public bool LikeIdea(int ideaId)
    {
        Idea idea = _context.Ideas.First(i => i.Id == ideaId);
        idea.Likes++;
        _context.SaveChanges();
        return true;
    }

    public bool DislikeIdea(int ideaId)
    {
        Idea idea = _context.Ideas.First(i => i.Id == ideaId);
        idea.Dislikes++;
        _context.SaveChanges();
        return true;
    }
}