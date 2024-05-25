using AnswerCube.BL;
using AnswerCube.BL.Domain.Project;
using AnswerCube.UI.MVC.Services;
using ASP;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers.Api
{
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
        
        [HttpGet]
        public ActionResult<Theme> GetTheme()
        {
            Theme theme;

            var controller = ControllerContext.ActionDescriptor.ControllerName;
            if (controller == "CircularFlow" || controller == "LinearFlow")
            {
                string token = Request.Cookies["jwtToken"];
                int installationId = _jwtService.GetInstallationIdFromToken(token);
                theme = _organizationManager.GetThemeByInstallationId(installationId);
            }
            if (controller == "Organization" )
            {
                string OrganizationId = Request.Cookies["OrganizationId"];
                theme = _organizationManager.GetThemeByOrganizationId(OrganizationId);
            }
            // in de repository method .include(flow).theninclude(project);
            
            if (theme == null)
            {
                return NotFound("Theme not found");
            }

            return Ok(theme);
        }
    }
}