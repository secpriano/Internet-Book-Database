using Interface.DTO;

namespace Business.Entity;

public class Comment
{
    public long? Id { get; }
    public string Content { get; }
    public Account Account { get; }
    public long ReviewId { get; set; }

    public Comment(long? id, string content, Account account, long reviewId) =>
        (Id, Content, Account, ReviewId) = (id, content, account, reviewId);

    public Comment(CommentDTO commentDTO) : this(commentDTO.Id, commentDTO.Content, new(commentDTO.Account), commentDTO.ReviewId)
    {
    }
    
    public CommentDTO ToDTO()
    {
        return new(Id, Content, Account.ToDTO(), ReviewId);
    }
}