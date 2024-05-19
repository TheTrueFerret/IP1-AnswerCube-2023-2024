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
            if (project == null)
            {
                return View("Error");
            }

            var organization = _manager.GetOrganizationById(organizationid);
            return View(new ProjectOrganizationDTO
            {
                Project = project,
                Organization = organization
            });
        }

        public IActionResult NewProject(int organizationId)
        {
            Organization organization = _manager.GetOrganizationById(organizationId);
            if (organization != null)
            {
                return View(organization);
            }
            else
            {
                return View("Error");
            }
        }

        public async Task<IActionResult> CreateProject(int organizationId, string title, string description,
            bool isActive)
        {
            Project project = await _manager.CreateProject(organizationId, title, description, isActive);
            if (project != null)
            {
                return RedirectToAction("Project", new
                {
                    projectId = project.Id,
                    organizationid = organizationId
                });
            }
            else
            {
                return View("Error");
            }
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

        public IActionResult EditProject(int projectId)
        {
            Project project = _manager.GetProjectById(projectId);
            if (project == null)
            {
                return View("Error");
            }

            return View(project);
        }

        public async Task<IActionResult> SaveProjectChanges(Project updatedProject, int projectId)
        {
            updatedProject.Id = projectId;

            if (await _manager.UpdateProject(updatedProject))
            {
                var project = _manager.GetProjectById(projectId);
                return RedirectToAction("Project",
                    new { projectid = project.Id, organizationId = project.Organization.Id });
            }

            return View("Error");
        }

        public IActionResult Flows(int projectId)
        {
            Project project = _manager.GetProjectWithFlowsById(projectId);
            if (project == null)
            {
                return View("Error");
            }

            return View(project);
        }

        public IActionResult NewFlowView(int projectId)
        {
            //This goes to the NewFlowView
            Project project = _manager.GetProjectById(projectId);
            return View(project);
        }
    }
}