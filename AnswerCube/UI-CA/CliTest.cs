using AnswerCube.BL;
using static System.Console;

namespace AnswerCube.UI.CA;

public class CliTest
{
    private readonly IFlowManager _flowManager;

    public CliTest(IFlowManager manager)
    {
        _flowManager = manager;
    }

    public void TestData()
    {
        WriteLine("\nShow Open Slides");
        ShowOpenSlides();

        WriteLine("\nShow List Slides");
        ShowListSlides();

        WriteLine("\nShow Single Choice Slides");
        ShowSingleChoiceSlides();

        WriteLine("\nShow Multiple Choice Slides");
        ShowMultipleChoiceSlides();
    }

    public void ShowOpenSlides()
    {
        _flowManager.GetOpenSlides().ForEach(slide => WriteLine(slide.Id + " " + slide.Text));
    }

    public void ShowListSlides()
    {
        _flowManager.GetListOfSlides().ForEach(slide => WriteLine(slide.Id + " " + slide.Text));
    }

    public void ShowSingleChoiceSlides()
    {
        _flowManager.GetSingleChoiceSlides().ForEach(slide => WriteLine(slide.Id + " " + slide.Text));
    }

    public void ShowMultipleChoiceSlides()
    {
        _flowManager.GetMultipleChoiceSlides().ForEach(slide => WriteLine(slide.Id + " " + slide.Text));
    }
}