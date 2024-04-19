using AnswerCube.BL;
using AnswerCube.BL.Domain;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UI_MVC.Models.Dto;

namespace UI_MVC.Controllers.Api;

[ApiController]
[Route("/api/[controller]")]
public class SlidesController : ControllerBase
{
    private readonly IManager _manager;

    public SlidesController(IManager manager)
    {
        _manager = manager;
    }
    
    
    [Route("GetSlideData/{id}")]
    [HttpGet]
    public IActionResult GetSlideData(int id)
    {
        Slide slide = _manager.GetSlideById(id);
        return new JsonResult(slide);
    }
    
    // generate content in the slides
    // and posts
    // deze krijgen allemaal een ID!!!!
    
    
}