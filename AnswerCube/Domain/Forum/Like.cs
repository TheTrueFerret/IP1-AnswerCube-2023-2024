using AnswerCube.BL.Domain.User;

namespace Domain;


    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AnswerCubeUser User { get; set; }
        public int? IdeaId { get; set; }
        public Idea Idea { get; set; }
        public int? ReactionId { get; set; }
        public Reaction Reaction { get; set; }
    }

    public class Dislike
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public AnswerCubeUser User { get; set; }
        public int? IdeaId { get; set; }
        public Idea Idea { get; set; }
        public int? ReactionId { get; set; }
        public Reaction Reaction { get; set; }
    }
