using AnswerCube.BL;
using Microsoft.AspNetCore.Mvc;
using AnswerCube.BL.Domain.Project;
using AnswerCube.UI.MVC.Models.Dto;
using Domain;

namespace AnswerCube.UI.MVC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IManager _manager;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IManager manager, ILogger<ProjectController> logger)
        {
            _manager = manager;
            _logger = logger;
        }

        public IActionResult Project(int projectid, int organizationid)
        {
            var project = _manager.GetProjectById(projectid);
            var organization = _manager.GetOrganizationById(organizationid);
            return View(new ProjectOrganizationDTO
            {
                Project = project,
                Organization = organization
            });
        }

        public IActionResult NewProject(int organizationId,string title,string description)
        {
            Project project = _manager.CreateProject(organizationId);
            Organization organization = _manager.GetOrganizationById(organizationId);
            if (project != null)
            {
                return View("Project", new ProjectOrganizationDTO
                {
                    Project = project,
                    Organization = organization
                });
            }
            else
            {
                return View("Error");
            }
        }
        
        public IActionResult CreateProject(int organizationId,string title,string description)
        {
            Project project = _manager.CreateProject(organizationId);
            Organization organization = _manager.GetOrganizationById(organizationId);
            if (project != null)
            {
                return View("Project", new ProjectOrganizationDTO
                {
                    Project = project,
                    Organization = organization
                });
            }
            else
            {
                return View("Error");
            }
        }
        

        public IActionResult Flows()
        {
            return View();
        }

        public IActionResult NewFlow()
        {
            return View();
        }


        public IActionResult DeleteProject(int projectId, int organisationId)
        {
            if (_manager.DeleteProject(projectId))
            {
                return RedirectToAction("Index", "Organization", new { organizationId = organisationId });
            }
            else
            {
                return View("Error");
            }
        }
    }
}