using AnswerCube.BL.Domain;

namespace Domain;

public class RequestingInfo : AbstractSlide
{
    public string Question { get; set;}
    public string Email { get; set; }
}