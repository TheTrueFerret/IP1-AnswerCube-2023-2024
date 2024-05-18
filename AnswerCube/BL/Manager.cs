using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL;
using AnswerCube.DAL.EF;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.BL;

public class Manager : IManager
{
    private readonly IRepository _repository;
    private readonly UserManager<AnswerCubeUser>? _userManager;

    public Manager(IRepository repository, UserManager<AnswerCubeUser>? userManager)
    {
        _repository = repository;
        _userManager = userManager;
    }


    public List<Slide> GetOpenSlides()
    {
        return _repository.GetOpenSlides();
    }

    public List<Slide> GetListOfSlides()
    {
        return _repository.GetListSlides();
    }

    public List<Slide> GetSingleChoiceSlides()
    {
        return _repository.GetSingleChoiceSlides();
    }

    public List<Slide> GetMultipleChoiceSlides()
    {
        return _repository.GetMultipleChoiceSlides();
    }

    public List<Slide> GetInfoSlides()
    {
        return _repository.GetInfoSlides();
    }

    public Slide GetSlideFromFlow(int flowId, int number)
    {
        return _repository.GetSlideFromFlow(flowId, number);
    }

    public SlideList GetSlideList()
    {
        return _repository.getSlideList();
    }

    public SlideList GetSlideListById(int id)
    {
        return _repository.ReadSlideListById(id);
    }

    public Slide GetSlideById(int id)
    {
        return _repository.ReadSlideById(id);
    }

    public Boolean AddAnswer(List<string> answers, int id, Session session)
    {
        return _repository.AddAnswer(answers, id, session);
    }

    public Slide GetSlideFromSlideListByIndex(int index, int slideListId)
    {
        return _repository.ReadSlideFromSlideListByIndex(index, slideListId);
    }

    public Installation StartInstallationWithFlow(int installationId, int flowId)
    {
        return _repository.StartInstallationWithFlow(installationId, flowId);
    }

    public Boolean UpdateInstallation(int id)
    {
        return _repository.UpdateInstallation(id);
    }

    public int[] GetIndexAndSlideListFromInstallations(int id)
    {
        return _repository.GetIndexAndSlideListFromInstallations(id);
    }

    public Slide GetActiveSlideByInstallationId(int id)
    {
        return _repository.ReadActiveSlideByInstallationId(id);
    }

    public List<IdentityRole> GetAllAvailableRoles(AnswerCubeUser user)
    {
        var userRoles = _userManager.GetRolesAsync(user).Result.ToList();
        return _repository.ReadAllAvailableRoles(userRoles);
    }

    public List<AnswerCubeUser> GetAllUsers()
    {
        return _repository.ReadAllUsers();
    }

    public bool GetDeelplatformBeheerderByEmail(string userEmail)
    {
        return _repository.ReadDeelplatformBeheerderByEmail(userEmail);
    }

    public bool AddDeelplatformBeheerderByEmail(string userEmail)
    {
        return _repository.CreateDeelplatformBeheerderByEmail(userEmail);
    }

    public bool RemoveDeelplatformBeheerderByEmail(string userEmail)
    {
        return _repository.DeleteDeelplatformBeheerderByEmail(userEmail);
    }

    public List<Organization> GetOrganizationByUserId(string userId)
    {
        return _repository.ReadOrganizationByUserId(userId);
    }

    public Organization GetOrganizationById(int organizationId)
    {
        return _repository.ReadOrganizationById(organizationId);
    }

    public bool DeleteProject(int id)
    {
        return _repository.DeleteProject(id);
    }

    public Project GetProjectById(int projectid)
    {
        return _repository.ReadProjectById(projectid);
    }

    public async Task<Project> CreateProject(int organizationId, string title, string description, bool isActive)
    {
        return await _repository.CreateProject(organizationId, title, description, isActive);
    }

    public async Task<bool> UpdateProject(Project project)
    {
        return await _repository.UpdateProject(project);
    }

    public List<Answer> GetAnswers()
    {
        return _repository.GetAnswers();
    }

    public bool CreateSlide(SlideType type, string question, string[]? options, int slideListId, string? mediaUrl=null)
    {
        
        if (type == SlideType.InfoSlide && options.Length == 1)
        {
            string info = question + "\n" + options[0];
            return _repository.CreateSlide(type, info, null!, slideListId, mediaUrl);
        }

        return _repository.CreateSlide(type, question, options, slideListId,mediaUrl);
    }

    public List<Slide> GetAllSlides()
    {
        return _repository.ReadSlideList();
    }

    public bool CreateFlow(string name, string desc, bool circularFlow, int projectId)
    {
        return _repository.CreateFlow(name, desc, circularFlow, projectId);
    }

    public bool CreateSlidelist(string title, string description, int flowId)
    {
        return _repository.CreateSlideList(title, description, flowId);
    }

