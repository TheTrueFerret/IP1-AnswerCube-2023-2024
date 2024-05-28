using AnswerCube.BL.Domain;
using Domain;

namespace AnswerCube.DAL;

public interface IAnswerRepository
{
    bool AddAnswer(List<string> answers, int id, Session session);
    List<Answer> GetAnswers();
    
    List<Slide> GetSlides();
    List<Session> GetSessions();
    List<Answer> GetAnswersBySessionId(int sessionId);
    Session GetSessionById(int id);
}