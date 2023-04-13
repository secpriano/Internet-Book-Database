namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookViewModel
{
    public List<BookModel> Books { get; set; }

    public BookViewModel(List<BookModel> books) =>
    Books = books;
}