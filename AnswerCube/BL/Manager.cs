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


    public List<OpenQuestion> GetOpenSlides()
    {
        return _repository.GetOpenSlides();
    }

    public List<ListQuestion> GetListSlides()
    {
        return _repository.GetListSlides();
    }

    public List<ListQuestion> GetSingleChoiceSlides()
    {
        return _repository.GetSingleChoiceSlides();
    }
    
    public List<ListQuestion> GetMultipleChoiceSlides()
    {
        return _repository.GetMultipleChoiceSlides();
    }
    
    public List<Info> GetInfoSlides()
    {
        return _repository.GetInfoSlides();
    }
    
    // public AbstractSlide GetSlideByIdAndSlideType(int id, TypeSlide typeSlide)
    // {
    //     return _repository.ReadSlideById(id, typeSlide);
    // }

    public SlideList GetSlideListById(int id)
    {
        return _repository.ReadSlideList(id);
    }

    public List<SlideList> GetSlideLists()
    {
        return _repository.ReadSlideLists();
    }
}