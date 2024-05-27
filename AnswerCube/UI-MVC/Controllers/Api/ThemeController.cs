using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.User;
using AnswerCube.UI.MVC.Services;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers.Api;

    [Route("api/[controller]")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly UserManager<AnswerCubeUser> _userManager;
        private readonly JwtService _jwtService;
        
        public ThemeController(IOrganizationManager manager, UserManager<AnswerCubeUser> userManager, JwtService jwtService)
        {
            _organizationManager = manager;
            _userManager = userManager;
            _jwtService = jwtService;
        }
        
        //GetThemeByInstallationId
        // inside site.ts an if statement or case to check in which controller we are
        // if inside CircularFlow Controller || LinearFlowController GetThemeByInstallationId
        
        [Route("/api/[controller]/GetTheme/{controllerName}")]
        [HttpGet]
        public IActionResult GetTheme(string controllerName)
        {
            Theme? theme = null;

            if (controllerName == "CircularFlow" || controllerName == "LinearFlow")
            {
                string token = Request.Cookies["jwtToken"];
                int installationId = _jwtService.GetInstallationIdFromToken(token);
                theme = _organizationManager.GetThemeByInstallationId(installationId);
            }
            else if (controllerName == "Organization")
            {
                string organizationIdStr = Request.Cookies["OrganizationId"];
                if (int.TryParse(organizationIdStr, out int organizationId))
                {
                    theme = _organizationManager.GetThemeByOrganisationId(organizationId);
                }
                else
                {
                    return BadRequest("Invalid OrganizationId cookie value");
                }
            }
            else
            {
                Organization organization = _organizationManager.GetOrganizationByUserId(_userManager.GetUserId(User)).First();
                theme = _organizationManager.GetThemeByOrganisationId(organization.Id);
            }
            
            return Ok(theme.ToString());
        }

    }
