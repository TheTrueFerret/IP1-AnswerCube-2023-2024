using AnswerCube.BL;
using AnswerCube.BL.Domain;
using AnswerCube.UI.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers.Api;

    [Route("api/[controller]")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly JwtService _jwtService;
        
        public ThemeController(IOrganizationManager manager, JwtService jwtService)
        {
            _organizationManager = manager;
            _jwtService = jwtService;
        }
        
        //GetThemeByInstallationId
        // inside site.ts an if statement or case to check in which controller we are
        // if inside CircularFlow Controller || LinearFlowController GetThemeByInstallationId
        
        [Route("GetTheme/{controllerName}")]
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
    
            /*if (theme == null)
            {
                return NotFound("Theme not found");
            }*/

            return Ok(theme);
        }

    }
