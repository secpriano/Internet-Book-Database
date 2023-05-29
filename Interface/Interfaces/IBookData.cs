using Interface.DTO;

namespace Interface.Interfaces;

public interface IBookData : IBase<BookDTO>
{
    bool Favorite(long bookId, long userId);   
}