using AnswerCube.BL.Domain.User;
using Domain;

namespace AnswerCube.DAL;

public interface IForumRepository
{
    List<Forum> ReadForums();
    Forum ReadForum(int forumId);
    int ReadForumByIdeaId(int ideaId);
    bool CreateReaction(int ideaId, string reaction, AnswerCubeUser? user);
    bool CreateIdea(int forumId, string title, string content, AnswerCubeUser user);
    int ReadForumByReactionId(int reactionId);
    bool LikeReaction(int reactionId, AnswerCubeUser user);
    bool DislikeReaction(int reactionId, AnswerCubeUser user);
    bool LikeIdea(int ideaId, AnswerCubeUser user);
    bool DislikeIdea(int ideaId, AnswerCubeUser user);
}