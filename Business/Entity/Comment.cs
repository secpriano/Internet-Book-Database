using Interface.DTO;

namespace Business.Entity;

public class Comment
{
    public long Id { get; set; }
    public string Content { get; set; }
    public long UserId { get; set; }

    public Comment(long id, string content, long userId) =>
        (Id, Content, UserId) = (id, content, userId);

    public Comment(CommentDTO commentDTO) : this(commentDTO.Id, commentDTO.Content, commentDTO.UserId)
    {
    }
    
    public CommentDTO ToDTO()
    {
        return new(Id, Content, UserId);
    }
}