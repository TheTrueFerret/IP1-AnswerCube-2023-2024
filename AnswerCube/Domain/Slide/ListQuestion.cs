using AnswerCube.BL.Domain;

namespace Domain;

public class ListQuestion : Slide
{ 
    public Boolean IsMultipleChoice { get; set; }
    public List<String> AnswerList { get; set; }
}