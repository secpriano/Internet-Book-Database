using Interface.DTO;

namespace Interface.Interfaces;

public interface IBookData : IBase<BookDTO>, IValidateData
{
    bool Favorite(long bookId, long userId);   
    bool Unfavorite(long bookId, long accountId);
    bool IsFavorite(long bookId, long accountId);
    IEnumerable<BookDTO> GetAllFavoritesByAccountId(long accountId);
    bool Shelf(long bookId, long accountId);
    bool Unshelve(long bookId, long accountId);
    bool Shelved(long bookId, long accountId);
    IEnumerable<BookDTO> GetAllShelvedByAccountId(long accountId);
}