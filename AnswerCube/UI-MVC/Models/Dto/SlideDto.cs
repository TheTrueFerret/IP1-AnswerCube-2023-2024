using Domain;

namespace UI_MVC.Models.Dto;

public class SlideDto
{
    public Slide Slide { get; set; }

    public SlideDto(Slide slide)
    {
        Slide = slide;
    }
}