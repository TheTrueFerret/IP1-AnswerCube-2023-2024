using AnswerCube.BL;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers.Api;

[ApiController]
[Route("api/[controller]")] 
public class BeheerderController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<BeheerderController> _logger;

    public BeheerderController(IManager manager, ILogger<BeheerderController> logger)
    {
        _manager = manager;
        _logger = logger;
    }
    
    public IActionResult Flows()
    {
        return View(this);
    }
}