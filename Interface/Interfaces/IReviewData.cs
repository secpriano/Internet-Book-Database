using Interface.DTO;

namespace Interface.Interfaces;

public interface IReviewData : IBase<ReviewDTO>
{
    public IEnumerable<ReviewDTO> GetAllByBookId(long bookId);
}