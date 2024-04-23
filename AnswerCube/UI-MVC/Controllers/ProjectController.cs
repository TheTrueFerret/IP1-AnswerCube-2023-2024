using Microsoft.AspNetCore.Mvc;
using AnswerCube.BL.Domain.Project; 
using System.Collections.Generic;

namespace AnswerCube.UI.MVC.Controllers
{
    public class ProjectController : Controller
    {
        public IActionResult Index()
        {
            // projecten Database
            var projects = new List<Project>
            {
                new Project { Title = "Project 1", Description = "Dit is project 1", IsActive = true, TotalFlows = 10, TotalActiveInstallations = 5 },
                new Project { Title = "Project 2", Description = "Dit is project 2", IsActive = false, TotalFlows = 15, TotalActiveInstallations = 8 },
                new Project { Title = "Project 3", Description = "Dit is project 3", IsActive = true, TotalFlows = 20, TotalActiveInstallations = 12 }
                // ...
            };

            return View(projects); 
        }

        public IActionResult NewProject()
        {
            return View();
        }
        
        public IActionResult Flows()
        {
            return View();
        }
        
        public IActionResult NewFlow()
        {
            return View();
        }
    }
}