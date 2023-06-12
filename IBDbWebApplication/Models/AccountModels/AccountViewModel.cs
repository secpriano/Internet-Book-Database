using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AccountModels;

public class AccountViewModel
{
    public AccountModel AccountModel { get; set; }
    public IEnumerable<BookModel> FavoriteBookModels { get; set; }
    
    public AccountViewModel(AccountModel accountModel, IEnumerable<BookModel> favoriteBooks) =>
        (AccountModel, FavoriteBookModels) = (accountModel, favoriteBooks);
}