using AnswerCube.BL.Domain;
using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnswerCube.DAL.EF;

public class AnswerRepository : IAnswerRepository
{
    private readonly ILogger<AnswerRepository> _logger;
    private readonly AnswerCubeDbContext _context;
    private readonly UserManager<AnswerCubeUser> _userManager;

    public AnswerRepository(AnswerCubeDbContext context, ILogger<AnswerRepository> logger, UserManager<AnswerCubeUser> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }
    
    public bool AddAnswer(List<string> answers, int id, Session session)
    {
        Slide slide = _context.Slides.First(s => s.Id == id);
        Answer newAnswer = new Answer
        {
            AnswerText = answers,
            Slide = slide,
            Session = session
        };
        _context.Answers.Add(newAnswer);
        return true;
    }

    public List<Answer> GetAnswers()
    {
        var answers = _context.Answers
            .Include(a => a.Slide)
            .ToList();
        return answers;
    }
    
    public List<Slide> GetSlides()
    {
        var slides = _context.Slides
            .Include(s => s.Answers)
            .ThenInclude(s => s.Session)
            .ToList();
        return slides;
    }

    public List<Session> GetSessions()
    {
        var sessions = _context.Sessions.ToList();
        return sessions;
    }

    public List<Answer> GetAnswersBySessionId(int sessionId)
    {
        var answers = _context.Answers.Where(s => s.Session.Id == sessionId)
            .Include(a => a.Slide)
            .ToList();
        return answers;
    }

    public Session GetSessionById(int id)
    {
        var session = _context.Sessions
            // .Include()
            .FirstOrDefault(s => s.Id == id);
        return session;
    }
}