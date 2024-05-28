using System.Security.Claims;
using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL.EF;
using AnswerCube.UI.MVC.Areas.Identity.Data;
using AnswerCube.UI.MVC.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnswerCube.UI.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : BaseController
{
    private readonly ILogger<AdminController> _logger;
    private readonly UserManager<AnswerCubeUser> _userManager;
    private readonly SignInManager<AnswerCubeUser> _signInManager;
    private readonly IMailManager _mailManager;
    private readonly IOrganizationManager _organizationManager;
    private readonly CloudStorageService _cloudStorageService;
    private readonly UnitOfWork _uow; 
    
    public AdminController(
        ILogger<AdminController> logger, 
        UserManager<AnswerCubeUser> userManager, 
        SignInManager<AnswerCubeUser> signInManager, 
        IMailManager mailManager, 
        IOrganizationManager organizationManager,
        CloudStorageService cloudStorageService,
        UnitOfWork uow)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _mailManager = mailManager;
        _organizationManager = organizationManager;
        _cloudStorageService = cloudStorageService;
        _uow = uow;
    }

    public async Task<IActionResult> DeelplatformOverview()
    {
        var userOrganizations = _organizationManager.GetDeelplatformBeheerderUsers();
        ViewBag.HasCredential = _cloudStorageService.hasCredential;
        return View(userOrganizations);
    }

    public async Task<IActionResult> Users()
    {
        var usersRoleDto = new UserRolesDto(_organizationManager.GetAllUsers(), new Dictionary<string, List<string>>());
        foreach (var users in usersRoleDto.Users)
        {
            var roles = await _userManager.GetRolesAsync(users);
            usersRoleDto.Roles.Add(users.Id, roles.ToList());
        }

        return View(usersRoleDto);
    }

    [HttpGet("User/Role/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Role(string id)
    {
        foreach (var user in _userManager.Users.ToList())
        {
            var allAvailableRoles = _organizationManager.GetAllAvailableRoles(user);
            if (user.Id.Equals(id))
            {
                var roles = _userManager.GetRolesAsync(user).Result.ToList();

                UserRoleDto newUser;
                if (user is AnswerCubeUser answercubeUser)
                {
                    newUser = new UserRoleDto()
                    {
                        Name = answercubeUser.FirstName,
                        Id = user.Id,
                        LastName = answercubeUser.LastName,
                        Roles = roles,
                        SelectedRole = "none",
                        SelectedRoleToRemove = "none",
                        AllAvailableIdentityRoles = allAvailableRoles
                    };
                }
                else
                {
                    newUser = new UserRoleDto()
                    {
                        Name = user.FirstName,
                        Id = user.Id,
                        Roles = roles,
                        SelectedRole = "none",
                        AllAvailableIdentityRoles = allAvailableRoles,
                    };
                }
                return View(newUser);
            }
        }
        return RedirectToPage("User");
    }

    [HttpPost("AssignRole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AssignRole(UserRoleDto model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);

        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.AddToRoleAsync(user, model.SelectedRole);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Unable to assign role.");
        }

        await _signInManager.RefreshSignInAsync(_userManager.GetUserAsync(User).Result);
        return RedirectToAction("Role", new { id = model.Id });
    }

    [HttpPost("RemoveRole")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveRole(UserRoleDto model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);

        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.RemoveFromRoleAsync(user, model.SelectedRoleToRemove);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Unable to remove role.");
        }
        await _signInManager.RefreshSignInAsync(_userManager.GetUserAsync(User).Result);
        return RedirectToAction("Role", new { id = model.Id });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("Delete/{id}")]
    public IActionResult Delete(string id)
    {
        var user = _userManager.FindByIdAsync(id).Result;
        if (user == null)
        {
            return NotFound();
        }

        if (user.Id == User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
        {
            TempData["ErrorOwnAccountDelete"] = "Je kan jezelf niet verwijderen.";
            return RedirectToAction("Users", "Admin");
        }


        var result = _userManager.DeleteAsync(user).Result;
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Unable to delete developer.");
        }

        return RedirectToAction("Users");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("AddDeelplatform")]
    public async Task<IActionResult> AddDeelplatform(string email, string deelplatformName,IFormFile? logo)
    {
        string? logoUrl;
        if (logo != null)
        {
            logoUrl= _cloudStorageService.UploadFileToBucket(logo);
        }
        else
        {
            logoUrl = null;
        }
        _uow.BeginTransaction();
        Organization organization = _organizationManager.CreateNewOrganization(email, deelplatformName,logoUrl);
        _uow.Commit();
        _uow.BeginTransaction();
        _organizationManager.SaveBeheerderAndOrganization(email, organization);
        _uow.Commit();
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            await _mailManager.SendNewEmail(email, organization.Name);
            TempData["Success"] = "An email has been sent to the provided email address.";
        }
        else
        {
            await _userManager.AddToRoleAsync(user, "DeelplatformBeheerder");
            await _mailManager.SendExistingEmail(email, organization.Name);
            TempData["Success"] =
                $"The user now has the DeelplatformBeheerder role for the {organization.Name} organization.";
            _organizationManager.CreateUserOrganization(user, organization);
        }

        _organizationManager.AddDeelplatformBeheerderByEmail(email);
        return RedirectToAction("DeelplatformOverview");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("RemoveDeelplatformBeheederRole/{id}")]
    public async Task<IActionResult> RemoveDeelplatformBeheederRole(string id, string deelplatformNaam)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (_organizationManager.IsUserInMultipleOrganizations(user.Id))
        {
            _organizationManager.RemoveDpbFromOrganization(user.Id, _organizationManager.GetOrganizationByName(deelplatformNaam).Id);
        }
        else
        {
            await _userManager.RemoveFromRoleAsync(user, "DeelplatformBeheerder");
            _organizationManager.RemoveDeelplatformBeheerderByEmail(user.Email, deelplatformNaam);
        }
        return RedirectToAction("DeelplatformOverview");
    }
}

public class UserRolesDto
{
    public List<AnswerCubeUser> Users { get; set; }
    public Dictionary<string, List<String>> Roles { get; set; }

    public UserRolesDto(List<AnswerCubeUser> users, Dictionary<string, List<String>> usersRoles)
    {
        Users = users;
        Roles = usersRoles;
    }
}