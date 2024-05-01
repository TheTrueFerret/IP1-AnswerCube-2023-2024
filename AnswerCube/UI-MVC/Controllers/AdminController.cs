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

    public async Task<IActionResult> DeelplatformOverview()
    {
        var users = await _userManager.Users.ToListAsync();
        users = users.Where(user =>
                _userManager.GetRolesAsync(user).Result.Any(role => role.ToLower().Equals("deelplatformbeheerder")))
            .ToList();

        var usersRoleDto = new UserRolesDto(users, new Dictionary<string, List<string>>());
        if (users.Any())
        {
            foreach (var user in usersRoleDto.Users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                usersRoleDto.Roles.Add(user.Id, roles.ToList());
            }
        }

        return View(usersRoleDto);
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

        if (model.SelectedRoleToRemove == "DeelplatformBeheerder")
        {
            _manager.RemoveDeelplatformBeheerderByEmail(user.Email);
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
    public async Task<IActionResult> AddDeelplatform([FromForm] string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            // Generate registration link with the email as a query parameter
            var registerUrl = Url.Page(
                "/Account/Register",
                pageHandler: null,
                values: new { area = "Identity", email = email },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(email, "Register for DeelplatformBeheerder",
                $"<!DOCTYPE html> <html lang='en'><head>    <meta charset=\"UTF-8\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Welcome to AnswerCube!</title>\n    <style>\n        /* Styles for the email template */\n        body {{\n            font-family: Arial, sans-serif;\n            background-color: #f4f4f4;\n            margin: 0;\n            padding: 0;\n            text-align: center;\n        }}\n\n        .container {{\n            max-width: 600px;\n            margin: 20px auto;\n            background-color: #fff;\n            padding: 20px;\n            border-radius: 8px;\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\n        }}\n\n        h1 {{\n            color: #333;\n        }}\n\n        p {{\n            color: #666;\n            margin-bottom: 20px;\n        }}\n\n        .btn {{\n            display: inline-block;\n            padding: 10px 20px;\n            background-color: #007bff;\n            color: #fff;\n            text-decoration: none;\n            border-radius: 5px;\n            transition: background-color 0.3s;\n        }}\n\n        .btn:hover {{\n            background-color: #0056b3;\n        }}\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Welcome to AnswerCube!</h1>\n        <p>You have been added as a DeelplatformBeheerder. Please register by using the button below!</p>\n        <a href=\"{registerUrl}\" class=\"btn\">Register Here</a>\n    </div>\n</body>\n</html>\n");
            TempData["Success"] = "An email has been sent to the provided email address.";
        }
        else
        {
            // Generate login link with mail
            var loginUrl = Url.Page(
                "/Account/Login",
                pageHandler: null,
                values: new { area = "Identity", email = email },
                protocol: Request.Scheme);
            await _userManager.AddToRoleAsync(user, "DeelplatformBeheerder");
            await _emailSender.SendEmailAsync(email, "You have been added as a DeelplatformBeheeder",
                $"<!DOCTYPE html> <html lang='en'><head>    <meta charset=\"UTF-8\">\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\n    <title>Role Update!</title>\n    <style>\n        /* Styles for the email template */\n        body {{\n            font-family: Arial, sans-serif;\n            background-color: #f4f4f4;\n            margin: 0;\n            padding: 0;\n            text-align: center;\n        }}\n\n        .container {{\n            max-width: 600px;\n            margin: 20px auto;\n            background-color: #fff;\n            padding: 20px;\n            border-radius: 8px;\n            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);\n        }}\n\n        h1 {{\n            color: #333;\n        }}\n\n        p {{\n            color: #666;\n            margin-bottom: 20px;\n        }}\n\n        .btn {{\n            display: inline-block;\n            padding: 10px 20px;\n            background-color: #007bff;\n            color: #fff;\n            text-decoration: none;\n            border-radius: 5px;\n            transition: background-color 0.3s;\n        }}\n\n        .btn:hover {{\n            background-color: #0056b3;\n        }}\n    </style>\n</head>\n<body>\n    <div class=\"container\">\n        <h1>Role Update!</h1>\n        <p>Your role has been updated to DeelplatformBeheerder. Please log in to your account to see the changes!</p>\n        <a href=\"{loginUrl}\" class=\"btn\">Log In Here</a>\n    </div>\n</body>\n</html>\n");
            TempData["Success"] = "The user now has the DeelplatformBeheerder role.";
        }

        _manager.AddDeelplatformBeheerderByEmail(email);
        return RedirectToAction("DeelplatformOverview");
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("RemoveDeelplatformBeheederRole/{id}")]
    public async Task<IActionResult> RemoveDeelplatformBeheederRole(string id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        await _userManager.RemoveFromRoleAsync(user, "DeelplatformBeheerder");
        _manager.RemoveDeelplatformBeheerderByEmail(user.Email);

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