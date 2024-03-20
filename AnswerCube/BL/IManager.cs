using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.BL;

public interface IManager
{
    List<OpenQuestion> GetOpenSlides();
    
    List<ListQuestion> GetListSlides();
    
    List<ListQuestion> GetSingleChoiceSlides();
    
    SlideList GetSlideListById(int id);

    List<ListQuestion> GetMultipleChoiceSlides();
    
    List<Info> GetInfoSlides();
    LinearFlow GetLinearFlow();
    AbstractSlide GetSlideFromFlow(int flowId, int number);
}