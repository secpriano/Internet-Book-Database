using Interface.DTO;

namespace Business.Entity;

public class Comment
{
    public long Id { get; }
    public long ReviewId { get; }
    public string Content { get; }
    public long UserId { get; }

    public Comment(long id, long reviewId, string content, long userId) =>
        (Id, ReviewId, Content, UserId) = (id, reviewId, content, userId);

    public Comment(CommentDTO commentDTO) : this(commentDTO.Id, commentDTO.ReviewId, commentDTO.Content, commentDTO.UserId)
    {
    }
    
    public CommentDTO ToDTO()
    {
        return new(Id, ReviewId, Content, UserId);
    }
}