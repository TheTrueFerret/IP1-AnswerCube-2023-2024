namespace Domain;

public class List_Question : ISlide
{
    public int Id { get; set;}
    public string Name { get; set;}
    public Boolean IsMultipleChoice { get; set; }
    public List<String> QuestionList { get; set; }
    public List<String> AnswerList { get; set; }
}