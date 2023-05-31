using Interface.DTO;
using Interface.Interfaces;

namespace Business.Entity;

public class Review
{
    public long? Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public long UserId { get; set; }
    public long BookId { get; set; }
    public IEnumerable<Comment>? Comments { get; set; }
    
    public Review(long? id, string title, string content, long userId, long bookId, IEnumerable<Comment>? comments) => 
        (Id, Title, Content, UserId, BookId, Comments) = (id, title, content, userId, bookId, comments);

    public Review(ReviewDTO reviewDTO) : this(reviewDTO.Id, reviewDTO.Title, reviewDTO.Content, reviewDTO.UserId, reviewDTO.BookId, reviewDTO.Comments.Select(comment => new Comment(comment)))
    {
    }
    
    public ReviewDTO ToDTO()
    {
        return new(
            Id,
            Title,
            Content,
            UserId,
            BookId,
            (Comments != null ? Comments.Select(comment => comment.ToDTO()) : null) as List<CommentDTO>
            );
    }
    
    private readonly ICommentData _commentData;

    public Review(ICommentData commentData)
    {
        _commentData = commentData;
    }

    protected Review()
    {
    }

    public bool AddComment(Comment comment)
    {
        return _commentData.Add(comment.ToDTO());
    }
}