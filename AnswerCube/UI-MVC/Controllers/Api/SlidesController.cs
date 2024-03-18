using AnswerCube.BL;
using AnswerCube.BL.Domain;
using Domain;
using Microsoft.AspNetCore.Mvc;
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
    
    
    // generate content in the slides
    // and posts
    
    
    
    
    
}