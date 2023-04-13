using System.Collections.ObjectModel;
using Business.Entity;
using Interface.DTO;
using Interface.Interfaces;

namespace Business.Container;

public class BookContainer
{
    private readonly IBookData _bookData;
    private Collection<ArgumentException?> _exceptions;
    public BookContainer(IBookData bookData)
    {
        _bookData = bookData;
        _exceptions = new();
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
        ValidateDuplicate(book.Authors);
        ValidateDuplicate(book.Genres);
        ValidateDuplicate(book.Settings);
        ValidateDuplicate(book.Themes);
        
        if (_exceptions.Count > 0)
        {
            throw new AggregateException(_exceptions);
        }
    }
    
    private void ValidateIsbn(string isbn)
    {
        _exceptions.Add(Validate.ExactValue((ulong)isbn.Length, 13, "ISBN", "character"));

        _exceptions.Add(Validate.ValidateExpression(isbn, "^[0-9]+$", "ISBN must only contain numbers."));
    }
    
    private void ValidateTitle(string title)
    {
        _exceptions.Add(Validate.OutOfRange((ulong)title.Length, 1, 100 ,"Title", "character"));
        
        _exceptions.Add(Validate.ValidateExpression(title, "^[a-zA-Z ]+$", "Title must only contain letters, and spaces."));
    }
    
    private void ValidateSynopsis(string synopsis, string title)
    {
        _exceptions.Add(Validate.OutOfRange((ulong)synopsis.Length, (ulong)title.Length, 1000, "Synopsis", "character"));

        _exceptions.Add(Validate.ValidateExpression(synopsis, "^[a-zA-Z ,.?!]+$", "Synopsis must only contain letters, spaces, and punctuation."));
    }
    
    private void ValidatePublishDate(DateTime publishDate, DateTime authorBirthdate)
    {
        if (publishDate < authorBirthdate)
            _exceptions.Add(new("Publish date cannot be earlier than author's birthdate unless you travel back in time."));
    }
    
    private void ValidateAmountPages(ulong amountPages)
    {
        _exceptions.Add(Validate.OutOfRange(amountPages, 1, 50000, "Amount of pages", "page"));
    }

    private void ValidateDuplicate<T>(IEnumerable<T> entities)
    {
        byte count = 0;
        
        for (int i = 0; i < entities.Count(); i++)
        {
            for (int j = 0; j < entities.Count(); j++)
            {
                switch (entities)
                {
                    case IEnumerable<Author> authors:
                        if (i != j && authors.ElementAt(i).ThisEquals(authors.ElementAt(j)))
                            _exceptions.Add(new("Author is already added."));
                        break;
                    case IEnumerable<Genre> genres:
                        if (i != j && genres.ElementAt(i).ThisEquals(genres.ElementAt(j)))
                            _exceptions.Add(new("Genre is already added."));
                        break;
                    case IEnumerable<Setting> settings:
                        if (i != j && settings.ElementAt(i).ThisEquals(settings.ElementAt(j)))
                            _exceptions.Add(new("Setting is already added."));
                        break;
                    case IEnumerable<Theme> themes:
                        if (i != j && themes.ElementAt(i).ThisEquals(themes.ElementAt(j)))
                            _exceptions.Add(new("Theme is already added."));
                        break;
                }
            }
        }
    }
}