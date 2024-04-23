using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.BL;

public interface IManager
{
    List<Slide> GetOpenSlides();
    List<Slide> GetListOfSlides();
    List<Slide> GetSingleChoiceSlides();
    List<Slide> GetMultipleChoiceSlides();
    List<Slide> GetInfoSlides();
    Slide GetSlideFromFlow(int flowId, int number);
    SlideList GetSlideList();
    SlideList GetSlideListById(int id);
    Boolean AddAnswer(List<string> answers,int id);
    Slide GetSlideById(int id);
    Slide GetSlideFromSlideListByIndex(int index, int slideListId);
    Boolean StartInstallation(int id, SlideList slideList);
    Boolean UpdateInstallation(int id);
    int[] GetIndexAndSlideListFromInstallations(int id);
    Slide GetActiveSlideByInstallationId(int id);
    List<IdentityRole> GetAllAvailableRoles(AnswerCubeUser user);
    List<AnswerCubeUser> GetAllUsers();
    Boolean GetDeelplatformBeheerderByEmail(string userEmail);
    Boolean AddDeelplatformBeheerderByEmail(string userEmail);
    Boolean RemoveDeelplatformBeheerderByEmail(string userEmail);
}