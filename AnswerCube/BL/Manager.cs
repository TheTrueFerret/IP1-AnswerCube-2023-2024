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

    public List<Slide> GetListOfSlides()
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
    
    public SlideList GetSlideListById(int id)
    {
        return _repository.ReadSlideListById(id);
    }

    public Slide GetSlideById(int id)
    {
        return _repository.ReadSlideById(id);
    }
    
    public Boolean AddAnswer(List<string> answers,int id)
    {
        return _repository.AddAnswer(answers,id);
    }

    public Slide GetSlideFromSlideListByIndex(int index, int slideListId)
    {
        return _repository.ReadSlideFromSlideListByIndex(index, slideListId);
    }

    public Boolean StartInstallation(int id, SlideList slideList)
    {
        return _repository.StartInstallation(id, slideList);
    }

    public int[] UpdateInstallation(int id)
    {
        return _repository.UpdateInstallation(id);
    }

}