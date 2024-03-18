using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.BL;

public interface IManager
{
    List<OpenQuestion> GetOpenSlides();
    
    List<ListQuestion> GetListSlides();
    
    List<ListQuestion> GetSingleChoiceSlides();

    List<ListQuestion> GetMultipleChoiceSlides();
    
    List<Info> GetInfoSlides();
    
    SlideList GetSlideListById(int id);
    
    List<SlideList> GetSlideLists();

}