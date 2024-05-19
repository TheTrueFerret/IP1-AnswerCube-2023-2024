using AnswerCube.BL;
using AnswerCube.BL.Domain.User;
using AnswerCube.UI.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class ForumController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<ForumController> _logger;
    private UserManager<AnswerCubeUser> _UserManager;

    public ForumController(IManager manager, ILogger<ForumController> logger, UserManager<AnswerCubeUser> userManager)
    {
        _manager = manager;
        _logger = logger;
        _UserManager = userManager;
    }

    public IActionResult Forums()
    {
        //This will show the forums that are available
        return View(_manager.GetForums());
    }

    [Route("[action]/{forumId:int}")]
    public IActionResult ShowForum(int forumId)
    {
        //This will show the forum with the given id
        return View(_manager.GetForum(forumId));
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
        _manager.AddIdea(forumId, title, content, user);
        return RedirectToAction("ShowForum", new { forumId });
    }

    public IActionResult Addreaction(int ideaId, string reaction)
    {
        AnswerCubeUser user = _UserManager.GetUserAsync(User).Result;
        if (user == null)
        {
            if (_manager.AddReaction(ideaId, reaction, null))
            {
                return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByIdeaId(ideaId) });
            }
        }

        //This will add a reaction to the idea with the given id
        if (_manager.AddReaction(ideaId, reaction, user))
        {
            return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByIdeaId(ideaId) });
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
        _manager.LikeReaction(reactionId,user);
        return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByReactionId(reactionId) });
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
        _manager.DislikeReaction(reactionId,user);
        return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByReactionId(reactionId) });
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
        _manager.LikeIdea(ideaId,user);
        return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByIdeaId(ideaId) });
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
        _manager.DislikeIdea(ideaId,user);
        return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByIdeaId(ideaId) });
    }
}