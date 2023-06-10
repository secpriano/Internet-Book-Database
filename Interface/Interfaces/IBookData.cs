using Interface.DTO;

namespace Interface.Interfaces;

public interface IBookData : IBase<BookDTO>, IValidateData
{
    bool Favorite(long bookId, long userId);   
    bool Unfavorite(long bookId, long accountId);
    bool IsFavorite(long bookId, long accountId);
}