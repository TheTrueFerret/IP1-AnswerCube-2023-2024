using AnswerCube.BL;
using Microsoft.AspNetCore.Mvc;
using AnswerCube.BL.Domain.Project;
using AnswerCube.DAL.EF;
using AnswerCube.UI.MVC.Models.Dto;
using Domain;
using Microsoft.AspNetCore.Authorization;

namespace AnswerCube.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin,DeelplatformBeheerder")]
    public class ProjectController : Controller
    {
        private readonly IOrganizationManager _organizationManager;
        private readonly ILogger<ProjectController> _logger;
        private readonly UnitOfWork _uow;


        public ProjectController(IOrganizationManager manager, ILogger<ProjectController> logger, UnitOfWork uow)
        {
            _organizationManager = manager;
            _logger = logger;
            _uow = uow;
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

        public async Task<IActionResult> CreateProject(int organizationId, string title, string description, bool isActive)
        {
            _uow.BeginTransaction();
            Project project = await _organizationManager.CreateProject(organizationId, title, description, isActive);
            _uow.Commit();
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
            _uow.BeginTransaction();
            if (_organizationManager.DeleteProject(projectId))
            {
                _uow.Commit();
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
        {
            //hier da thema meegeven
            updatedProject.Id = projectId;

            _uow.BeginTransaction();
            if (await _organizationManager.UpdateProject(updatedProject))
            {
                _uow.Commit();
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