using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.DAL;

public interface IOrganizationRepository
{
    List<IdentityRole> ReadAllAvailableRoles(IList<string> userRoles);
    List<AnswerCubeUser> ReadAllUsers();
    bool ReadDeelplatformBeheerderByEmail(string userEmail);
    bool CreateDeelplatformBeheerderByEmail(string userEmail);
    bool IsUserInMultipleOrganizations(string userId);
    bool DeleteDeelplatformBeheerderByEmail(string userEmail, string deelplatformNaam);
    List<Organization> ReadOrganizationByUserId(string userId);
    Organization ReadOrganizationById(int organizationId);
    bool DeleteProject(int id);
    Project ReadProjectById(int projectid);
    Task<Project> CreateProject(int organizationId, string title, string description, bool isActive);
    Task<bool> UpdateProject(Project project);
    Project ReadProjectWithFlowsById(int projectId);
    Organization CreateNewOrganization(string email, string name, string? logoUrl);
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
}