using Business.Entity;
using Interface.DTO;
using Interface.Interfaces;

namespace Business.Container;

public class BookContainer
{
    private readonly IBookData _bookData;
    public BookContainer(IBookData bookData)
    {
        _bookData = bookData;
    }

    public IEnumerable<Book> GetAll()
    {
        return _bookData.GetAll().Select(dto => new Book(dto));
    }

    public bool Add(Book book)
    {
        ValidateBook(book);
        
        return _bookData.Add(book.GetDto());
    }

    public BookDTO Update(Book book)
    {
        ValidateBook(book);
        
        return _bookData.Update(book.GetDto());
    }

    public bool Delete(long id) => _bookData.Delete(id);
    
    private void ValidateBook(Book book)
    {
        ValidateIsbn(book.Isbn);
        ValidateTitle(book.Title);
        ValidateSynopsis(book.Synopsis, book.Title);
        ValidatePublishDate(book.PublishDate, book.Authors.Select(author => author.BirthDate).Min());
        ValidateAmountPages(book.AmountPages);
        ValidateAuthors(book.Authors);
        ValidateGenres(book.Genres);
        ValidateSettings(book.Settings);
        ValidateThemes(book.Themes);
        
        if (Validate.Exceptions.InnerExceptions.Count > 0)
        {
            throw Validate.Exceptions;
        }
    }
    
    private void ValidateIsbn(string isbn)
    {
        Validate.ExactValue((ulong)isbn.Length, 13, "ISBN", Validate.Unit.Character);

        Validate.Regex(isbn, "^[0-9]+$", "ISBN must only contain numbers.");
    }
    
    private void ValidateTitle(string title)
    {
        Validate.OutOfRange((ulong)title.Length, 1, 100, "Title", Validate.Unit.Character);
        
        Validate.Regex(title, "^[a-zA-Z ]+$", "Title must only contain letters, and spaces.");
    }
    
    private void ValidateSynopsis(string synopsis, string title)
    {
        Validate.OutOfRange((ulong)synopsis.Length, (ulong)title.Length, 1000, "Synopsis", Validate.Unit.Character);

    Validate.Regex(synopsis, "^[a-zA-Z ,.?!]+$", "Synopsis must only contain letters, spaces, and punctuation.");
    }
    
    private void ValidatePublishDate(DateTime publishDate, DateTime authorBirthdate)
    {
        if (publishDate < authorBirthdate)
            Validate.Exceptions.InnerExceptions.Append(new("Publish date cannot be earlier than author's birthdate unless you travel back in time."));
    }
    
    private void ValidateAmountPages(ulong amountPages)
    {
        Validate.OutOfRange(amountPages, 1, 50000, "Amount of pages", Validate.Unit.Page);
    }
    
    private void ValidateAuthors(IEnumerable<Author> authors)
    {
        Validate.Duplicate(authors, "Author");
    }
    
    private void ValidateGenres(IEnumerable<Genre> genres)
    {
        Validate.Duplicate(genres, "Genre");
    }
    
    private void ValidateSettings(IEnumerable<Setting> settings)
    {
        Validate.Duplicate(settings, "Setting");
    }
    
    private void ValidateThemes(IEnumerable<Theme> themes)
    {
        Validate.Duplicate(themes, "Theme");
    }
}