using AnswerCube.BL;
using static System.Console;

namespace AnswerCube.UI.CA;

public class CliTest
{
    private IManager _manager;
    
    public CliTest(Manager manager)
    {
        _manager = manager;
    }

    public void TestData()
    {
        ShowOpenSlides();
    }
    
    public void ShowOpenSlides()
    {
        _manager.GetOpenSlides().ForEach(slide => WriteLine(slide.Name));
    }

}