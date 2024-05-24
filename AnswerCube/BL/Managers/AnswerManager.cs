using AnswerCube.BL.Domain;
using AnswerCube.DAL;
using Domain;

namespace AnswerCube.BL;

public class AnswerManager : IAnswerManager
{
    private readonly IAnswerRepository _repository;
    
    public AnswerManager(IAnswerRepository repository)
    {
        _repository = repository;
    }
    
    public bool AddAnswer(List<string> answers, int id, Session session)
    {
        return _repository.AddAnswer(answers, id, session);
    }
    
    public List<Answer> GetAnswers()
    {
        return _repository.GetAnswers();
    }
    
    public List<Slide> GetSlides()
    {
        return _repository.GetSlides();
    }
    
    public List<Answer> GetAnswersBySessionId(int sessionId)
    {
        return _repository.GetAnswersBySessionId(sessionId);
    }
    
    
}