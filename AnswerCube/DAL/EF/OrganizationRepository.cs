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
    
    public OrganizationRepository(AnswerCubeDbContext context, UserManager<AnswerCubeUser> userManager)
    {
        _context = context;
        _userManager = userManager;
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
        _context.SaveChanges();
        //Check if any other deelplatformbeheerders are assigned to the organization, if not delete the organization.
        if (_context.Organizations.Single(organization => organization.Id == uo.OrganizationId).UserOrganizations
                .Count == 0)
        {
            _context.Organizations.Remove(organization);
            _context.Forums.Remove(_context.Forums.First(f => f.OrganizationId == organization.Id));
        }


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
    

    public Organization CreateNewOrganization(string email, string name, string? logoUrl)
    {
        Organization organization = new Organization(name, email,logoUrl);
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

    public bool RemoveDpbFromOrganization(string userId, int organisationid)
    {
        UserOrganization userOrganization = _context.UserOrganizations.Include(uo => uo.User)
            .Include(uo => uo.Organization)
            .First(uo => uo.UserId == userId && uo.OrganizationId == organisationid);
        if (userOrganization == null)
        {
            return false;
        }

        DeleteDeelplatformBeheerderByEmail(userOrganization.User.Email, userOrganization.Organization.Name);
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
                await _userManager.AddToRoleAsync(user, "DeelplatformBeheerder");
                _context.SaveChanges();
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
        
        return Theme.LightTheme; 
    }
    
}