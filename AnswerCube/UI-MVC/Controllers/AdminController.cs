using System.Security.Claims;
using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.UI.MVC.Areas.Identity.Data;
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
    private readonly IEmailSender _emailSender;
    private readonly IManager _manager;

    public AdminController(ILogger<AdminController> logger, UserManager<AnswerCubeUser> userManager,
        SignInManager<AnswerCubeUser> signInManager, IEmailSender emailSender, IManager manager)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _manager = manager;
    }
    
    public async Task<IActionResult> Users()
    {
        var usersRoleDto = new UserRolesDto(_manager.GetAllUsers(), new Dictionary<string, List<string>>());
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
            var allAvailableRoles = _manager.GetAllAvailableRoles(user);
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

        await _signInManager.RefreshSignInAsync(_userManager.FindByIdAsync(model.Id).Result);
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

        await _signInManager.RefreshSignInAsync(_userManager.FindByIdAsync(model.Id).Result);
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
            return RedirectToAction("User", "Home");
        }


        var result = _userManager.DeleteAsync(user).Result;
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Unable to delete developer.");
        }

        return RedirectToAction("Users");
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