using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.DAL.EF;
using AnswerCube.UI.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class ForumController : BaseController
{
    private readonly IForumManager _forumManager;
    private readonly ILogger<ForumController> _logger;
    private UserManager<AnswerCubeUser> _UserManager;
    private UnitOfWork _uow;

    public ForumController(IForumManager manager, ILogger<ForumController> logger, UserManager<AnswerCubeUser> userManager, UnitOfWork uow)
    {
        _forumManager = manager;
        _logger = logger;
        _UserManager = userManager;
        _uow = uow;
    }

    public IActionResult Forums()
    {
        //This will show the forums that are available
        return View(_forumManager.GetForums());
    }

    [Route("[action]/{forumId:int}")]
    public IActionResult ShowForum(int forumId)
    {
        //TODO cookie instellen zodat een thema van de organization op het forum word geladen (kijk naar organization Controller voor cookie in the stellen)
        
        //This will show the forum with the given id
        return View(_forumManager.GetForum(forumId));
    }

    [Authorize(Roles = "Gebruiker")]
    public IActionResult AddIdea(int forumId, string title, string content)
    {
        AnswerCubeUser user = _UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return RedirectToAction("Forums");
        }

        //This will add an idea to the forum with the given id
        _uow.BeginTransaction();
        _forumManager.AddIdea(forumId, title, content, user);
        _uow.Commit();
        return RedirectToAction("ShowForum", new { forumId });
    }

    public IActionResult Addreaction(int ideaId, string reaction)
    {
        AnswerCubeUser user = _UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            _uow.BeginTransaction();
            if (_forumManager.AddReaction(ideaId, reaction, null))
            {
                _uow.Commit();
                return RedirectToAction("ShowForum", new { forumId = _forumManager.GetForumByIdeaId(ideaId) });
            }
        }

        //This will add a reaction to the idea with the given id
        _uow.BeginTransaction();
        if (_forumManager.AddReaction(ideaId, reaction, user))
        {
            _uow.Commit();
            return RedirectToAction("ShowForum", new { forumId = _forumManager.GetForumByIdeaId(ideaId) });
        }

        return NotFound();
    }

    [Authorize(Roles = "Gebruiker")]
    public IActionResult LikeReaction(int reactionId)
    {
        AnswerCubeUser user = _UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return RedirectToAction("Forums");
        }

        //This will like the reaction with the given id
        _uow.BeginTransaction();
        _forumManager.LikeReaction(reactionId,user);
        _uow.Commit();
        return RedirectToAction("ShowForum", new { forumId = _forumManager.GetForumByReactionId(reactionId) });
    }

    [Authorize(Roles = "Gebruiker")]
    public IActionResult DislikeReaction(int reactionId)
    {
        AnswerCubeUser user = _UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return RedirectToAction("Forums");
        }
        //This will dislike the reaction with the given id
        _uow.BeginTransaction();
        _forumManager.DislikeReaction(reactionId,user);
        _uow.Commit();
        return RedirectToAction("ShowForum", new { forumId = _forumManager.GetForumByReactionId(reactionId) });
    }

    [Authorize(Roles = "Gebruiker")]
    public IActionResult LikeIdea(int ideaId)
    {
        AnswerCubeUser user = _UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return RedirectToAction("Forums");
        }
        //This will like the idea with the given id
        _uow.BeginTransaction();
        _forumManager.LikeIdea(ideaId,user);
        _uow.Commit();
        return RedirectToAction("ShowForum", new { forumId = _forumManager.GetForumByIdeaId(ideaId) });
    }

    [Authorize(Roles = "Gebruiker")]
    public IActionResult DislikeIdea(int ideaId)
    {
        AnswerCubeUser user = _UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            return RedirectToAction("Forums");
        }
        //This will dislike the idea with the given id
        _uow.BeginTransaction();
        _forumManager.DislikeIdea(ideaId,user);
        _uow.Commit();
        return RedirectToAction("ShowForum", new { forumId = _forumManager.GetForumByIdeaId(ideaId) });
    }
}