using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.DAL;

public interface IRepository
{
    List<Slide> GetOpenSlides();

    List<Slide> GetListSlides();

    List<Slide> GetSingleChoiceSlides();

    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    
    LinearFlow GetLinearFlow();
    Slide GetSlideFromFlow(int flowId, int number);
    Slide ReadSlideById(int id);
    SlideList getSlideList();
    SlideList ReadSlideListById(int id);
    Boolean AddAnswer(List<string> answers,int id);
    Slide ReadSlideFromSlideListByIndex(int index, int slideListId);
    
}