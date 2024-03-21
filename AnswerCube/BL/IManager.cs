using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.BL;

public interface IManager
{
    List<Slide> GetOpenSlides();
    
    List<Slide> GetListSlides();
    
    List<Slide> GetSingleChoiceSlides();
    
    SlideList GetSlideListById(int id);

    List<Slide> GetMultipleChoiceSlides();
    
    List<Slide> GetInfoSlides();
    LinearFlow GetLinearFlow();
    Slide GetSlideFromFlow(int flowId, int number);
    SlideList GetSlideList();
}