using AnswerCube.BL.Domain.Slide;
using AnswerCube.DAL;
using Domain;

namespace AnswerCube.BL;

public class FlowManager : IFlowManager
{
    private readonly IFlowRepository _repository;
    
    public FlowManager(IFlowRepository repository)
    {
        _repository = repository;
    }
    
    #region Slide
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
    
    public Slide GetSlideById(int id)
    {
        return _repository.ReadSlideById(id);
    }
    
    public Slide GetSlideFromFlow(int flowId, int number)
    {
        return _repository.GetSlideFromFlow(flowId, number);
    }
    
    public Slide GetSlideFromSlideListByIndex(int index, int slideListId)
    {
        return _repository.ReadSlideFromSlideListByIndex(index, slideListId);
    }
    
    public bool CreateSlide(SlideType type, string question, List<string>? options, int slideListId, string? mediaUrl=null)
    {
        
        if (type == SlideType.InfoSlide && options.Count == 1)
        {
            string info = question + "\n" + options[0];
            return _repository.CreateSlide(type, info, null, slideListId, mediaUrl);
        }

        return _repository.CreateSlide(type, question, options, slideListId,mediaUrl);
    }

    public List<Slide> GetAllSlides()
    {
        return _repository.ReadSlideList();
    }
    
    public IEnumerable<Slide> GetSlidesBySlideListId(int slideListId)
    {
        return _repository.ReadSlidesBySlideListId(slideListId);
    }

    public bool RemoveSlideFromSlideList(int slideId, int slidelistid)
    {
        return _repository.RemoveSlideFromSlideList(slideId, slidelistid);
    }

    public void UpdateSlide(SlideType slideType, string text, List<string> answersList, int slideId)
    {
        _repository.UpdateSlide(slideType, text, answersList, slideId);
    }
    #endregion

    #region SlideList
    public SlideList GetSlideList()
    {
        return _repository.getSlideList();
    }

    public SlideList GetSlideListById(int id)
    {
        return _repository.ReadSlideListById(id);
    }
    
    public SlideList GetSlideListByTitle(string title)
    {
        return _repository.ReadSlideListByTitle(title);
    }
    
    public bool CreateSlidelist(string title, string description, int flowId)
    {
        return _repository.CreateSlideList(title, description, flowId);
    }
    
    public SlideList GetSlideListWithFlowById(int slideListId)
    {
        return _repository.ReadSlideListWithFlowById(slideListId);
    }
    
    public IEnumerable<SlideList> GetSlideListsByFlowId(int flowId)
    {
        return _repository.GetSlideListsByFlowId(flowId);
    }
    
    public bool RemoveSlideListFromFlow(int slideListId, int flowId)
    {
        return _repository.RemoveSlideListFromFlow(slideListId, flowId);
    }

    public void UpdateSlideList(string title, string description, int slideListId)
    {
        _repository.UpdateSlideList(title, description, slideListId);
    }

    public SlideList GetSlideListByInstallationId(int installationId)
    {
        return _repository.ReadSlideListByInstallationId(installationId);
    }
    #endregion

    #region Flow
    public bool CreateFlow(string name, string desc, bool circularFlow, int projectId)
    {
        return _repository.CreateFlow(name, desc, circularFlow, projectId);
    }
    public Flow GetFlowById(int flowId)
    {
        return _repository.ReadFlowById(flowId);
    }
    
    public Flow GetFlowWithProjectById(int flowId)
    {
        return _repository.ReadFlowWithProjectById(flowId);
    }
    public void UpdateFlow(Flow model)
    {
        _repository.UpdateFlow(model);
    }
    
    public List<Flow> GetFlowsByUserId(string userId)
    {
        return _repository.ReadFlowsByUserId(userId);
    }

    public Flow GetFlowByInstallationId(int installationId)
    {
        return _repository.ReadFlowByInstallationId(installationId);
    }
    #endregion
    
}