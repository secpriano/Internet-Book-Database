using Interface.DTO;

namespace Interface.Interfaces;

public interface IBookData : IBase<BookDTO>
{
    bool Favorite(long bookId, long userId);   
    bool Unfavorite(long bookId, long userId);
    bool IsFavorite(long bookId, long userId);
}