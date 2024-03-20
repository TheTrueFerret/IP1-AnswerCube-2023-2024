using Domain;

namespace UI_MVC.Models.Dto;

public class SlideDto
{
    public AbstractSlide Slide { get; set; }

    public SlideDto(AbstractSlide slide)
    {
        Slide = slide;
    }
}