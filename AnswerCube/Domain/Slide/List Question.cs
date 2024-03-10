namespace Domain;

public class List_Question : ISlide
{
    public int Id { get; set;}
    public string Question { get; set;}
    public Boolean IsMultipleChoice { get; set; }
    public List<String> AnswerList { get; set; }
}