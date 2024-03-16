using AnswerCube.BL.Domain;
using Domain;

namespace UI_MVC.Models.Dto;

public class SlideListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public SubTheme SubTheme { get; set; }
    public LinkedList<AbstractSlide> Slides { get; set; }

    public SlideListDto(SlideList slideList)
    {
        Id = slideList.Id;
        Title = slideList.Title;
        SubTheme = slideList.SubTheme;
        Slides = slideList.Slides;
    }
}