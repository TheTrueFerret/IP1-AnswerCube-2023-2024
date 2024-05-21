using AnswerCube.BL.Domain.User;
using Domain;

namespace AnswerCube.BL;

public interface IForumManager
{
    List<Forum> GetForums();
    Forum GetForum(int forumId);
    bool AddIdea(int forumId, string title, string content, AnswerCubeUser user);
    bool AddReaction(int ideaId, string reaction,AnswerCubeUser? user);
    int GetForumByIdeaId(int ideaId);
    int GetForumByReactionId(int reactionId);
    bool LikeReaction(int reactionId,AnswerCubeUser user);
    bool DislikeReaction(int reactionId,AnswerCubeUser user);
    bool LikeIdea(int ideaId,AnswerCubeUser user);
    bool DislikeIdea(int ideaId,AnswerCubeUser user);
}