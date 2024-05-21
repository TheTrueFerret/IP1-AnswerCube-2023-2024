using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.DAL;

public interface IFlowRepository
{
    #region Slide
    List<Slide> GetOpenSlides();
    List<Slide> GetListSlides();
    List<Slide> GetSingleChoiceSlides();
    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    Slide GetSlideFromFlow(int flowId, int number);
    Slide ReadSlideById(int id);
    Slide ReadSlideFromSlideListByIndex(int index, int slideListId);
    bool CreateSlide(SlideType type, string question, string[]? options, int slideListId,string? mediaUrl);
    void UpdateSlide(SlideType slideType, string text, List<string> answers, int slideId);
    IEnumerable<Slide> ReadSlidesBySlideListId(int slideListId);
    bool RemoveSlideFromSlideList(int slideId, int slidelistid);
    #endregion

    #region SlideList
    SlideList getSlideList();
    SlideList ReadSlideListById(int id);
    bool CreateSlideList(string title, string description, int flowId);
    bool RemoveSlideListFromFlow(int slideListId, int flowId);
    List<Slide> ReadSlideList();
    SlideList ReadSlideListByTitle(string title);
    SlideList ReadSlideListWithFlowById(int slideListId);
    IEnumerable<SlideList> GetSlideListsByFlowId(int flowId);
    void UpdateSlideList(string title, string description, int slideListId);
    #endregion

    #region Flow
    bool CreateFlow(string name, string desc, bool circularFlow, int projectId);
    Flow ReadFlowById(int flowId);
    Flow ReadFlowWithProjectById(int flowId);
    void UpdateFlow(Flow model);
    List<Flow> ReadFlowsByUserId(string userId);
    Flow ReadFlowByInstallationId(int installationId);
    #endregion
}