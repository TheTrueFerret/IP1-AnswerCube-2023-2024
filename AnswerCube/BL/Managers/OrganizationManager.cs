using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.BL;

public class OrganizationManager : IOrganizationManager
{
    private readonly IOrganizationRepository _repository;
    private readonly UserManager<AnswerCubeUser>? _userManager;
    
    public OrganizationManager(IOrganizationRepository repository, UserManager<AnswerCubeUser>? userManager)
    {
        _repository = repository;
        _userManager = userManager;
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
    
    public bool IsUserInMultipleOrganizations(string userId)
    {
        return _repository.IsUserInMultipleOrganizations(userId);
    }

    public bool RemoveDeelplatformBeheerderByEmail(string userEmail, string deelplatformNaam)
    {
        return _repository.DeleteDeelplatformBeheerderByEmail(userEmail,deelplatformNaam);
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
    
    public List<Organization> GetOrganizations()
    {
        return _repository.ReadOrganizations();
    }

    public bool IsUserInOrganization(string? userId, int organizationid)
    {
        return _repository.IsUserInOrganization(userId, organizationid);
    }

    public Task<bool> AddDpbToOrgByEmail(string email, int organizationid)
    {
        return _repository.CreateDpbToOrgByEmail(email, organizationid);
    }

    public Organization GetOrganizationByName(string organizationName)
    {
        return _repository.ReadOrganizationByName(organizationName);
    }

    public Organization CreateNewOrganization(string email, string name, string? logoUrl)
    {
        return _repository.CreateNewOrganization(email, name, logoUrl); 
    }

    public async Task AddUserToOrganization(AnswerCubeUser user)
    {
        await _repository.CreateUserOrganization(user);
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

    public Task<bool> RemoveDpbFromOrganization(string userId, int organisationid)
    {
        return _repository.RemoveDpbFromOrganization(userId, organisationid);
    }

    public bool SearchOrganizationByName(string organizationName)
    {
        return _repository.SearchOrganizationByName(organizationName);
    }
    
    public Project GetProjectWithFlowsById(int projectId)
    {
        return _repository.ReadProjectWithFlowsById(projectId);
    }

    public Theme GetThemeByOrganisationId(int organisationId)
    {
        return _repository.ReadThemeByOrganisationId(organisationId);
    }
    
    public Theme GetThemeByInstallationId(int installationId)
    {
        return _repository.ReadThemeByInstallationId(installationId);
    }

    public bool UpdateOrganization(int organizationId, Theme theme)
    {
        return _repository.UpdateOrganization(organizationId, theme);
    }

    public Task<bool> AddSupervisorToOrgByEmail(string email, int organizationid)
    {
        return _repository.CreateSupervisorToOrgByEmail(email, organizationid);
    }

    public Task<bool> RemoveSupervisorFromOrgByEmail(string email, int organizationid)
    {
        return _repository.RemoveSupervisorFromOrgByEmail(email, organizationid);
    }

    public List<AnswerCubeUser> GetDeelplatformBeheerdersByOrgId(int organizationId)
    {
        return _repository.ReadDeelplatformBeheedersByOrgId(organizationId);
    }

    public List<AnswerCubeUser> GetSupervisorsByOrgId(int organizationId)
    {
        return _repository.ReadSupervisorsByOrgId(organizationId);
    }

    public void CreateBeheerderEmail(AnswerCubeUser user, Organization organization)
    {
        _repository.CreateBeheerderEmail(user, organization);
    }
}