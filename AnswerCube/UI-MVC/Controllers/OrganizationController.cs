using AnswerCube.BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

[Authorize(Roles = "Admin,DeelplatformBeheerder")]
public class OrganizationController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<OrganizationController> _logger;

    public OrganizationController(IManager manager, ILogger<OrganizationController> logger)
    {
        _manager = manager;
        _logger = logger;
    }

    public IActionResult Index(string? userId, int? organizationId)
    {
        if (organizationId.HasValue)
        {
            var organization = _manager.GetOrganizationById(organizationId.Value);
            if (organization != null)
            {
                return View(organization);
            }
        }
        else
        {
            var organizations = _manager.GetOrganizationByUserId(userId);
            if (organizations.Count > 1)
            {
                return View("SelectOrganization", organizations);
            }
            else if (organizations.Count == 1)
            {
                return View(organizations[0]);
            }
            else
            {
                return RedirectToPage("AccessDenied", new { area = "Identity" });
            }
        }

        return NotFound();
    }
}