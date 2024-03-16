using AnswerCube.BL.Domain;

namespace Domain;

public class ListQuestion : AbstractSlide
{ 
    public Boolean IsMultipleChoice { get; set; }
    public List<String> AnswerList { get; set; }
}