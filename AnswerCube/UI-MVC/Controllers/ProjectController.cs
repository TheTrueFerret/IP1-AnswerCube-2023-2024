using AnswerCube.BL;
using Microsoft.AspNetCore.Mvc;
using AnswerCube.BL.Domain.Project;
using AnswerCube.UI.MVC.Models.Dto;
using Domain;

namespace AnswerCube.UI.MVC.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IOrganizationManager manager, ILogger<ProjectController> logger)
        {
            _organizationManager = manager;
            _logger = logger;
        }

        public IActionResult Project(int projectid, int organizationid)
        {
            var project = _organizationManager.GetProjectById(projectid);
            if (project == null)
            {
                return View("Error");
            }

            var organization = _organizationManager.GetOrganizationById(organizationid);
            return View(new ProjectOrganizationDTO
            {
                Project = project,
                Organization = organization
            });
        }

        public IActionResult NewProject(int organizationId)
        {
            Organization organization = _organizationManager.GetOrganizationById(organizationId);
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
            Project project = await _organizationManager.CreateProject(organizationId, title, description, isActive);
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
            if (_organizationManager.DeleteProject(projectId))
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
            Project project = _organizationManager.GetProjectById(projectId);
            
            if (project == null)
            {
                return View("Error");
            }

            return View(project);
        }
        
        public async Task<IActionResult> SaveProjectChanges(Project updatedProject, int projectId)
        { //hier da thema meegeven
            updatedProject.Id = projectId;

            if (await _organizationManager.UpdateProject(updatedProject))
            {
                var project = _organizationManager.GetProjectById(projectId);
                return RedirectToAction("Project",
                    new { projectid = project.Id, organizationId = project.Organization.Id });
            }

            return View("Error");
        }
        

        public IActionResult Flows(int projectId)
        {
            Project project = _organizationManager.GetProjectWithFlowsById(projectId);
            if (project == null)
            {
                return View("Error");
            }

            return View(project);
        }

        public IActionResult NewFlowView(int projectId)
        {
            //This goes to the NewFlowView
            Project project = _organizationManager.GetProjectById(projectId);
            return View(project);
        }
    }
}