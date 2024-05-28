using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Project;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AnswerCube.DAL.EF;

public class OrganizationRepository : IOrganizationRepository
{
    private readonly AnswerCubeDbContext _context;
    private readonly UserManager<AnswerCubeUser> _userManager;
    private readonly IMailRepository _mailRepository;

    public OrganizationRepository(AnswerCubeDbContext context, UserManager<AnswerCubeUser> userManager,
        IMailRepository mailRepository)
    {
        _context = context;
        _userManager = userManager;
        _mailRepository = mailRepository;
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
        DeelplatformbeheerderEmail deelplatformbeheerderEmail = new DeelplatformbeheerderEmail
            { Email = userEmail, IsDeelplatformBeheerder = true };
        _context.DeelplatformbeheerderEmails.Add(deelplatformbeheerderEmail);
        return true;
    }

    public bool IsUserInMultipleOrganizations(string userId)
    {
        return _context.UserOrganizations.Count(uo => uo.UserId == userId) > 1;
    }

    public bool DeleteDeelplatformBeheerderByEmail(string userEmail, string deelplatformNaam)
    {
        var organization = _context.Organizations.Include(o => o.UserOrganizations)
            .SingleOrDefault(o => o.Name.ToLower() == deelplatformNaam.ToLower());
        var user = _context.Users.First(u => u.Email == userEmail);
        DeelplatformbeheerderEmail? deelplatformbeheerderEmail =
            _context.DeelplatformbeheerderEmails.SingleOrDefault(d =>
                d.Email.ToLower() == userEmail.ToLower() && d.DeelplatformNaam.ToLower() == deelplatformNaam.ToLower());
        if (deelplatformbeheerderEmail == null || deelplatformbeheerderEmail.Email != userEmail)
        {
            return false;
        }

        _context.DeelplatformbeheerderEmails.Remove(deelplatformbeheerderEmail);
        var uo = _context.UserOrganizations.Single(uo => uo.UserId == user.Id && uo.OrganizationId == organization.Id);
        _context.UserOrganizations.Remove(uo);
        //Check if any other deelplatformbeheerders are assigned to the organization, if not delete the organization.
        if (_context.Organizations.Single(organization => organization.Id == uo.OrganizationId).UserOrganizations
                .Count == 0)
        {
            _context.Organizations.Remove(organization);
            _context.Forums.Remove(_context.Forums.First(f => f.OrganizationId == organization.Id));
        }
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
        return project;
    }

    public async Task<bool> UpdateProject(Project project)
    {
        // Fetch the existing project from the database
        var existingProject = _context.Projects.FirstOrDefault(p => p.Id == project.Id);

        if (existingProject == null)
        {
            // Handle the case where the project does not exist
            return false;
        }

        // Update the specific properties
        existingProject.Title = project.Title;
        existingProject.Description = project.Description;
        existingProject.IsActive = project.IsActive;

        // Save the changes
        _context.Projects.Update(existingProject);

        return true;
    }

    public Project ReadProjectWithFlowsById(int projectId)
    {
        return _context.Projects.Include(p => p.Flows)
            .Include(p => p.Organization)
            .FirstOrDefault(p => p.Id == projectId);
    }
    

    public Organization CreateNewOrganization(string email, string name, string? logoUrl)
    {
        Organization organization = new Organization(name, email, logoUrl);
        _context.Organizations.Add(organization);
        return organization;
    }

    public void SaveBeheerderAndOrganization(string email, string organizationName)
    {
        _context.DeelplatformbeheerderEmails.Add(new DeelplatformbeheerderEmail
            { Email = email, DeelplatformNaam = organizationName, IsDeelplatformBeheerder = true });
        _mailRepository.SendNewEmail(email, organizationName);
    }

