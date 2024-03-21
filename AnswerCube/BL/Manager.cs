using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
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


    public List<Slide> GetOpenSlides()
    {
        return _repository.GetOpenSlides();
    }

    public List<Slide> GetListSlides()
    {
        return _repository.GetListSlides();
    }

    public List<Slide> GetSingleChoiceSlides()
    {
        return _repository.GetSingleChoiceSlides();
    }
    
    public List<Slide> GetMultipleChoiceSlides()
    {
        return _repository.GetMultipleChoiceSlides();
    }

    public SlideList GetSlideListById(int id)
    {
        return _repository.ReadSlideList(id);
    }

    public List<Slide> GetInfoSlides()
    {
        return _repository.GetInfoSlides();
    }

    public LinearFlow GetLinearFlow()
    {
        return _repository.GetLinearFlow();
    }

    public Slide GetSlideFromFlow(int flowId, int number)
    {
        return _repository.GetSlideFromFlow(flowId, number);
    }

    public SlideList GetSlideList()
    {
        return _repository.getSlideList();
    }
}