    public Project GetProjectWithFlowsById(int projectId)
    {
        return _repository.ReadProjectWithFlowsById(projectId);
    }

    public Flow GetFlowById(int flowId)
    {
        return _repository.ReadFlowById(flowId);
    }

    public SlideList GetSLideListByTitle(string title)
    {
        return _repository.ReadSLideListByTitle(title);
    }

    public Flow GetFlowWithProjectById(int flowId)
    {
        return _repository.ReadFlowWithProjectById(flowId);
    }

    public SlideList GetSlideListWithFlowById(int slideListId)
    {
        return _repository.GetSlideListWithFlowById(slideListId);
    }

    public IEnumerable<SlideList> GetSlideListsByFlowId(int flowId)
    {
        return _repository.GetSlideListsByFlowId(flowId);
    }

    public IEnumerable<Slide> GetSlidesBySlideListId(int slideListId)
    {
        return _repository.ReadSlidesBySlideListId(slideListId);
    }

    public void UpdateFlow(Flow model)
    {
        _repository.UpdateFlow(model);
    }

    public Organization CreateNewOrganization(string email, string name)
    {
        return _repository.CreateNewOrganization(email, name);
    }

    public bool AddUserToOrganization(AnswerCubeUser user)
    {
        return _repository.CreateUserOrganization(user);
    }

    public void SaveBeheerderAndOrganization(string email, Organization organization)
    {
        _repository.SaveBeheerderAndOrganization(email, organization.Name);
    }

    public void CreateUserOrganization(AnswerCubeUser user, Organization organization)
    {
        _repository.CreateNewUserOrganization(user, organization);
    }

    public List<UserOrganization> GetDeelplatformBeheerderUsers()
    {
        return _repository.ReadAllDeelplatformBeheerders();
    }

    public bool RemoveSlideFromList(int slideId, int slidelistid)
    {
        return _repository.RemoveSlideFromList(slideId, slidelistid);
    }

    public bool RemoveDpbFromOrganization(string userId, int organisationid)
    {
        return _repository.RemoveDpbFromOrganization(userId, organisationid);
    }

    public bool SearchDeelplatformByName(string deelplatformName)
    {
        return _repository.SearchDeelplatformByName(deelplatformName);
    }

    public bool RemoveSlideListFromFlow(int slideListId, int flowId)
    {
        return _repository.RemoveSlideListFromFlow(slideListId, flowId);
    }

    public List<Forum> GetForums()
    {
        return _repository.ReadForums();
    }

    public Forum GetForum(int forumId)
    {
        return _repository.ReadForum(forumId);
    }

    public bool AddIdea(int forumId, string title, string content, AnswerCubeUser user)
    {
        return _repository.CreateIdea(forumId, title, content,user);
    }


    public bool AddReaction(int ideaId, string reaction,AnswerCubeUser? user)
    {
        if (user != null)
        {
            return _repository.CreateReaction(ideaId, reaction,user);
        }
        else
        {
            return _repository.CreateReaction(ideaId, reaction,null);
        }
        
    }

    public int GetForumByIdeaId(int ideaId)
    {
        return _repository.ReadForumByIdeaId(ideaId);
    }

    public int GetForumByReactionId(int reactionId)
    {
        return _repository.ReadForumByReactionId(reactionId);
    }

    public bool LikeReaction(int reactionId, AnswerCubeUser user)
    {
        return _repository.LikeReaction(reactionId,user);
    }

    public bool DislikeReaction(int reactionId, AnswerCubeUser user)
    {
        return _repository.DislikeReaction(reactionId, user);
    }


    public bool LikeIdea(int ideaId, AnswerCubeUser user)
    {
        return _repository.LikeIdea(ideaId, user);
    }


    public bool DislikeIdea(int ideaId, AnswerCubeUser user)
    {
        return _repository.DislikeIdea(ideaId, user);
    }

    public List<Installation> GetInstallationsByUserId(string userId)
    {
        return _repository.ReadInstallationsByUserId(userId);
    }

    public bool SetInstallationToActive(int installationId)
    {
        return _repository.UpdateInstallationToActive(installationId);
    }

    public List<Flow> getFlowsByUserId(string userId)
    {
        return _repository.readFlowsByUserId(userId);
    }

    public bool AddNewInstallation(string name, string location, int organizationId)
    {
        return _repository.CreateNewInstallation(name, location, organizationId);
    }

    public Session? GetSessionByInstallationIdAndCubeId(int installationId, int cubeId)
    {
        return _repository.GetSessionByInstallationIdAndCubeId(installationId, cubeId);
    }

    public bool AddNewSessionWithInstallationId(Session newSession, int installationId)
    {
        return _repository.WriteNewSessionWithInstallationId(newSession, installationId);
    }

}