using AnswerCube.BL.Domain.User;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace AnswerCube.DAL.EF;

public class ForumRepository : IForumRepository
{
    private readonly AnswerCubeDbContext _context;
    
    public ForumRepository(AnswerCubeDbContext context)
    {
        _context = context;
    }
    
    public List<Forum> ReadForums()
    {
        return _context.Forums.Include(f => f.Organization).Include(f => f.Ideas).ToList();
    }

    public Forum ReadForum(int forumId)
    {
        return _context.Forums
            .Include(f => f.Ideas).ThenInclude(i => i.Reactions).ThenInclude(r => r.Likes)
            .Include(f => f.Ideas).ThenInclude(i => i.Reactions).ThenInclude(r => r.Dislikes)
            .Include(f => f.Ideas).ThenInclude(i => i.Reactions).ThenInclude(r => r.User)
            .Include(f => f.Ideas).ThenInclude(i => i.Likes)
            .Include(f => f.Ideas).ThenInclude(i => i.Dislikes)
            .Include(f => f.Ideas).ThenInclude(i => i.User)
            .Include(f => f.Organization)
            .First(f => f.Id == forumId);
    }

    public int ReadForumByIdeaId(int ideaId)
    {
        return _context.Ideas.First(i => i.Id == ideaId).ForumId;
    }

    public bool CreateReaction(int ideaId, string reaction, AnswerCubeUser? user)
    {
        Idea idea = _context.Ideas.First(i => i.Id == ideaId);
        if (user != null)
        {
            _context.Reactions.Add(new Reaction
            {
                Text = reaction,
                IdeaId = ideaId,
                Idea = idea,
                Date = DateTime.UtcNow,
                User = user
            });
            _context.SaveChanges();
            return true;
        }
        else
        {
            _context.Reactions.Add(new Reaction
            {
                Text = reaction,
                IdeaId = ideaId,
                Idea = idea,
                Date = DateTime.UtcNow,
            });
            _context.SaveChanges();
            return true;
        }
    }

    public bool CreateIdea(int forumId, string title, string content, AnswerCubeUser user)
    {
        Forum forum = _context.Forums.Single(f => f.Id == forumId);
        // Create the new idea
        Idea newIdea = new Idea
        {
            Title = title,
            Content = content,
            ForumId = forumId,
            Forum = forum,
            User = user
        };

        _context.Ideas.Add(newIdea);
        _context.SaveChanges();
        return true;
    }

    public int ReadForumByReactionId(int reactionId)
    {
        return _context.Reactions.Include(reaction => reaction.Idea).Single(r => r.Id == reactionId).Idea.ForumId;
    }

    public bool LikeReaction(int reactionId, AnswerCubeUser user)
    {
        Reaction reaction = _context.Reactions.Include(r => r.Likes).Single(r => r.Id == reactionId);
        Like newLike = new Like
        {
            ReactionId = reactionId,
            Reaction = reaction,
            UserId = user.Id,
            User = user
        };
        //Remove the dislike
        Dislike? dislike = _context.Dislikes.SingleOrDefault(d => d.ReactionId == reactionId && d.UserId == user.Id);
        if (dislike != null)
        {
            _context.Dislikes.Remove(dislike);
        }

        reaction.Likes.Add(newLike);
        _context.SaveChanges();
        return true;
    }

    public bool DislikeReaction(int reactionId, AnswerCubeUser user)
    {
        Reaction reaction = _context.Reactions.Include(r => r.Dislikes).Single(r => r.Id == reactionId);
        Dislike newDislike = new Dislike
        {
            ReactionId = reactionId,
            Reaction = reaction,
            UserId = user.Id,
            User = user
        };
        //Remove the like
        Like? like = _context.Likes.SingleOrDefault(d => d.ReactionId == reactionId && d.UserId == user.Id);
        if (like != null)
        {
            _context.Likes.Remove(like);
        }

        reaction.Dislikes.Add(newDislike);
        _context.SaveChanges();
        return true;
    }

    public bool LikeIdea(int ideaId, AnswerCubeUser user)
    {
        Idea idea = _context.Ideas.Include(i => i.Likes).Single(i => i.Id == ideaId);
        Like newLike = new Like
        {
            IdeaId = ideaId,
            Idea = idea,
            UserId = user.Id,
            User = user
        };
        //Remove the dislike
        Dislike? dislike = _context.Dislikes.SingleOrDefault(d => d.IdeaId == ideaId && d.UserId == user.Id);
        if (dislike != null)
        {
            _context.Dislikes.Remove(dislike);
        }

        idea.Likes.Add(newLike);
        _context.SaveChanges();
        return true;
    }

    public bool DislikeIdea(int ideaId, AnswerCubeUser user)
    {
        Idea idea = _context.Ideas.Include(i => i.Dislikes).Single(i => i.Id == ideaId);
        Dislike newDislike = new Dislike
        {
            IdeaId = ideaId,
            Idea = idea,
            UserId = user.Id,
            User = user
        };
        //Remove the like
        Like? like = _context.Likes.SingleOrDefault(d => d.IdeaId == ideaId && d.UserId == user.Id);
        if (like != null)
        {
            _context.Likes.Remove(like);
        }

        idea.Dislikes.Add(newDislike);
        _context.SaveChanges();
        return true;
    }

}