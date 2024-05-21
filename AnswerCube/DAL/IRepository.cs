using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.DAL;

public interface IRepository
{
    #region Organization
    List<IdentityRole> ReadAllAvailableRoles(IList<string> userRoles);
    List<AnswerCubeUser> ReadAllUsers();
    bool ReadDeelplatformBeheerderByEmail(string userEmail);
    bool CreateDeelplatformBeheerderByEmail(string userEmail);
    bool DeleteDeelplatformBeheerderByEmail(string userEmail);
    List<Organization> ReadOrganizationByUserId(string userId);
    Organization ReadOrganizationById(int organizationId);
    bool DeleteProject(int id);
    Project ReadProjectById(int projectid);
    Task<Project> CreateProject(int organizationId, string title, string description, bool isActive);
    Task<bool> UpdateProject(Project project);
    Project ReadProjectWithFlowsById(int projectId);
    Organization CreateNewOrganization(string email, string name);
    void SaveBeheerderAndOrganization(string email, string organizationName);
    bool CreateUserOrganization(AnswerCubeUser user);
    List<UserOrganization> ReadAllDeelplatformBeheerders();
    void CreateNewUserOrganization(AnswerCubeUser user, Organization organization);
    bool RemoveDpbFromOrganization(string userId, int organisationid);
    bool SearchOrganizationByName(string organizationName);
    List<Organization> ReadOrganizations();
    bool IsUserInOrganization(string? userId, int organizationid);
    Task<bool> CreateDpbToOrgByEmail(string email, string? userId, int organizationid);
    Organization ReadOrganizationByName(string organizationName);
    #endregion

    #region Answers
    Boolean AddAnswer(List<string> answers,int id, Session session);
    List<Answer> GetAnswers();
    #endregion

    #region FlowManager

    #region Slide
    List<Slide> GetOpenSlides();
    List<Slide> GetListSlides();
    List<Slide> GetSingleChoiceSlides();
    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    Slide GetSlideFromFlow(int flowId, int number);
    Slide ReadSlideById(int id);
    Slide ReadSlideFromSlideListByIndex(int index, int slideListId);
    bool CreateSlide(SlideType type, string question, string[]? options, int slideListId,string? mediaUrl);
    void UpdateSlide(SlideType slideType, string text, List<string> answers, int slideId);
    IEnumerable<Slide> ReadSlidesBySlideListId(int slideListId);
    bool RemoveSlideFromSlideList(int slideId, int slidelistid);
    #endregion

    #region SlideList
    SlideList getSlideList();
    SlideList ReadSlideListById(int id);
    bool CreateSlideList(string title, string description, int flowId);
    bool RemoveSlideListFromFlow(int slideListId, int flowId);
    List<Slide> ReadSlideList();
    SlideList ReadSlideListByTitle(string title);
    SlideList ReadSlideListBySlideId(int slideId);
    SlideList ReadSlideListWithFlowById(int slideListId);
    IEnumerable<SlideList> GetSlideListsByFlowId(int flowId);
    void UpdateSlideList(string title, string description, int slideListId);
    #endregion

    #region Flow
    bool CreateFlow(string name, string desc, bool circularFlow, int projectId);
    Flow ReadFlowById(int flowId);
    Flow ReadFlowWithProjectById(int flowId);
    void UpdateFlow(Flow model);
    List<Flow> ReadFlowsByUserId(string userId);
    Flow ReadFlowByInstallationId(int installationId);
    #endregion
    
    #endregion

    #region Installation
    Installation StartInstallationWithFlow(int installationId, int flowId);
    Boolean UpdateInstallation(int id);
    int[] GetIndexAndSlideListFromInstallations(int id);
    Slide ReadActiveSlideByInstallationId(int id);
    List<Installation> ReadInstallationsByUserId(string userId);
    bool UpdateInstallationToActive(int installationId);
    bool CreateNewInstallation(string name, string location, int organizationId);
    Session? GetSessionByInstallationIdAndCubeId(int installationId, int cubeId);
    Session WriteNewSessionWithInstallationId(Session newSession, int installationId);
    bool WriteSlideListToInstallation(int slideListId, int installationId);
    #endregion
    
    #region Forum
    List<Forum> ReadForums();
    Forum ReadForum(int forumId);
    int ReadForumByIdeaId(int ideaId);
    bool CreateReaction(int ideaId, string reaction, AnswerCubeUser? user);
    bool CreateIdea(int forumId, string title, string content, AnswerCubeUser user);
    int ReadForumByReactionId(int reactionId);
    bool LikeReaction(int reactionId, AnswerCubeUser user);
    bool DislikeReaction(int reactionId, AnswerCubeUser user);
    bool LikeIdea(int ideaId, AnswerCubeUser user);
    bool DislikeIdea(int ideaId, AnswerCubeUser user);
    #endregion
}