    public bool CreateUserOrganization(AnswerCubeUser user)
    {
        Organization? organization = _context.Organizations.FirstOrDefault(o => o.Name == _context
            .DeelplatformbeheerderEmails
            .First(d => d.Email == user.Email).DeelplatformNaam);
        if (_context.DeelplatformbeheerderEmails.First(d => d.Email == user.Email).IsDeelplatformBeheerder)
        {
            _userManager.AddToRoleAsync(user, "DeelplatformBeheerder");
        }
        else
        {
            _userManager.AddToRoleAsync(user, "Supervisor");
        }

        if (organization != null)
        {
            _context.UserOrganizations.Add(new UserOrganization()
            {
                Organization = organization,
                OrganizationId = organization.Id,
                User = user,
                UserId = user.Id
            });
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
    }

    public async Task<bool> RemoveDpbFromOrganization(string userId, int organizationid)
    {
        //remove the UserOrganization from the user, than remove him from beheerderEmails
        var userOrganization =
            _context.UserOrganizations.FirstOrDefault(uo => uo.UserId == userId && uo.OrganizationId == organizationid);
        if (userOrganization == null)
        {
            return false;
        }

        _context.UserOrganizations.Remove(userOrganization);
        var user = _context.Users.SingleOrDefault(u => u.Id == userId);
        if (user != null)
        {
            _userManager.RemoveFromRoleAsync(user, "DeelplatformBeheerder");
        }
        
        //Check if any other deelplatformbeheerders are assigned to the organization, if not delete the organization.
        var org = _context.Organizations.Include(o => o.UserOrganizations).Include(o => o.Projects)
            .ThenInclude(p => p.Flows).Single(organization => organization.Id == organizationid);
        if (org.UserOrganizations.Count == 0)
        {
            RemoveEmptyOrganization(org);
        }

        return true;
    }

    public void RemoveEmptyOrganization(Organization organization)
    {
        if (organization == null)
        {
            // Handle the case where the organization does not exist
            return;
        }

        // Remove all related Flows
        foreach (var project in organization.Projects)
        {
            _context.Flows.RemoveRange(project.Flows);
        }

        // Remove all related Projects
        _context.Projects.RemoveRange(organization.Projects);

        // Remove the Organization
        _context.Organizations.Remove(organization);
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

    public async Task<bool> CreateDpbToOrgByEmail(string email, int organizationid)
    {
        AnswerCubeUser user = _context.Users.SingleOrDefault(u => u.Email == email);
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
                await _userManager.AddToRoleAsync(user, "DeelplatformBeheerder");
                await _mailRepository.SendExistingEmail(email,
                    _context.Organizations.Single(o => o.Id == organizationid).Name);
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

    public Theme ReadThemeByOrganisationId(int organisationId)
    {
        return _context.Organizations.First(o => o.Id == organisationId).Theme;
    }
    
    public Theme ReadThemeByInstallationId(int installationId)
    {
        Installation installation = _context.Installations
            .Where(i => i.Id == installationId)
            .Include(i => i.Organization) // Laad de organisatie in
            .First(); // Voeg deze toe om de installatie op te halen

        if (installation != null)
        {
            return installation.Organization.Theme;
        }
        
        return Theme.Light; 
    }

    public bool UpdateOrganization(int organizationId, Theme theme)
    {
        Organization organization = _context.Organizations
            .Single(o => o.Id == organizationId);

        organization.Theme = theme;
        _context.Organizations.Update(organization);
        return true;
    }

    public async Task<bool> CreateSupervisorToOrgByEmail(string email, int organizationid)
    {
        AnswerCubeUser user = _context.Users.SingleOrDefault(u => u.Email == email);
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
                await _userManager.AddToRoleAsync(user, "Supervisor");
                await _mailRepository.SendExistingSupervisorEmail(email,
                    _context.Organizations.Single(o => o.Id == organizationid).Name);
            }
            return true;
        }

        //The user doesnt exist so we need to send email to register.
        SaveSupervisorAndOrganization(email, _context.Organizations.Single(o => o.Id == organizationid).Name);
        return true;
    }

    public async Task<bool> RemoveSupervisorFromOrgByEmail(string email, int organizationid)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == email);
        //remove the UserOrganization from the user, than remove him from beheerderEmails
        var userOrganization =
            _context.UserOrganizations.FirstOrDefault(uo =>
                uo.UserId == user.Id && uo.OrganizationId == organizationid);
        if (userOrganization == null)
        {
            return false;
        }

        _context.UserOrganizations.Remove(userOrganization);
        await _userManager.RemoveFromRoleAsync(_context.Users.Single(u => u.Id == user.Id), "Supervisor");

        return true;
    }

    public List<AnswerCubeUser> ReadSupervisorsByOrgId(int organizationId)
    {
        var usersInOrg = _context.UserOrganizations.Include(uo => uo.User)
            .Where(uo => uo.OrganizationId == organizationId)
            .Select(uo => uo.User)
            .ToList();

        return usersInOrg.FindAll(u => _userManager.IsInRoleAsync(u, "Supervisor").Result);
    }

    public List<AnswerCubeUser> ReadDeelplatformBeheedersByOrgId(int organizationId)
    {
        var usersInOrg = _context.UserOrganizations.Include(uo => uo.User)
            .Where(uo => uo.OrganizationId == organizationId)
            .Select(uo => uo.User)
            .ToList();

        return usersInOrg.FindAll(u => _userManager.IsInRoleAsync(u, "DeelplatformBeheerder").Result);
    }

    private void SaveSupervisorAndOrganization(string email, string name)
    {
        _context.DeelplatformbeheerderEmails.Add(new DeelplatformbeheerderEmail
            { Email = email, DeelplatformNaam = name, IsDeelplatformBeheerder = false });
        _mailRepository.SendNewSupervisorEmail(email, name);
    }
}