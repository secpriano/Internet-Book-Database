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
        return _reviewData.Add(review.ToDTO());
    }
    
    public IEnumerable<Review> GetAllByBookId(long bookId)
    {
        return _reviewData.GetAllByBookId(bookId).Select(reviewDTO => new Review(reviewDTO));
    }
}