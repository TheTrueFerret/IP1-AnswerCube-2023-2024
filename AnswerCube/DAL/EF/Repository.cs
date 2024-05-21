using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AnswerCube.DAL.EF;

public class Repository : IRepository
{
    private readonly ILogger<Repository> _logger;
    private readonly AnswerCubeDbContext _context;
    private readonly UserManager<AnswerCubeUser> _userManager;
    
    public Repository(AnswerCubeDbContext context, ILogger<Repository> logger, UserManager<AnswerCubeUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    #region Organization
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
    
    public Project ReadProjectWithFlowsById(int projectId)
    {
        return _context.Projects.Include(p => p.Flows).FirstOrDefault(p => p.Id == projectId);
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
        //HIERE
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
    
    public bool RemoveDpbFromOrganization(string userId, int organisationid)
    {
        UserOrganization userOrganization = _context.UserOrganizations.Include(uo => uo.User)
            .First(uo => uo.UserId == userId && uo.OrganizationId == organisationid);
        if (userOrganization == null)
        {
            return false;
        }

        string email = userOrganization.User.Email;
        DeleteDeelplatformBeheerderByEmail(email);
        _context.UserOrganizations.Remove(userOrganization);
        _context.SaveChanges();
        return true;
    }

    public bool SearchOrganizationByName(string organizationName)
    {
        return _context.Organizations.Any(o => o.Name == organizationName);
    }
    
    public List<Organization> ReadOrganizations()
    {
        return _context.Organizations.Include(o => o.Projects).ThenInclude(p => p.Flows)
            .Include(o => o.UserOrganizations).ThenInclude(uo => uo.User).ToList();
    }

    public bool IsUserInOrganization(string? userId, int organizationid)
    {
        return _context.UserOrganizations.Any(uo => uo.UserId == userId && uo.OrganizationId == organizationid);
    }

    public async Task<bool> CreateDpbToOrgByEmail(string email, string? userId, int organizationid)
    {
        var organization = _context.Organizations.Single(o => o.Id == organizationid);
        if (userId == null)
        {
            //Normally never null cause user needs role when on this page.
            return false;
        }

        // Check if the user is already part of the organization
        if (_context.UserOrganizations
            .Include(uo => uo.User)
            .Include(uo => uo.Organization)
            .Any(uo => uo.User.Email == email && uo.OrganizationId == organizationid))
        {
            return false;
        }

        AnswerCubeUser user = _context.Users.FirstOrDefault(u => u.Email == email);

        if (user != null)
        {
            // Check if a UserOrganization record already exists for this user and organization
            var existingUserOrganization = _context.UserOrganizations
                .FirstOrDefault(uo => uo.UserId == user.Id && uo.OrganizationId == organizationid);

            if (existingUserOrganization == null)
            {
                // If not, create a new UserOrganization record
                _context.UserOrganizations.Add(new UserOrganization
                {
                    UserId = user.Id,
                    OrganizationId = organizationid
                });
                _context.SaveChanges();
                await _userManager.AddToRoleAsync(user, "DeelplatformBeheerder");
            }

            return true;
        }

        //The user doesnt exist so we need to send email to register.
        SaveBeheerderAndOrganization(email, _context.Organizations.Single(o => o.Id == organizationid).Name);
        return true;
    }

    public Organization ReadOrganizationByName(string organizationName)
    {
        return _context.Organizations.First(o => o.Name == organizationName);
    }
    #endregion
    
    #region Answers
    public bool AddAnswer(List<string> answers, int id, Session session)
    {
        Slide slide = _context.Slides.First(s => s.Id == id);
        Answer newAnswer = new Answer
        {
            AnswerText = answers,
            Slide = slide,
            Session = session
        };
        _context.Answers.Add(newAnswer);
        _context.SaveChanges();
        return true;
    }
    public List<Answer> GetAnswers()
    {
        var answers = _context.Answers
            .Include(a => a.Slide)
            .ToList();
        return answers;
    }
    #endregion

    #region FlowManager

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
        return _context.Slides.Include(s=>s.ConnectedSlideLists).ThenInclude(cs => cs.SlideList).First(s => s.Id == id);
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
    
    public bool CreateSlide(SlideType type, string question, string[]? options, int slideListId,string? mediaUrl)
    {
        if (options == null || options.Length <= 0)
        {
            //Adding an OpenQuestion or Info Slide
            Slide slide = new Slide
            {
                SlideType = type,
                Text = question,
                AnswerList = null,
                mediaUrl = mediaUrl
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
                mediaUrl = mediaUrl
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
    
    public void UpdateSlide(SlideType slideType, string text, List<string> answers, int slideId)
    {
        Slide slide = _context.Slides.Include(sl => sl.ConnectedSlideLists).ThenInclude(cs => cs.SlideList)
            .First(sl => sl.Id == slideId);

        if (slide == null)
        {
            throw new Exception("Slide not found in the database");
        }

        slide.AnswerList = answers;
        slide.Text = text;
        slide.SlideType = slideType;
        
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
        return _context.Flows.Include(f => f.Project).Include(f => f.SlideLists).ThenInclude(s=>s.ConnectedSlides).FirstOrDefault(f => f.Id == flowId);
    }
    
    public void UpdateFlow(Flow model)
    {
        _context.Flows.Update(model);
        _context.SaveChanges();
    }
    
    public List<Flow> ReadFlowsByUserId(string userId)
    {
        List<Organization> organizations = ReadOrganizationByUserId(userId);
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
        return _context.Flows.Include(f => f.SlideLists).ThenInclude(sl => sl.SubTheme).Single(f => f.Id == installation.FlowId);
    }
    #endregion
    
    #endregion
    
    #region Installation
    public Installation StartInstallationWithFlow(int installationId, int flowId)
    {
        Installation installation = _context.Installations.Single(i => i.Id == installationId);
        installation.Active = true;
        installation.Flow = _context.Flows.Include(f => f.SlideLists).ThenInclude(sl => sl.ConnectedSlides).Single(f => f.Id == flowId);

        installation.ActiveSlideListId = null;
        installation.CurrentSlideIndex = 0;
        installation.MaxSlideIndex = null;
        _context.SaveChanges();
        return installation;
    }
    
    public bool UpdateInstallation(int installationId)
    {
        Installation installation = _context.Installations.Where(i => i.Id == installationId).First();
        if (installation.CurrentSlideIndex < installation.MaxSlideIndex)
        {
            installation.CurrentSlideIndex++;
            _context.SaveChanges();
            return true;
        }
        installation.ActiveSlideListId = null;
        // returns false if the currentslideindex exceeds the current slidelist
        return false;
    }

    public int[] GetIndexAndSlideListFromInstallations(int id)
    {
        Installation installation = _context.Installations.Where(i => i.Id == id).First();
        if (installation.MaxSlideIndex > installation.CurrentSlideIndex)
        {
            if (installation.ActiveSlideListId != null)
            {
                int[] idArray = new int[]
                {
                    installation.CurrentSlideIndex,
                    (int)installation.ActiveSlideListId
                };
                return idArray; 
            }
            return new int[] { };
        }
        return new int[] { };
    }
    
    public List<Installation> ReadInstallationsByUserId(string userId)
    {
        List<Organization> organizations = ReadOrganizationByUserId(userId);
        List<Installation> installations = new List<Installation>();
        foreach (var organization in organizations)
        {
            installations.AddRange(_context.Installations
                .Where(i => i.Organization == organization)
                .Where(i => i.Active == false));
        }
        return installations;
    }
    
    public bool UpdateInstallationToActive(int installationId)
    {
        Installation installation = _context.Installations.Where(i => i.Id == installationId).First();
        installation.Active = true;
        _context.Installations.Update(installation);
        _context.SaveChanges();
        return installation.Active;
    }
    
    public bool CreateNewInstallation(string name, string location, int organizationId)
    {
        Organization organization = _context.Organizations.Single(o => o.Id == organizationId);
        Installation installation = new Installation
        {
            Name = name,
            Location = location,
            Active = false,
            CurrentSlideIndex = 0,
            MaxSlideIndex = 0,
            ActiveSlideListId = 0,
            OrganizationId = organizationId,
            Organization = organization
        };
        _context.Installations.Add(installation);
        _context.SaveChanges();
        
        return false;
    }

    public Session? GetSessionByInstallationIdAndCubeId(int installationId, int cubeId)
    {
        Session? session = _context.Sessions.SingleOrDefault(s => s.Installation.Id == installationId && s.CubeId == cubeId);
        if (session != null)
        {
            return session;
        }
        return null;
    }

    public Session WriteNewSessionWithInstallationId(Session newSession, int installationId)
    {
        newSession.Installation = _context.Installations.Single(i => i.Id == installationId);
        _context.Sessions.Add(newSession);
        _context.SaveChanges();
        return _context.Sessions.Single(s => s.Installation.Id == installationId && s.CubeId == newSession.CubeId);
    }

    public bool WriteSlideListToInstallation(int slideListId, int installationId)
    {
        Installation installation = _context.Installations.Single(i => i.Id == installationId);
        installation.ActiveSlideListId = slideListId;
        installation.CurrentSlideIndex = 0;
        SlideList slideList = _context.SlideLists.Include(sl => sl.ConnectedSlides).Single(sl => sl.Id == slideListId);
        installation.MaxSlideIndex = slideList.ConnectedSlides.Count;
        _context.SaveChanges();
        return true;
    }
    #endregion
    
    #region Forum
    public List<Forum> ReadForums()
    {
        return _context.Forums.Include(f => f.Organization).Include(f => f.Ideas).ToList();
    }

    public Forum ReadForum(int forumId)
    {
        return _context.Forums
            .Include(f => f.Ideas).ThenInclude(i => i.Reactions).ThenInclude(r => r.Likes)
            .Include(f => f.Ideas).ThenInclude(i => i.Reactions).ThenInclude(r => r.Dislikes)
            .Include(f => f.Ideas).ThenInclude(i => i.Reactions).ThenInclude(r => r.User)
            .Include(f => f.Ideas).ThenInclude(i => i.Likes)
            .Include(f => f.Ideas).ThenInclude(i => i.Dislikes)
            .Include(f => f.Ideas).ThenInclude(i => i.User)
            .Include(f => f.Organization)
            .First(f => f.Id == forumId);
    }

    public int ReadForumByIdeaId(int ideaId)
    {
        return _context.Ideas.First(i => i.Id == ideaId).ForumId;
    }

    public bool CreateReaction(int ideaId, string reaction, AnswerCubeUser? user)
    {
        Idea idea = _context.Ideas.First(i => i.Id == ideaId);
        if (user != null)
        {
            _context.Reactions.Add(new Reaction
            {
                Text = reaction,
                IdeaId = ideaId,
                Idea = idea,
                Date = DateTime.UtcNow,
                User = user
            });
            _context.SaveChanges();
            return true;
        }
        else
        {
            _context.Reactions.Add(new Reaction
            {
                Text = reaction,
                IdeaId = ideaId,
                Idea = idea,
                Date = DateTime.UtcNow,
            });
            _context.SaveChanges();
            return true;
        }
    }

    public bool CreateIdea(int forumId, string title, string content, AnswerCubeUser user)
    {
        Forum forum = _context.Forums.Single(f => f.Id == forumId);
        // Create the new idea
        Idea newIdea = new Idea
        {
            Title = title,
            Content = content,
            ForumId = forumId,
            Forum = forum,
            User = user
        };

        _context.Ideas.Add(newIdea);
        _context.SaveChanges();
        return true;
    }

    public int ReadForumByReactionId(int reactionId)
    {
        return _context.Reactions.Include(reaction => reaction.Idea).Single(r => r.Id == reactionId).Idea.ForumId;
    }

    public bool LikeReaction(int reactionId, AnswerCubeUser user)
    {
        Reaction reaction = _context.Reactions.Include(r => r.Likes).Single(r => r.Id == reactionId);
        Like newLike = new Like
        {
            ReactionId = reactionId,
            Reaction = reaction,
            UserId = user.Id,
            User = user
        };
        //Remove the dislike
        Dislike? dislike = _context.Dislikes.SingleOrDefault(d => d.ReactionId == reactionId && d.UserId == user.Id);
        if (dislike != null)
        {
            _context.Dislikes.Remove(dislike);
        }

        reaction.Likes.Add(newLike);
        _context.SaveChanges();
        return true;
    }

    public bool DislikeReaction(int reactionId, AnswerCubeUser user)
    {
        Reaction reaction = _context.Reactions.Include(r => r.Dislikes).Single(r => r.Id == reactionId);
        Dislike newDislike = new Dislike
        {
            ReactionId = reactionId,
            Reaction = reaction,
            UserId = user.Id,
            User = user
        };
        //Remove the like
        Like? like = _context.Likes.SingleOrDefault(d => d.ReactionId == reactionId && d.UserId == user.Id);
        if (like != null)
        {
            _context.Likes.Remove(like);
        }

        reaction.Dislikes.Add(newDislike);
        _context.SaveChanges();
        return true;
    }

    public bool LikeIdea(int ideaId, AnswerCubeUser user)
    {
        Idea idea = _context.Ideas.Include(i => i.Likes).Single(i => i.Id == ideaId);
        Like newLike = new Like
        {
            IdeaId = ideaId,
            Idea = idea,
            UserId = user.Id,
            User = user
        };
        //Remove the dislike
        Dislike? dislike = _context.Dislikes.SingleOrDefault(d => d.IdeaId == ideaId && d.UserId == user.Id);
        if (dislike != null)
        {
            _context.Dislikes.Remove(dislike);
        }

        idea.Likes.Add(newLike);
        _context.SaveChanges();
        return true;
    }

    public bool DislikeIdea(int ideaId, AnswerCubeUser user)
    {
        Idea idea = _context.Ideas.Include(i => i.Dislikes).Single(i => i.Id == ideaId);
        Dislike newDislike = new Dislike
        {
            IdeaId = ideaId,
            Idea = idea,
            UserId = user.Id,
            User = user
        };
        //Remove the like
        Like? like = _context.Likes.SingleOrDefault(d => d.IdeaId == ideaId && d.UserId == user.Id);
        if (like != null)
        {
            _context.Likes.Remove(like);
        }

        idea.Dislikes.Add(newDislike);
        _context.SaveChanges();
        return true;
    }
    #endregion
}