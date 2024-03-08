using AnswerCube.DAL;

namespace AnswerCube.BL;

public class Manager : IManager
{
    private IRepository _repository;

    public Manager(IRepository repository)
    {
        _repository = repository;
    }
    
    
}