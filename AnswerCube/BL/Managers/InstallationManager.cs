using AnswerCube.BL.Domain;
using AnswerCube.DAL;
using Domain;

namespace AnswerCube.BL;

public class InstallationManager : IInstallationManager
{
    private readonly IInstallationRepository _repository;
    
    public InstallationManager(IInstallationRepository repository)
    {
        _repository = repository;
    }
    public List<Installation> GetInstallationsByUserId(string userId)
    {
        return _repository.ReadInstallationsByUserId(userId);
    }

    public bool SetInstallationToActive(int installationId)
    {
        return _repository.UpdateInstallationToActive(installationId);
    }
    
    public Installation AddNewInstallation(string name, string location, int organizationId)
    {
        return _repository.CreateNewInstallation(name, location, organizationId);
    }

    public Session? GetActiveSessionByInstallationIdAndCubeId(int installationId, int cubeId)
    {
        return _repository.ReadActiveSessionByInstallationIdAndCubeId(installationId, cubeId);
    }

    public bool AddNewSessionWithInstallationId(Session newSession, int installationId)
    {
        return _repository.WriteNewSessionWithInstallationId(newSession, installationId);
    }
    
    public Installation StartInstallationWithFlow(int installationId, int flowId)
    {
        return _repository.StartInstallationWithFlow(installationId, flowId);
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

    public bool AddSlideListToInstallation(int slideListId, int installationId)
    {
        return _repository.WriteSlideListToInstallation(slideListId, installationId);
    }

    public void AddNoteToInstallation(int installationId, string note, string? identityName, int flowId)
    {
        _repository.WriteNoteToInstallation(installationId, note, identityName, flowId);
    }

    public void SetInstallationUrl(int installationId, string url)
    {
        _repository.UpdateInstallationUrl(installationId, url);
    }

    public string GetConnectionIdByInstallationId(int installationId)
    {
        return _repository.GetConnectionIdByInstallationId(installationId);
    }

    public List<Installation> GetActiveInstallationsFromOrganizations(List<Organization> organizations)
    {
        return _repository.ReadActiveInstallationsFromOrganizations(organizations);
    }

    public List<Session>? GetActiveSessionsByInstallationId(int installationId)
    {
        return _repository.ReadActiveSessionsByInstallationId(installationId);
    }

    public bool EndSessionByInstallationIdAndCubeId(int installationId, int cubeId)
    {
        return _repository.EndSessionByInstallationIdAndCubeId(installationId, cubeId);
    }

    public int GetForumIdByInstallationId(int installationId)
    {
        return _repository.ReadForumIdByInstallationId(installationId);
    }
}