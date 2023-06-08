namespace Interface.DTO;

public record CommentDTO(long? Id, string Content, AccountDTO Account, long ReviewId)
{
    public CommentDTO(string commentContent, AccountDTO account, long reviewId) : this(null, commentContent, account, reviewId) { }
}