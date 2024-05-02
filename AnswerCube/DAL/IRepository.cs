using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.DAL;

public interface IRepository
{
    List<Slide> GetOpenSlides();

    List<Slide> GetListSlides();

    List<Slide> GetSingleChoiceSlides();

    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    
    Slide GetSlideFromFlow(int flowId, int number);
    Slide ReadSlideById(int id);
    SlideList getSlideList();
    SlideList ReadSlideListById(int id);
    Boolean AddAnswer(List<string> answers,int id);
    Slide ReadSlideFromSlideListByIndex(int index, int slideListId);
    
    Boolean StartInstallation(int id, SlideList slideList);
    Boolean UpdateInstallation(int id);
    int[] GetIndexAndSlideListFromInstallations(int id);
    Slide ReadActiveSlideByInstallationId(int id);
    List<IdentityRole> ReadAllAvailableRoles(IList<string> userRoles);
    List<AnswerCubeUser> ReadAllUsers();
    List<Answer> GetAnswers();
}