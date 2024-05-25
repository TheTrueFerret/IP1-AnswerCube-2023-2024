using AnswerCube.BL;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThemeController : ControllerBase
    {
        private readonly IOrganizationManager _organizationManager;

        public ThemeController(IOrganizationManager manager)
        {
            _organizationManager = manager;
        }

        [HttpGet("{id}")]
        public ActionResult<string> GetProjectTheme(int id)
        {
            var theme = _organizationManager.GetProjectThemeByProjectId(id);
            if (theme == null)
            {
                return NotFound("Theme not found");
            }

            return Ok(theme);
        }
    }
}