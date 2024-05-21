using AnswerCube.BL.Domain;
using Domain;

namespace AnswerCube.BL;

public interface IAnswerManager
{
    bool AddAnswer(List<string> answers, int id, Session session);
    List<Answer> GetAnswers();
}