using Domain;

namespace AnswerCube.BL.Domain;

public class SlideList
{
    public string Title { get; set; }
    public SubTheme SubTheme { get; set; }
    public LinkedList<ISlide> Slides { get; set; }

    public SlideList(string title, SubTheme subTheme, LinkedList<ISlide> slides)
    {
        Title = title;
        SubTheme = subTheme;
        Slides = slides;
    }
    
    
}