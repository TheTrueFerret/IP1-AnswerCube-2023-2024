namespace Domain;

public class CircularFlow : IFlow
{
    public string Name { get; }
    public LinkedList<string> SlideList { get; }

    public CircularFlow(string name, LinkedList<string> slideList)
    {
        Name = name;
        SlideList = slideList;
    }
}   