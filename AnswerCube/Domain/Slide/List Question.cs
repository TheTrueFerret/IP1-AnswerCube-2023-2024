namespace Domain;

public class List_Question : ISlide
{
    public string name { get; }
    private Boolean isMultipleChoice { get; set; }
    private List<String> questionList { get; set; }
    private List<String> answerList { get; set; }
    
    public List_Question(string name, Boolean isMultipleChoice, List<String> questionList, List<String> answerList)
    {
        this.name = name;
        this.isMultipleChoice = isMultipleChoice;
        this.questionList = questionList;
        this.answerList = answerList;
    }
}