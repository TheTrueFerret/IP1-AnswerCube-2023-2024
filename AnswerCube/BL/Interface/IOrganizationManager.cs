using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.BL;

public interface IOrganizationManager
{
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
    Organization CreateNewOrganization(string email, string name, string? logoUrl);
    Task AddUserToOrganization(AnswerCubeUser user);
    void SaveBeheerderAndOrganization(string email, Organization organization);
    void CreateUserOrganization(AnswerCubeUser user, Organization organization);
    List<UserOrganization> GetDeelplatformBeheerderUsers();
    Task<bool> RemoveDpbFromOrganization(string userId, int organisationid);
    bool SearchOrganizationByName(string deelplatformName);
    List<Organization> GetOrganizations();
    bool IsUserInOrganization(string? userId, int organizationid);
    Task<bool> AddDpbToOrgByEmail(string email, int organizationid);
    Organization GetOrganizationByName(string organizationName);
    Project GetProjectWithFlowsById(int projectId);
    Theme GetThemeByOrganisationId(int organisationId);
    Theme GetThemeByInstallationId(int installationId);
    bool UpdateOrganization(int organizationId, Theme theme);
    Task<bool> AddSupervisorToOrgByEmail(string email, int organizationid);
    Task<bool> RemoveSupervisorFromOrgByEmail(string email, int organizationid);
    List<AnswerCubeUser> GetDeelplatformBeheerdersByOrgId(int organizationId);
    List<AnswerCubeUser> GetSupervisorsByOrgId(int organizationId);
    void CreateBeheerderEmail(AnswerCubeUser user, Organization organization);
}