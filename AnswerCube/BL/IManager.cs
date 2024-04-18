using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using Domain;

namespace AnswerCube.BL;

public interface IManager
{
    List<Slide> GetOpenSlides();
    List<Slide> GetListOfSlides();
    List<Slide> GetSingleChoiceSlides();
    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    LinearFlow GetLinearFlow();
    Slide GetSlideFromFlow(int flowId, int number);
    SlideList GetSlideList();
    SlideList GetSlideListById(int id);
    Boolean AddAnswer(List<string> answers,int id);
    Slide GetSlideById(int id);
    Slide GetSlideFromSlideListByIndex(int index, int slideListId);
    Boolean StartInstallation(int id, SlideList slideList);
    int[] UpdateInstallation(int id);
    int[] GetIndexAndSlideListFromInstallations(int id);
}