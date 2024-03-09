using AnswerCube.DAL;
using Domain;

namespace AnswerCube.BL;

public class Manager : IManager
{
    private IRepository _repository;

    public Manager(IRepository repository)
    {
        _repository = repository;
    }


    public List<Open_Question> GetOpenSlides()
    {
        return _repository.GetOpenSlides();
    }
}