namespace Interface.DTO;

public record CommentDTO(long Id, long ReviewId, string Content, long UserId)
{
    public CommentDTO(long commentId, string commentContent, long commentUserId) : this(commentId, 0, commentContent, commentUserId)
    {
    }
}