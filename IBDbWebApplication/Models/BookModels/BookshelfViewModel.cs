using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookshelfViewModel
{
    public IEnumerable<BookModel> ShelvedBookModels { get; set; } = new List<BookModel>();

    public BookshelfViewModel(IEnumerable<BookModel> shelvedBookModels) => ShelvedBookModels = shelvedBookModels;

    public BookshelfViewModel() { }
}