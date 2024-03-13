using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.BL;

public interface IManager
{
    List<Open_Question> GetOpenSlides();
    
    List<List_Question> getListSlides();
    
    List<List_Question> GetSingleChoiceSlides();
    
    List<List_Question> GetMultipleChoiceSlides();
    
    List<Info> GetInfoSlides();
}