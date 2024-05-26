using AnswerCube.BL.Domain;
using Domain;

namespace AnswerCube.DAL;

public interface IInstallationRepository
{
    Installation StartInstallationWithFlow(int installationId, int flowId);
    Boolean UpdateInstallation(int id);
    int[] GetIndexAndSlideListFromInstallations(int id);
    Slide ReadActiveSlideByInstallationId(int id);
    List<Installation> ReadInstallationsByUserId(string userId);
    bool UpdateInstallationToActive(int installationId);
    bool CreateNewInstallation(string name, string location, int organizationId);
    Session? GetSessionByInstallationIdAndCubeId(int installationId, int cubeId);
    Session WriteNewSessionWithInstallationId(Session newSession, int installationId);
    bool WriteSlideListToInstallation(int slideListId, int installationId);
    void WriteNoteToInstallation(int installationId, string note, string? identityName, DateTime now, int flowId);
}