using System.Security.Claims;
using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL.EF;
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
    private readonly UnitOfWork _uow;


    public OrganizationController(IOrganizationManager manager, ILogger<OrganizationController> logger,
        UserManager<AnswerCubeUser> userManager, UnitOfWork uow)
    {
        _organizationManager = manager;
        _logger = logger;
        _userManager = userManager;
        _uow = uow;
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
                var deelplatformbeheerders = _organizationManager.GetDeelplatformBeheerdersByOrgId(organization.Id);
                var supervisors = _organizationManager.GetSupervisorsByOrgId(organization.Id);
                ViewBag.Deelplatformbeheeders = deelplatformbeheerders;
                ViewBag.Supervisors = supervisors;
                TempData["OrganizationLogo"] = organization.LogoUrl;
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
                var deelplatformbeheerders = _organizationManager.GetDeelplatformBeheerdersByOrgId(organizations[0].Id);
                var supervisors = _organizationManager.GetSupervisorsByOrgId(organizations[0].Id);
                ViewBag.Deelplatformbeheeders = deelplatformbeheerders;
                ViewBag.Supervisors = supervisors;
                TempData["OrganizationLogo"] = organizations[0].LogoUrl;
                return View(organizations[0]);
            }

            return Forbid();
        }

        return NotFound();
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
            var deelplatformbeheerders = _organizationManager.GetDeelplatformBeheerdersByOrgId(organization.Id);
            var supervisors = _organizationManager.GetSupervisorsByOrgId(organization.Id);
            ViewBag.Deelplatformbeheeders = deelplatformbeheerders;
            ViewBag.Supervisors = supervisors;
            TempData["OrganizationLogo"] = organization.LogoUrl;
            return View("Index", organization);
        }

        if (_organizationManager.IsUserInOrganization(User.FindFirstValue(ClaimTypes.NameIdentifier), organizationid))
        {
            var deelplatformbeheerders = _organizationManager.GetDeelplatformBeheerdersByOrgId(organization.Id);
            var supervisors = _organizationManager.GetSupervisorsByOrgId(organization.Id);
            ViewBag.Deelplatformbeheeders = deelplatformbeheerders;
            ViewBag.Supervisors = supervisors;
            TempData["OrganizationLogo"] = organization.LogoUrl;
            return View("Index", organization);
        }

        return Forbid();
    }

    public async Task<IActionResult> AddDeelplatformbeheerderToOrganization(string email, int organizationid)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        //If user isnt admin or not in the organizatie, kan die niet toevoegen
            if (_organizationManager.IsUserInOrganization(user.Id, organizationid) &&
                User.IsInRole("DeelplatformBeheerder"))
            {
                if (_organizationManager.IsUserInOrganization(email, organizationid))
                {
                    // The user is already part of the organization, return an appropriate response
                    TempData["Error"] = $"User {email} is already part of the organization";
                    return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
                }
                _uow.BeginTransaction();
                if (_organizationManager.AddDpbToOrgByEmail(email, organizationid).Result)
                {
                    _uow.Commit();
                    TempData["Succes"] = $"User {email} is added to the organization";
                    return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
                }
            }
            else
            {
                return Forbid(); // or return to an error page
            }

        return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
    }
    
    public async Task<IActionResult> RemoveDeelplatformbeheeder(string userId, int organizationid)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        // If user isnt in the organizatie, kan die niet toevoegen
            if (_organizationManager.IsUserInOrganization(user.Id, organizationid) &&
                User.IsInRole("DeelplatformBeheerder"))
            {
                _uow.BeginTransaction();
                if (_organizationManager.RemoveDpbFromOrganization(userId, organizationid).Result)
                {
                    _uow.Commit();
                    return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
                }

                return View("Error");
            }

            return Forbid(); // or return to an error page
    }


    public async Task<IActionResult> AddSupervisor(string email, int organizationid)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            if (_organizationManager.IsUserInOrganization(user.Id, organizationid))
            {
                if (_organizationManager.IsUserInOrganization(email, organizationid))
                {
                    // The user is already part of the organization, return an appropriate response
                    TempData["SupervisorError"] = $"User {email} is already part of the organization";
                    return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
                }
                _uow.BeginTransaction();
                if (_organizationManager.AddSupervisorToOrgByEmail(email, organizationid).Result)
                {
                    _uow.Commit();
                    TempData["SupervisorSuccess"] = $"User {email} is added to the organization";
                    return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
                }
            }
            else
            {
                return Forbid(); // or return to an error page
            }

        return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
    }
    
    public async Task<IActionResult> RemoveSupervisor(string email, int organizationid)
    {
        var user = await _userManager.FindByIdAsync(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (_organizationManager.IsUserInOrganization(user.Id, organizationid))
        {
            if (_organizationManager.IsUserInOrganization(email, organizationid))
            {
                // The user is already part of the organization, return an appropriate response
                ViewBag.Error = $"User {email} is already part of the organization";
                return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
            }
            _uow.BeginTransaction();
            if (_organizationManager.RemoveSupervisorFromOrgByEmail(email, organizationid).Result)
            {
                _uow.Commit();
                ViewBag.Success = $"User {email} is added to the organization";
                return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
            }
        }
        else
        {
            return Forbid(); // or return to an error page
        }

        return RedirectToAction("Index", "Organization", new { organizationId = organizationid });
    }
    
    public IActionResult UpdateTheme(int organizationId, Theme theme)
    {
        _uow.BeginTransaction();
        bool result = _organizationManager.UpdateOrganization(organizationId, theme);
        _uow.Commit();
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