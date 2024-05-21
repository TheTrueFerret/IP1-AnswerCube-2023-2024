using AnswerCube.BL.Domain.User;
using AnswerCube.DAL;
using Domain;

namespace AnswerCube.BL;

public class ForumManager : IForumManager
{
    
    private readonly IForumRepository _repository;
    
    public ForumManager(IForumRepository repository)
    {
        _repository = repository;
    }
    
    public List<Forum> GetForums()
    {
        return _repository.ReadForums();
    }

    public Forum GetForum(int forumId)
    {
        return _repository.ReadForum(forumId);
    }

    public bool AddIdea(int forumId, string title, string content, AnswerCubeUser user)
    {
        return _repository.CreateIdea(forumId, title, content, user);
    }


    public bool AddReaction(int ideaId, string reaction, AnswerCubeUser? user)
    {
        if (user != null)
        {
            return _repository.CreateReaction(ideaId, reaction, user);
        }
        else
        {
            return _repository.CreateReaction(ideaId, reaction, null);
        }
    }

    public int GetForumByIdeaId(int ideaId)
    {
        return _repository.ReadForumByIdeaId(ideaId);
    }

    public int GetForumByReactionId(int reactionId)
    {
        return _repository.ReadForumByReactionId(reactionId);
    }

    public bool LikeReaction(int reactionId, AnswerCubeUser user)
    {
        return _repository.LikeReaction(reactionId, user);
    }

    public bool DislikeReaction(int reactionId, AnswerCubeUser user)
    {
        return _repository.DislikeReaction(reactionId, user);
    }


    public bool LikeIdea(int ideaId, AnswerCubeUser user)
    {
        return _repository.LikeIdea(ideaId, user);
    }


    public bool DislikeIdea(int ideaId, AnswerCubeUser user)
    {
        return _repository.DislikeIdea(ideaId, user);
    }

}