using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.DAL;

public interface IRepository
{
    List<Slide> GetOpenSlides();

    List<Slide> GetListSlides();

    List<Slide> GetSingleChoiceSlides();

    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    
    Slide GetSlideFromFlow(int flowId, int number);
    Slide ReadSlideById(int id);
    SlideList getSlideList();
    SlideList ReadSlideListById(int id);
    Boolean AddAnswer(List<string> answers,int id);
    Slide ReadSlideFromSlideListByIndex(int index, int slideListId);
    
    Installation StartInstallationWithFlow(int installationId, int flowId);
    Boolean UpdateInstallation(int id);
    int[] GetIndexAndSlideListFromInstallations(int id);
    Slide ReadActiveSlideByInstallationId(int id);
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
    List<Answer> GetAnswers();
    bool CreateSlide(SlideType type, string question, string[]? options, int slideListId,string? mediaUrl);
    bool CreateSlideList(string title, string description, int flowId);
    bool RemoveSlideListFromFlow(int slideListId, int flowId);
    List<Slide> ReadSlideList();
    SlideList ReadSLideListByTitle(string title);
    bool CreateFlow(string name, string desc, bool circularFlow,int projectId);
    Project ReadProjectWithFlowsById(int projectId);
    Flow ReadFlowById(int flowId);
    Flow ReadFlowWithProjectById(int flowId);
    SlideList GetSlideListWithFlowById(int slideListId);
    IEnumerable<SlideList> GetSlideListsByFlowId(int flowId);
    IEnumerable<Slide> ReadSlidesBySlideListId(int slideListId);
    void UpdateFlow(Flow model); 
    Organization CreateNewOrganization(string email, string name);
    void SaveBeheerderAndOrganization(string email, string organizationName);
    bool CreateUserOrganization(AnswerCubeUser user);
    List<UserOrganization> ReadAllDeelplatformBeheerders();
    void CreateNewUserOrganization(AnswerCubeUser user, Organization organization);
    bool RemoveSlideFromList(int slideId, int slidelistid);
    bool RemoveDpbFromOrganization(string userId, int organisationid);
    bool SearchDeelplatformByName(string deelplatformName);
    List<Forum> ReadForums();
    Forum ReadForum(int forumId);
    int ReadForumByIdeaId(int ideaId);
    bool CreateReaction(int ideaId, string reaction,AnswerCubeUser? user);
    bool CreateIdea(int forumId, string title, string content,AnswerCubeUser user);
    int ReadForumByReactionId(int reactionId);
    bool LikeReaction(int reactionId,AnswerCubeUser user);
    bool DislikeReaction(int reactionId,AnswerCubeUser user);
    bool LikeIdea(int ideaId,AnswerCubeUser user);
    bool DislikeIdea(int ideaId,AnswerCubeUser user);
    List<Installation> ReadInstallationsByUserId(string userId);
    bool UpdateInstallationToActive(int installationId);
    List<Flow> readFlowsByUserId(string userId);
    bool CreateNewInstallation(string name, string location, int organizationId);
}