using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class ReviewSTUB : IReviewData
{
    static AccountSTUB _accountStub = new();
    static CommentSTUB _commentStub = new();
    
    public List<ReviewDTO> Reviews = new()
    {
        new(1, "This is a review title", "This is a review", _accountStub.Accounts[0], 5, _commentStub.GetAllByReviewId(1)),
        new(2, "This is another review title", "This is another review", _accountStub.Accounts[1], 4, _commentStub.GetAllByReviewId(2)),
    };

    public bool Add(ReviewDTO entity)
    {
        Reviews.Add(entity);
        return Reviews.Exists(entity.Equals);
    }

    public ReviewDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public ReviewDTO Update(ReviewDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ReviewDTO> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ReviewDTO> GetAllByBookId(long bookId)
    {
        return Reviews.Where(reviews => reviews.BookId == bookId);
    }
}