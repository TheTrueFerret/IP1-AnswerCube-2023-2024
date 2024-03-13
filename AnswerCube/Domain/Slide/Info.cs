using Domain;

namespace AnswerCube.BL.Domain.Slide;

public class Info : ISlide
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Text { get; set; }
}