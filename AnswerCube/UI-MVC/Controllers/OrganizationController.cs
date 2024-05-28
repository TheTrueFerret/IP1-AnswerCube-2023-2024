using System.Security.Claims;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
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
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AnswerCubeUser> _userManager;


    public OrganizationController(IOrganizationManager manager, ILogger<OrganizationController> logger, IEmailSender emailSender,
        UserManager<AnswerCubeUser> userManager)
    {
        _organizationManager = manager;
        _logger = logger;
        _emailSender = emailSender;
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
                return View(organizations.First());
            }

            return RedirectToPage("AccessDenied", new { area = "Identity" });
        }

        return NotFound();
    }

    public IActionResult RemoveDeelplatformbeheeder(string userId, int organisationid)
    {
        if (_organizationManager.RemoveDpbFromOrganization(userId, organisationid))
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
        // Set a cookie for the organization ID
        CookieOptions option = new CookieOptions();
        Response.Cookies.Append("OrganizationId", organizationid.ToString(), option);
        
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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!_organizationManager.IsUserInOrganization(userId, organizationid))
        {
            return Forbid(); // or return to an error page
        }

        if (_organizationManager.IsUserInOrganization(email, organizationid))
        {
            // The user is already part of the organization, return an appropriate response
            return View("Error", new ErrorViewModel());
        }

        if (_organizationManager.AddDpbToOrgByEmail(email, userId, organizationid).Result)
        {
            return RedirectToAction("Index", "Organization",new{organizationId = organizationid});
        }

        return View("Error", new ErrorViewModel());
    }
    
    public IActionResult UpdateTheme(int organizationId, Theme theme)
    {
        bool result = _organizationManager.UpdateOrganization(organizationId, theme);
        if (result)
        { 
            var organization = _organizationManager.GetOrganizationById(organizationId);
            return View("Index", organization);
        } 
        var organizationWithError = _organizationManager.GetOrganizationById(organizationId);
        ViewBag.ErrorMessage = "Failed to update theme";
        return View("Index", organizationWithError);
    }
}