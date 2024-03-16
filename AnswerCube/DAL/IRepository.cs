using AnswerCube.BL.Domain;
using Domain;

namespace AnswerCube.DAL;

public interface IRepository
{
    List<OpenQuestion> GetOpenSlides();
    
    List<ListQuestion> GetListSlides();

    List<ListQuestion> GetSingleChoiceSlides();
    
    List<ListQuestion> GetMultipleChoiceSlides();
    
    SlideList ReadSlideList(int id);

    List<List_Question> GetMultipleChoiceSlides();

    List<Info> GetInfoSlides();
    
}