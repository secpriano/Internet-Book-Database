using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookDetailModel
{
    public BookModel BookModel { get; set; }

    public BookDetailModel(BookModel bookModel)
    {
        BookModel = bookModel;
    }
}