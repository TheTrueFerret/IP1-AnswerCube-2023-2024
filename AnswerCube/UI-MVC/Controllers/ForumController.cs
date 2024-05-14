using AnswerCube.BL;
using AnswerCube.UI.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnswerCube.UI.MVC.Controllers;

public class ForumController : BaseController
{
    private readonly IManager _manager;
    private readonly ILogger<ForumController> _logger;

    public ForumController(IManager manager, ILogger<ForumController> logger)
    {
        _manager = manager;
        _logger = logger;
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

    public IActionResult AddIdea(int forumId, string title, string content)
    {
        //This will add an idea to the forum with the given id
        _manager.AddIdea(forumId, title, content);
        return RedirectToAction("ShowForum", new { forumId });
    }

    public IActionResult Addreaction(int ideaId, string reaction)
    {
        //This will add a reaction to the idea with the given id
        if (_manager.AddReaction(ideaId, reaction))
        {
            return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByIdeaId(ideaId) });
        }

        return NotFound();
    }

    public IActionResult LikeReaction(int reactionId)
    {
        //This will like the reaction with the given id
        _manager.LikeReaction(reactionId);
        return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByReactionId(reactionId) });
    }

    public IActionResult DislikeReaction(int reactionId)
    {
        //This will dislike the reaction with the given id
        _manager.DislikeReaction(reactionId);
        return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByReactionId(reactionId) });
    }

    public IActionResult LikeIdea(int ideaId)
    {
        //This will like the idea with the given id
        _manager.LikeIdea(ideaId);
        return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByIdeaId(ideaId) });
    }

    public IActionResult DislikeIdea(int ideaId)
    {
        //This will dislike the idea with the given id
        _manager.DislikeIdea(ideaId);
        return RedirectToAction("ShowForum", new { forumId = _manager.GetForumByIdeaId(ideaId) });
    }
}