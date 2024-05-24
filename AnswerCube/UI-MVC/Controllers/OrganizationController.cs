using System.Security.Claims;
using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.UI.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

[Authorize(Roles = "Admin,DeelplatformBeheerder")]
public class OrganizationController : BaseController
{
    private readonly IOrganizationManager _organizationManager;
    private readonly ILogger<OrganizationController> _logger;
    private readonly UserManager<AnswerCubeUser> _userManager;


    public OrganizationController(IOrganizationManager manager, ILogger<OrganizationController> logger,
        UserManager<AnswerCubeUser> userManager)
    {
        _organizationManager = manager;
        _logger = logger;
        _userManager = userManager;
    }

    public IActionResult Index(string? userId, int? organizationId)
    {
        //if user is admin, he can see all organizations
        if (User.IsInRole("Admin"))
        {
            var organizations = _organizationManager.GetOrganizations();
            return View("SelectOrganization", organizations);
        }

        if (organizationId.HasValue)
        {
            var organization = _organizationManager.GetOrganizationById(organizationId.Value);
            if (organization != null)
            {
                return View(organization);
            }
        }
        else if (userId != null)
        {
            var organizations = _organizationManager.GetOrganizationByUserId(userId);
            if (organizations.Count > 1)
            {
                return View("SelectOrganization", organizations);
            }

            if (organizations.Count == 1)
            {
                return View(organizations[0]);
            }

            return RedirectToPage("AccessDenied", new { area = "Identity" });
        }

        return NotFound();
    }

    public async Task<IActionResult> RemoveDeelplatformbeheeder(string userId, int organizationid)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        //If user isnt admin or not in the organizatie, kan die niet toevoegen
        if (!User.IsInRole("Admin"))
        {
            if (_organizationManager.IsUserInOrganization(user.Id, organizationid) &&
                User.IsInRole("DeelplatformBeheerder"))
            {
                if (_organizationManager.RemoveDpbFromOrganization(userId, organizationid))
                {
                    return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
                }

                return View("Error");
            }

            return Forbid(); // or return to an error page
        }

        return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
    }

    public IActionResult OrganizationView(int organizationid)
    {
        var organization = _organizationManager.GetOrganizationById(organizationid);
        //Check if user is admin, to not check if user is in organization
        if (User.IsInRole("Admin"))
        {
            return View("Index", organization);
        }

        if (_organizationManager.IsUserInOrganization(User.FindFirstValue(ClaimTypes.NameIdentifier), organizationid))
        {
            return View("Index", organization);
        }

        return RedirectToPage("/AccessDenied", new { area = "Identity" });
    }

    public async Task<IActionResult> AddDeelplatformbeheerderToOrganization(string email, int organizationid)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        //If user isnt admin or not in the organizatie, kan die niet toevoegen
        if (!User.IsInRole("Admin"))
        {
            if (_organizationManager.IsUserInOrganization(user.Id, organizationid) &&
                User.IsInRole("DeelplatformBeheerder"))
            {
                if (_organizationManager.IsUserInOrganization(email, organizationid))
                {
                    // The user is already part of the organization, return an appropriate response
                    ViewBag.Error = "User is already part of the organization";
                    return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
                }

                if (_organizationManager.AddDpbToOrgByEmail(email, organizationid).Result)
                {
                    ViewBag.Success = $"User {email} is added to the organization";
                    return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
                }
            }
            else
            {
                return Forbid(); // or return to an error page
            }
        }

        return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
    }
}