using AnswerCube.BL.Domain.Slide;
>>>>>>> AnswerCube/BL/IManager.cs
using Domain;

namespace AnswerCube.BL;

public interface IManager
{
    List<OpenQuestion> GetOpenSlides();
    
    List<ListQuestion> GetListSlides();
    
    List<ListQuestion> GetSingleChoiceSlides();
    
    SlideList GetSlideListById(int id);

    List<List_Question> GetMultipleChoiceSlides();
    
    List<Info> GetInfoSlides();
}