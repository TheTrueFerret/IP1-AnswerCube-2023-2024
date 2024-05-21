using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.BL;

public interface IManager
{
    #region Organization
    List<IdentityRole> GetAllAvailableRoles(AnswerCubeUser user);
    List<AnswerCubeUser> GetAllUsers();
    bool GetDeelplatformBeheerderByEmail(string userEmail);
    bool AddDeelplatformBeheerderByEmail(string userEmail);
    bool IsUserInMultipleOrganizations(string userId);
    bool RemoveDeelplatformBeheerderByEmail(string userEmail, string deelplatformNaam);
    List<Organization> GetOrganizationByUserId(string userId);
    Organization GetOrganizationById(int organizationId);
    bool DeleteProject(int id);
    Project GetProjectById(int projectid);
    Task<Project> CreateProject(int organizationId, string title, string description, bool isActive);
    Task<bool> UpdateProject(Project project);
    Organization CreateNewOrganization(string email, string name);
    bool AddUserToOrganization(AnswerCubeUser user);
    void SaveBeheerderAndOrganization(string email, Organization organization);
    void CreateUserOrganization(AnswerCubeUser user, Organization organization);
    List<UserOrganization> GetDeelplatformBeheerderUsers();
    bool RemoveDpbFromOrganization(string userId, int organisationid);
    bool SearchOrganizationByName(string deelplatformName);
    List<Organization> GetOrganizations();
    bool IsUserInOrganization(string? userId, int organizationid);
    Task<bool> AddDpbToOrgByEmail(string email, string? userId, int organizationid);
    Organization GetOrganizationByName(string organizationName);
    Project GetProjectWithFlowsById(int projectId);
    #endregion

    #region Answers
    bool AddAnswer(List<string> answers, int id, Session session);
    List<Answer> GetAnswers();
    #endregion
    
    #region FlowManager

    #region Slide
    List<Slide> GetOpenSlides();
    List<Slide> GetListOfSlides();
    List<Slide> GetSingleChoiceSlides();
    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    Slide GetSlideFromFlow(int flowId, int number);
    Slide GetSlideById(int id);
    Slide GetSlideFromSlideListByIndex(int index, int slideListId);
    bool CreateSlide(SlideType type, string question, string[]? options,int slideListId,string? mediaUrl);
    List<Slide> GetAllSlides();
    void UpdateSlide(SlideType slideType, string text, List<string> answers, int slideId);
    IEnumerable<Slide> GetSlidesBySlideListId(int slideListId);
    bool RemoveSlideFromSlideList(int slideId, int slidelistid);
    #endregion
    
    #region SlideList
    SlideList GetSlideList();
    SlideList GetSlideListById(int id);
    bool CreateSlidelist(string title, string description, int flowId);
    SlideList GetSlideListByTitle(string title);
    SlideList GetSlideListWithFlowById(int slideListId);
    IEnumerable<SlideList> GetSlideListsByFlowId(int flowId);
    void UpdateSlideList(string title, string description, int slideListId);
    bool RemoveSlideListFromFlow(int slideListId, int flowId);
    #endregion
    
    #region Flow
    bool CreateFlow(string name, string desc, bool circularFlow, int projectId);
    Flow GetFlowById(int flowId);
    Flow GetFlowWithProjectById(int flowId);
    void UpdateFlow(Flow model);
    List<Flow> GetFlowsByUserId(string userId);
    Flow GetFlowByInstallationId(int installationId);
    #endregion
    
    #endregion
    
    #region Installation
    Installation StartInstallationWithFlow(int installationId, int flowId);
    bool UpdateInstallation(int id);
    int[] GetIndexAndSlideListFromInstallations(int id);
    Slide GetActiveSlideByInstallationId(int id);
    List<Installation> GetInstallationsByUserId(string userId);
    bool SetInstallationToActive(int installationId);
    bool AddNewInstallation(string name, string location, int organizationId);
    Session? GetSessionByInstallationIdAndCubeId(int installationId, int cubeId);
    Session AddNewSessionWithInstallationId(Session newSession, int installationId);
    bool AddSlideListToInstallation(int slideListId, int installationId);
    #endregion
    
    #region Forum
    
    List<Forum> GetForums();
    Forum GetForum(int forumId);
    bool AddIdea(int forumId, string title, string content, AnswerCubeUser user);
    bool AddReaction(int ideaId, string reaction,AnswerCubeUser? user);
    int GetForumByIdeaId(int ideaId);
    int GetForumByReactionId(int reactionId);
    bool LikeReaction(int reactionId,AnswerCubeUser user);
    bool DislikeReaction(int reactionId,AnswerCubeUser user);
    bool LikeIdea(int ideaId,AnswerCubeUser user);
    bool DislikeIdea(int ideaId,AnswerCubeUser user);

    #endregion
}