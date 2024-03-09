using Domain;

namespace AnswerCube.DAL;

public interface IRepository
{
    List<Open_Question> GetOpenSlides();
}