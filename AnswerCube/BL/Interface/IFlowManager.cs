using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.BL;

public interface IFlowManager
{
    #region Slide
    List<Slide> GetOpenSlides();
    List<Slide> GetListOfSlides();
    List<Slide> GetSingleChoiceSlides();
    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    Slide GetSlideFromFlow(int flowId, int number);
    Slide GetSlideById(int id);
    Slide GetSlideFromSlideListByIndex(int index, int slideListId);
    bool CreateSlide(SlideType type, string question, string[]? options,int slideListId,string? mediaUrl);
    List<Slide> GetAllSlides();
    void UpdateSlide(string text, List<string>? answers, int slideId);
    IEnumerable<Slide> GetSlidesBySlideListId(int slideListId);
    bool RemoveSlideFromSlideList(int slideId, int slidelistid);
    #endregion
    
    #region SlideList
    SlideList GetSlideList();
    SlideList GetSlideListById(int id);
    bool CreateSlidelist(string title, string description, int flowId);
    SlideList GetSlideListByTitle(string title);
    SlideList GetSlideListWithFlowById(int slideListId);
    IEnumerable<SlideList> GetSlideListsByFlowId(int flowId);
    void UpdateSlideList(string title, string description, int slideListId);
    bool RemoveSlideListFromFlow(int slideListId, int flowId);
    SlideList GetSlideListByInstallationId(int installationId);
    #endregion
    
    #region Flow
    bool CreateFlow(string name, string desc, bool circularFlow, int projectId);
    Flow GetFlowById(int flowId);
    Flow GetFlowWithProjectById(int flowId);
    void UpdateFlow(Flow model);
    bool RemoveFlowFromProject(int flowId);
    List<Flow> GetFlowsByUserId(string userId);
    Flow GetFlowByInstallationId(int installationId);
    #endregion
}