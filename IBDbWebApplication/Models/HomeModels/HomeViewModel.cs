using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.HomeModels;

public class HomeViewModel
{
    public IEnumerable<BookModel> BookModels { get; set; } = new List<BookModel>();

    public HomeViewModel(IEnumerable<BookModel> bookModels)
    {
        BookModels = bookModels;
    }
}