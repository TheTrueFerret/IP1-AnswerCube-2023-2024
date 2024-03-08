namespace Domain;

public interface IFlow
{
    string Name { get; }
    LinkedList<string> SlideList { get; }
}