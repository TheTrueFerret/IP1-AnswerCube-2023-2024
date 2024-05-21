using AnswerCube.BL.Domain;
using Domain;

namespace AnswerCube.DAL;

public interface IAnswerRepository
{
    bool AddAnswer(List<string> answers,int id, Session session);
    List<Answer> GetAnswers();
}