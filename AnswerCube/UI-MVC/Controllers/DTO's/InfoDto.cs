using AnswerCube.BL.Domain.Slide;

namespace AnswerCube.UI.MVC.Controllers.DTO_s;

public class InfoDto
{
    public Info Info { get; set; }
    
    public InfoDto(Info info)
    {
        Info = info;
    }
}