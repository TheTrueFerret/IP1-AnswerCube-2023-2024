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
    private readonly IManager _manager;
    private readonly ILogger<OrganizationController> _logger;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AnswerCubeUser> _userManager;


    public OrganizationController(IManager manager, ILogger<OrganizationController> logger, IEmailSender emailSender,
        UserManager<AnswerCubeUser> userManager)
    {
        _manager = manager;
        _logger = logger;
        _emailSender = emailSender;
        _userManager = userManager;
    }

    public IActionResult Index(string? userId, int? organizationId)
    {
        //if user is admin, he can see all organizations
        if (User.IsInRole("Admin"))
        {
            var organizations = _manager.GetOrganizations();
            return View("SelectOrganization", organizations);
        }

        if (organizationId.HasValue)
        {
            var organization = _manager.GetOrganizationById(organizationId.Value);
            if (organization != null)
            {
                return View(organization);
            }
        }
        else if (userId != null)
        {
            var organizations = _manager.GetOrganizationByUserId(userId);
            if (organizations.Count > 1)
            {
                return View("SelectOrganization", organizations);
            }

            if (organizations.Count == 1)
            {
                return View(organizations.First());
            }

            return RedirectToPage("AccessDenied", new { area = "Identity" });
        }

        return NotFound();
    }

    public IActionResult RemoveDeelplatformbeheeder(string userId, int organisationid)
    {
        if (_manager.RemoveDpbFromOrganization(userId, organisationid))
        {
            return RedirectToAction("Index", "Organization", new { organizationId = organisationid });
        }
        else
        {
            return View("Error");
        }
    }

    public IActionResult OrganizationView(int organizationid)
    {
        var organization = _manager.GetOrganizationById(organizationid);
        //Check if user is admin, to not check if user is in organization
        if (User.IsInRole("Admin"))
        {
            return View("Index", organization);
        }

        if (_manager.IsUserInOrganization(User.FindFirstValue(ClaimTypes.NameIdentifier), organizationid))
        {
            return View("Index", organization);
        }

        return RedirectToPage("/AccessDenied", new { area = "Identity" });
    }

    public async Task<IActionResult> AddDeelplatformbeheerderToOrganization(string email, int organizationid)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!_manager.IsUserInOrganization(userId, organizationid))
        {
            return Forbid(); // or return to an error page
        }

        if (_manager.IsUserInOrganization(email, organizationid))
        {
            // The user is already part of the organization, return an appropriate response
            return View("Error", new ErrorViewModel());
        }

        if (_manager.AddDpbToOrgByEmail(email, userId, organizationid).Result)
        {
            return RedirectToAction("Index", "Organization",new{organizationId = organizationid});
        }

        return View("Error", new ErrorViewModel());
    }
}