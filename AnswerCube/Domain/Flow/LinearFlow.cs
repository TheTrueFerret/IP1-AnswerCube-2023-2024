namespace Domain;

public class LinearFlow : IFlow
{
    public string Name { get; }
    public LinkedList<string> SlideList { get; }

    public LinearFlow(string name, LinkedList<string> slideList)
    {
        Name = name;
        SlideList = slideList;
    }
}