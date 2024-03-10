using Domain;

namespace AnswerCube.DAL;

public interface IRepository
{
    List<Open_Question> GetOpenSlides();
    
    List<List_Question> getListSlides();
    List<List_Question> GetSingleChoiceSlides();
    
    List<List_Question> GetMultipleChoiceSlides();
}