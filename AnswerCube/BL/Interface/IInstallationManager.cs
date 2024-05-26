using AnswerCube.BL.Domain;
using Domain;

namespace AnswerCube.BL;

public interface IInstallationManager
{
    Installation StartInstallationWithFlow(int installationId, int flowId);
    bool UpdateInstallation(int id);
    int[] GetIndexAndSlideListFromInstallations(int id);
    Slide GetActiveSlideByInstallationId(int id);
    List<Installation> GetInstallationsByUserId(string userId);
    bool SetInstallationToActive(int installationId);
    bool AddNewInstallation(string name, string location, int organizationId);
    Session? GetSessionByInstallationIdAndCubeId(int installationId, int cubeId);
    Session AddNewSessionWithInstallationId(Session newSession, int installationId);
    bool AddSlideListToInstallation(int slideListId, int installationId);
    void AddNoteToInstallation(int installationId, string note, string? identityName, DateTime now, int flowId);
}