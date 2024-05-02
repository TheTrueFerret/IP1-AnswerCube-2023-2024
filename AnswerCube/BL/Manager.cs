using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Slide;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL;
using AnswerCube.DAL.EF;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace AnswerCube.BL;

public class Manager : IManager
{
    private readonly IRepository _repository;
    private readonly UserManager<AnswerCubeUser>? _userManager;

    public Manager(IRepository repository, UserManager<AnswerCubeUser>? userManager)
    {
        _repository = repository;
        _userManager = userManager;
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

    
    public Boolean AddAnswer(List<string> answers, int id)
    {
        return _repository.AddAnswer(answers, id);
    }

    public Slide GetSlideFromSlideListByIndex(int index, int slideListId)
    {
        return _repository.ReadSlideFromSlideListByIndex(index, slideListId);
    }

    public Boolean StartInstallation(int id, SlideList slideList)
    {
        return _repository.StartInstallation(id, slideList);
    }

    public Boolean UpdateInstallation(int id)
    {
        return _repository.UpdateInstallation(id);
    }

    public int[] GetIndexAndSlideListFromInstallations(int id)
    {
        return _repository.GetIndexAndSlideListFromInstallations(id);
    }

    public Slide GetActiveSlideByInstallationId(int id)
    {
        return _repository.ReadActiveSlideByInstallationId(id);
    }

    public List<IdentityRole> GetAllAvailableRoles(AnswerCubeUser user)
    {
        var userRoles = _userManager.GetRolesAsync(user).Result.ToList();
        return _repository.ReadAllAvailableRoles(userRoles);
    }

    public List<AnswerCubeUser> GetAllUsers()
    {
        return _repository.ReadAllUsers();
    }
    public List<Answer> GetAnswers()
    {
        return _repository.GetAnswers();
    }
}