namespace Domain;

public class Range_Question : ISlide
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<String> RangeList
    {
        get { return RangeList; }
        set
        {
            if (value.Count > 5)
            {
                throw new ArgumentException("Question details cannot have more than 5 items");
            }

            RangeList = value;
        }
    }

    public List<int> RangeValues
    {
        get { return RangeValues; }
        set
        {
            if (value.Any(i => i > 5))
            {
                throw new ArgumentException("Answer data cannot be above 5");
            }

            RangeValues = value;
        }
    }
}