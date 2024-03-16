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

    [HttpGet]
    public ActionResult<SlideDto> NextSlide()
    {
        List<ListQuestion> allSlides = _manager.GetMultipleChoiceSlides();
        Random rand = new Random();
        SlideDto slideDto = new SlideDto(allSlides[rand.Next(allSlides.Count)]);
        return slideDto;
    }
    
    public ActionResult<SlideListDto> NextSlideList()
    {
        SlideListDto slideList = new SlideListDto(_manager.GetSlideListById(1));
        return slideList;
    }
}