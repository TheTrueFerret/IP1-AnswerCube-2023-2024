using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.Installation;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace AnswerCube.DAL.EF;

public class InstallationRepository : IInstallationRepository
{
    private readonly AnswerCubeDbContext _context;
    private readonly IOrganizationRepository _organizationRepository;

    public InstallationRepository(AnswerCubeDbContext context, IOrganizationRepository organizationRepository)
    {
        _context = context;
        _organizationRepository = organizationRepository;
    }

    public Installation StartInstallationWithFlow(int installationId, int flowId)
    {
        Installation installation = _context.Installations.Single(i => i.Id == installationId);
        installation.Active = true;
        installation.Flow = _context.Flows.Include(f => f.SlideLists).ThenInclude(sl => sl.ConnectedSlides)
            .Single(f => f.Id == flowId);

        installation.ActiveSlideListId = null;
        installation.CurrentSlideIndex = 0;
        installation.MaxSlideIndex = null;
        return installation;
    }

    public bool UpdateInstallation(int installationId)
    {
        Installation installation = _context.Installations.Where(i => i.Id == installationId).First();
        if (installation.CurrentSlideIndex < installation.MaxSlideIndex)
        {
            installation.CurrentSlideIndex++;
            return true;
        }

        installation.ActiveSlideListId = null;
        // returns false if the currentslideindex exceeds the current slidelist
        return false;
    }

    public int[] GetIndexAndSlideListFromInstallations(int id)
    {
        Installation installation = _context.Installations.Where(i => i.Id == id).First();
        if (installation.MaxSlideIndex > installation.CurrentSlideIndex)
        {
            if (installation.ActiveSlideListId != null)
            {
                int[] idArray = new int[]
                {
                    installation.CurrentSlideIndex,
                    (int)installation.ActiveSlideListId
                };
                return idArray;
            }

            return new int[] { };
        }

        return new int[] { };
    }
    public Slide ReadActiveSlideByInstallationId(int id)
    {
        Installation installation = _context.Installations.Where(i => i.Id == id).First();
        SlideList slideList = _context.SlideLists.Where(sl => sl.Id == installation.ActiveSlideListId)
            .Include(sl => sl.ConnectedSlides).First();

        SlideConnection slideConnections = _context.SlideConnections
            .Where(sc => sc.SlideList.Id == slideList.Id)
            .Where(sc => sc.SlideOrder == installation.CurrentSlideIndex).Single();

        Slide slide = _context.Slides.Where(s => s.Id == slideConnections.SlideId).Single();
        return slide;
    }

    public List<Installation> ReadInstallationsByUserId(string userId)
    {
        List<Organization> organizations = _organizationRepository.ReadOrganizationByUserId(userId);
        List<Installation> installations = new List<Installation>();
        foreach (var organization in organizations)
        {
            installations.AddRange(_context.Installations
                .Where(i => i.Organization == organization)
                .Where(i => i.Active == false));
        }

        return installations;
    }

    public bool UpdateInstallationToActive(int installationId)
    {
        Installation installation = _context.Installations.Where(i => i.Id == installationId).First();
        installation.Active = true;
        _context.Installations.Update(installation);
        return installation.Active;
    }

    public bool CreateNewInstallation(string name, string location, int organizationId)
    {
        Organization organization = _context.Organizations.Single(o => o.Id == organizationId);
        Installation installation = new Installation
        {
            Name = name,
            Location = location,
            Active = false,
            CurrentSlideIndex = 0,
            MaxSlideIndex = 0,
            ActiveSlideListId = 0,
            OrganizationId = organizationId,
            Organization = organization
        };
        _context.Installations.Add(installation);
        return false;
    }

    public Session? ReadActiveSessionByInstallationIdAndCubeId(int installationId, int cubeId)
    {
        Session? session = _context.Sessions.SingleOrDefault(s => s.Installation.Id == installationId && s.CubeId == cubeId && s.EndTime == null);
        if (session != null)
        {
            return session;
        }
        return null;
    }

    public bool WriteNewSessionWithInstallationId(Session newSession, int installationId)
    {
        newSession.Installation = _context.Installations.Single(i => i.Id == installationId);
        _context.Sessions.Add(newSession);
        return true;
    }

    public bool WriteSlideListToInstallation(int slideListId, int installationId)
    {
        Installation installation = _context.Installations.Single(i => i.Id == installationId);
        installation.ActiveSlideListId = slideListId;
        installation.CurrentSlideIndex = 0;
        SlideList slideList = _context.SlideLists.Include(sl => sl.ConnectedSlides).Single(sl => sl.Id == slideListId);
        installation.MaxSlideIndex = slideList.ConnectedSlides.Count;
        return true;
    }

    public void WriteNoteToInstallation(int installationId, string note, string? identityName, int flowId)
    {
        DateTime utcTime = DateTime.UtcNow;
        Note newNote = new Note
        {
            NoteText = note,
            IdentityName = identityName,
            CreatedAt = utcTime,
            FlowId = flowId,
            InstallationId = installationId
        };
        _context.Notes.Add(newNote);
    }

    public void UpdateInstallationUrl(int installationId, string url)
    {
        Installation installation = _context.Installations.SingleOrDefault(i => i.Id == installationId);
        if (installation == null)
        {
            return;
        }
        installation.ConnectionId = url;
    }

    public string GetConnectionIdByInstallationId(int installationId)
    {
        Installation installation = _context.Installations.SingleOrDefault(i => i.Id == installationId);
        if (installation == null)
        {
            return null;
        }
        return installation.ConnectionId;
    }

    public List<Installation> ReadActiveInstallationsFromOrganizations(List<Organization> organizations)
    {
        List<Installation> installations = new List<Installation>();
        foreach (var organization in organizations)
        {
            installations.AddRange(_context.Installations
                .Where(i => i.Organization == organization)
                .Where(i => i.Active));
        }

        return installations;
    }

    public List<Session>? ReadActiveSessionsByInstallationId(int installationId)
    {
        return _context.Sessions.Where(s => s.Installation.Id == installationId && s.EndTime == null).ToList();
    }

    public bool EndSessionByInstallationIdAndCubeId(int installationId, int cubeId)
    {
        Session? session = ReadActiveSessionByInstallationIdAndCubeId(installationId, cubeId);
        session.EndTime = DateTime.Now.ToUniversalTime();
        _context.Sessions.Update(session);
        return true;
    }

    public int ReadForumIdByInstallationId(int installationId)
    {
        Installation installation = _context.Installations.Include(i => i.Organization).ThenInclude(o =>o.Forum).SingleOrDefault(i => i.Id == installationId);
        if (installation == null)
        {
            return 0;
        }
        return installation.Organization.Forum.Id;
    }
}