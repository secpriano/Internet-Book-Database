using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class ReviewContainer
{
    private readonly IReviewData _reviewData;

    public ReviewContainer(IReviewData reviewData)
    {
        _reviewData = reviewData;
    }
    
    public bool Add(Review review)
    {
        ValidateReview(review);
        
        return _reviewData.Add(review.ToDTO());
    }

    public IEnumerable<Review> GetAllByBookId(long bookId)
    {
        return _reviewData.GetAllByBookId(bookId).Select(reviewDTO => new Review(reviewDTO));
    }
    
    private void ValidateReview(Review review)
    {
        try
        {
            Task[] tasks = {
                Task.Run(() => ValidateTitle(review.Title)),
                Task.Run(() => ValidateContent(review.Content))
            };

            Task.WaitAll(tasks);
        }
        catch (AggregateException ex)
        {
            throw new AggregateException(ex.InnerExceptions);
        }
    }
    
    private void ValidateTitle(string reviewTitle)
    {
        Validate.OutOfRange((ulong)reviewTitle.Length, 1, 100, "Title", Validate.Unit.Character);
        Validate.Regex(reviewTitle, "^[a-zA-Z0-9 &]+$", "Title must only contain letters, numbers, spaces, and ampersand.");
    }

    private void ValidateContent(string reviewContent)
    {
        Validate.OutOfRange((ulong)reviewContent.Length, 2, 2000, "Content", Validate.Unit.Character);
    }
}