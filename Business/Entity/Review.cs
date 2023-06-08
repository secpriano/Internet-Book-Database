using Business.Container;
using Interface.DTO;
using Interface.Interfaces;

namespace Business.Entity;

public class Review
{
    public long? Id { get; }
    public string Title { get; }
    public string Content { get; }
    public Account Account { get; }
    public long BookId { get; }
    public IEnumerable<Comment>? Comments { get; }
    
    public Review(long? id, string title, string content, Account account, long bookId, IEnumerable<Comment>? comments) => 
        (Id, Title, Content, Account, BookId, Comments) = (id, title, content, account, bookId, comments);

    public Review(ReviewDTO reviewDTO) : this(reviewDTO.Id, reviewDTO.Title, reviewDTO.Content, new(reviewDTO.Account), reviewDTO.BookId, reviewDTO.Comments.Select(comment => new Comment(comment)))
    {
    }
    
    public ReviewDTO ToDTO()
    {
        return new(
            Id,
            Title,
            Content,
            Account.ToDTO(),
            BookId,
            (Comments != null ? Comments.Select(comment => comment.ToDTO()) : null) as List<CommentDTO>
            );
    }
    
    private readonly ICommentData _commentData;

    public Review(ICommentData commentData)
    {
        _commentData = commentData;
    }

    public bool AddComment(Comment comment)
    {
        ValidateComment(comment);
        
        return _commentData.Add(comment.ToDTO());
    }

    private void ValidateComment(Comment comment)
    {
        try
        {
            Task[] tasks = {
                Task.Run(() => ValidateContent(comment.Content))
            };

            Task.WaitAll(tasks);
        }
        catch (AggregateException ex)
        {
            throw new AggregateException(ex.InnerExceptions);
        }
    }

    private void ValidateContent(string commentContent)
    {
        Validate.OutOfRange((ulong)commentContent.Length, 2, 4000, "Content", Validate.Unit.Character);
    }
}