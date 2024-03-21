using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.UI.MVC.Controllers.DTO_s;

public class InfoDto
{
    public Slide Info { get; set; }
    
    public InfoDto(Slide info)
    {
        Info = info;
    }
}