namespace AnswerCube.BL.Domain.Slide;

public class Info : global::Domain.AbstractSlide
{
    public int Id { get; set; }
    public string Question { get; set; }
    public string Text { get; set; }
    public int? vraagId { get; set; }
}