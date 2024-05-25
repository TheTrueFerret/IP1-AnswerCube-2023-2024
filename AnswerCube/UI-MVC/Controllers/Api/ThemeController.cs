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
        
        [Route("GetTheme")]
        [HttpGet]
        public IActionResult GetTheme()
        {
            Theme? theme = null;

            var controller = ControllerContext.ActionDescriptor.ControllerName;
            if (controller == "CircularFlow" || controller == "LinearFlow")
            {
                string token = Request.Cookies["jwtToken"];
                int installationId = _jwtService.GetInstallationIdFromToken(token);
                theme = _organizationManager.GetThemeByInstallationId(installationId);
            }
            else if (controller == "Organization")
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
    
            if (theme == null)
            {
                return NotFound("Theme not found");
            }

            return Ok(theme);
        }

    }
