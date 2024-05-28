using AnswerCube.BL;
using AnswerCube.BL.Domain;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UI_MVC.Models.Dto;

namespace AnswerCube.UI.MVC.Controllers.Api;

[ApiController]
[Route("/api/[controller]")]
public class SlidesController : ControllerBase
{
    private readonly IFlowManager _flowManager;

    public SlidesController(IFlowManager manager)
    {
        _flowManager = manager;
    }
    
    [Route("GetSlideData/{id}")]
    [HttpGet]
    public IActionResult GetSlideData(int id)
    {
        Slide slide = _flowManager.GetSlideById(id);
        return new JsonResult(slide);
    }
    
    // generate content in the slides
    // and posts
    // deze krijgen allemaal een ID!!!!
    
    
}