using Business.Entity;
using Interface.DTO;
using Interface.Interfaces;

namespace Business.Container;

public class BookContainer
{
    private readonly IBookData _bookData;
    Validate Validate = new();

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
    
    public bool Unfavorite(long bookId, long accountId)
    {
        return _bookData.Unfavorite(bookId, accountId);
    }
    
    public bool IsFavorite(long bookId, long accountId)
    {
        return _bookData.IsFavorite(bookId, accountId);
    }
    
    public bool Shelf(long bookId, long accountId)
    {
        return _bookData.Shelf(bookId, accountId);
    }
    
    public bool Unshelve(long bookId, long accountId)
    {
        return _bookData.Unshelve(bookId, accountId);
    }
    
    public bool Shelved(long bookId, long accountId)
    {
        return _bookData.Shelved(bookId, accountId);
    }
    
    public IEnumerable<Book> GetAllFavoritesByAccountId(long accountId) 
    {
        return _bookData.GetAllFavoritesByAccountId(accountId).Select(dto => new Book(dto));
    }

    public IEnumerable<Book> GetAllShelvedByAccountId(long accountId)
    {
        return _bookData.GetAllShelvedByAccountId(accountId).Select(dto => new Book(dto));
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
        Validate.Regex(isbn, "^[0-9]+$", "ISBN", "ISBN must contain only numeric characters.");
        if(_bookData.Exist(isbn)) throw new KeyValueException($"ISBN {isbn} is already in use.", "ISBN");
    }
    
    private void ValidateTitle(string title)
    {
        Validate.OutOfRange((ulong)title.Length, 1, 100, "Title", Validate.Unit.Character);
        Validate.Regex(title, "^[a-zA-Z ]+$", "Title", "Title must contain only letters, and spaces.");
    }
    
    private void ValidateSynopsis(string synopsis, string title)
    {
        Validate.OutOfRange((ulong)synopsis.Length, (ulong)title.Length, 1000, "Synopsis", "title", Validate.Unit.Character);
        Validate.Regex(synopsis, "^[a-zA-Z ,'’.?!]+$", "Synopsis", "Synopsis must contain only letters, spaces, and punctuation.");
    }
    
    private void ValidatePublishDate(DateOnly publishDate, DateOnly authorBirthdate)
    {
        if (publishDate < authorBirthdate) throw new KeyValueException($"Publish date {publishDate} cannot be earlier than author's birthdate {authorBirthdate} unless you travel back in time.", "Publish date");
    }
    
    private void ValidateAmountPages(ulong amountPages) 
    {
        Validate.OutOfRange(amountPages, 1, 50000, "Amount pages", Validate.Unit.Page);
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