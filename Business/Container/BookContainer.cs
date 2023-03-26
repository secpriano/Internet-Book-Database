using System.Collections;
using Business.Entity;
using Interface.DTO;
using Interface.Interfaces;

namespace Business.Container;

public readonly struct BookContainer
{
    private readonly IBookData _bookData;
    
    public BookContainer(IBookData bookData)
    {
        _bookData = bookData;
    }   
    
    public bool Add(Book book)
    {
        ValidateBook(in book);
        
        return _bookData.Add(book.GetDto());
    }

    public BookDTO Update(Book book)
    {
        ValidateBook(in book);
        
        return _bookData.Update(book.GetDto());
    }

    public bool Delete(long id) => _bookData.Delete(id);
    
    private static void ValidateBook(in Book book)
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
    }
    
    private static void ValidateIsbn(string isbn)
    {
        if (isbn.Length is not 13)
        {
            throw new ArgumentException("ISBN must be exactly 13 characters.");
        }
        
        if (isbn.Any(character => !char.IsDigit(character)))
        {
            throw new ArgumentException("ISBN must only contain numbers.");
        }
    }
    
    private static void ValidateTitle(string title)
    {
        if (title.Length > 100)
        {
            throw new ArgumentException("Title must be less or equal to 100 characters.");
        }

        if (title.Length < 1)
        {
            throw new ArgumentException("Title must be more than 1 character.");
        }
        
        if (title.Any(character => !char.IsLetterOrDigit(character)  && !char.IsWhiteSpace(character)))
        {
            throw new ArgumentException("Title must only contain letters, and spaces.");
        }
    }
    
    private static void ValidateSynopsis(string synopsis, string title)
    {
        if (synopsis.Length > 1000)
        {
            throw new ArgumentException("Synopsis must be less or equal to 1000 characters.");
        }
        
        if (synopsis.Length < title.Length)
        {
            throw new ArgumentException("Synopsis must have more characters than the title.");
        }
        
        if (synopsis.Any(character => !char.IsLetter(character) && !char.IsWhiteSpace(character) && !char.IsPunctuation(character)))
        {
            throw new ArgumentException("Synopsis must only contain letters, and punctuation.");
        }
    }
    
    private static void ValidatePublishDate(DateTime publishDate, DateTime authorBirthdate)
    {
        if (publishDate < authorBirthdate)
        {
            throw new ArgumentException("Publish date cannot be earlier than author's birthdate unless you travel back in time.");
        }
    }
    
    private static void ValidateAmountPages(ulong amountPages)
    {
        if (amountPages > 50000)
        {
            throw new ArgumentException("Amount of pages must be less or equal to 50000.");
        }
        
        if (amountPages < 1)
        {
            throw new ArgumentException("Amount of pages must be more than 0.");
        }
    }

    private static void ValidateDuplicate<T>(IEnumerable<T> entities)
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
                            throw new ArgumentException("Author is already added.");
                        break;
                    case IEnumerable<Genre> genres:
                        if (i != j && genres.ElementAt(i).ThisEquals(genres.ElementAt(j)))
                            throw new ArgumentException("Genre is already added.");
                        break;
                    case IEnumerable<Setting> settings:
                        if (i != j && settings.ElementAt(i).ThisEquals(settings.ElementAt(j)))
                            throw new ArgumentException("Setting is already added.");
                        break;
                    case IEnumerable<Theme> themes:
                        if (i != j && themes.ElementAt(i).ThisEquals(themes.ElementAt(j)))
                            throw new ArgumentException("Theme is already added.");
                        break;
                }
            }
        }
    }
}