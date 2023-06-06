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
        
        return _bookData.Add(book.ToDto());
    }

    public Book Update(Book book)
    {
        ValidateBook(book);
        
        return new(_bookData.Update(book.ToDto()));
    }

    public bool Delete(long id) => _bookData.Delete(id);
    
    public Book GetById(long id)
    {
        return new(_bookData.GetById(id));
    }
    
    public bool Favorite(long bookId, long userId)
    {
        return _bookData.Favorite(bookId, userId);
    }
    
    public bool Unfavorite(long bookId, long userId)
    {
        return _bookData.Unfavorite(bookId, userId);
    }
    
    public bool IsFavorite(long bookId, long userId)
    {
        return _bookData.IsFavorite(bookId, userId);
    }
    
    private void ValidateBook(Book book)
    {
        try
        {
            Task[] tasks = {
                Task.Run(() => ValidateIsbn(book.Isbn)),
                Task.Run(() => ValidateTitle(book.Title)),
                Task.Run(() => ValidateSynopsis(book.Synopsis, book.Title)),
                Task.Run(() => ValidatePublishDate(book.PublishDate, book.Authors.Select(author => author.BirthDate).Min())),
                Task.Run(() => ValidateAmountPages(book.AmountPages)),
                Task.Run(() => ValidateAuthors(book.Authors)),
                Task.Run(() => ValidateGenres(book.Genres)),
                Task.Run(() => ValidateSettings(book.Settings)),
                Task.Run(() => ValidateThemes(book.Themes))
            };
            
            Task.WaitAll(tasks);
        }
        catch (AggregateException ex)
        {
            throw new AggregateException(ex.InnerExceptions);
        }
    }
    
    private void ValidateIsbn(string isbn)
    {
        Validate.ExactValue((ulong)isbn.Length, 13, "ISBN", Validate.Unit.Character);

        Validate.Regex(isbn, "^[0-9]+$", "ISBN must contain only numeric characters.");
    }
    
    private void ValidateTitle(string title)
    {
        Validate.OutOfRange((ulong)title.Length, 1, 100, "Title", Validate.Unit.Character);
        
        Validate.Regex(title, "^[a-zA-Z ]+$", "Title must contain only letters, and spaces.");
    }
    
    private void ValidateSynopsis(string synopsis, string title)
    {
        Validate.OutOfRange((ulong)synopsis.Length, (ulong)title.Length, 1000, "Synopsis", Validate.Unit.Character);

        Validate.Regex(synopsis, "^[a-zA-Z ,'’.?!]+$", "Synopsis must contain only letters, spaces, and punctuation.");
    }
    
    private void ValidatePublishDate(DateOnly publishDate, DateOnly authorBirthdate)
    {
        if (publishDate < authorBirthdate) throw new($"Publish date {publishDate} cannot be earlier than author's birthdate {authorBirthdate} unless you travel back in time.");
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