using Business.Container;
using Interface.DTO;
using Test.STUB;

namespace Test.Book;
using Business.Entity;

[TestFixture]
public class TestException
{
    private BookSTUB _bookStub = null!;
    private BookContainer _bookContainer;

    [SetUp]
    public void Setup()
    {
        _bookStub = new();
        _bookContainer = new(_bookStub);
    }

    [Test, Combinatorial]
    [Category("ISBN")]
    public void Test_Exception_AddBook_When_Isbn_IsNot_Exactly13Characters(
        [Values(0, 1)] int bookIndex,
        [Values("0", "031", "0302", "23478723456238452", "1763951678356181993847")] string isbn
        )
    {
        // Arrange
        BookDTO book = _bookStub.Books[bookIndex];
        
        book.Isbn = isbn;
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo("ISBN must be exactly 13 Character."));
        }
    }
    
    [Test, Combinatorial]
    [Category("ISBN")]
    public void Test_Exception_AddBook_When_Isbn_Contains_NonNumericCharacters(
        [Values(0, 1)] int bookIndex,
        [Values("G1234/7#$012a", "aG12F!K#$012a", "dke#*aQ1!hjY#")] string isbn
        )
    {
        // Arrange
        BookDTO book = _bookStub.Books[bookIndex];
        
        book.Isbn = isbn;
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception exception in aggregateException.InnerExceptions)
        {
            Assert.That(exception, Is.TypeOf<Exception>());
            Assert.That(exception.Message, Is.EqualTo("ISBN must contain only numeric characters."));
        }
        
    }
    
    [Test]
    [Category("Title")]
    public void Test_Exception_AddBook_When_Title_IsLonger_Than100Characters(
        [Values(101, 150, 500)] int length
        )
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Title = new('a', length);
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo($"Title must be less or equal to 100 Character. Not {book.Title.Length} Character."));
        }
    }
    
    [Test]
    [Category("Title")]
    public void Test_Exception_AddBook_When_Title_IsEmpty()
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Title = string.Empty;
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo($"Title must be more than or equal to 1 Character. Not {book.Title.Length} Character."));
        }
    }
    
    [Test]
    [Category("Title")]
    public void Test_Exception_AddBook_When_Title_Contains_NonAlphabeticCharacters(
        [Values("This is a test. 123", "This is a test. #$@", "This is a test. {}?<>{?><")] string title
        )
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];

        book.Title = title;
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo("Title must contain only letters, and spaces."));
        }
    }
    
    [Test]
    [Category("Synopsis")]
    public void Test_Exception_AddBook_When_Synopsis_IsLonger_Than1000Characters(
        [Values(1001, 2000, 10000)] int length
    )
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Synopsis = new('a', length);
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo($"Synopsis must be less or equal to 1000 Character. Not {book.Synopsis.Length} Character."));
        }
    }

    [Test]
    [Category("Synopsis")]
    public void Test_Exception_AddBook_When_Synopsis_IsLess_CharactersThanTitle(
        [Random(50, 100, 5)] int titleLength,
        [Random(0, 49, 5)] int synopsisLength
    )
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Title = new('a', titleLength);
        book.Synopsis = new('a', synopsisLength);
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo($"Synopsis must be more than or equal to {book.Title.Length} Character. Not {book.Synopsis.Length} Character."));
        }
    }
    
    [Test]
    [Category("Synopsis")]
    public void Test_Exception_AddBook_When_Synopsis_Contains_NonAlphabeticCharactersExcludingPunctuation(
        [Values("This is a test. 123", "This is a test. #$@", "This is a test. {}?<>{?><")] string synopsis
    )
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Synopsis = synopsis;
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo("Synopsis must contain only letters, spaces, and punctuation."));
        }
    }
    
    [Test]
    [Category("PublishDate")]
    public void Test_Exception_AddBook_When_PublishDate_IsEarlier_ThanAuthorBirthdate(
        [Random(1, 1000, 5)] int publishYear,
        [Random(1001, 2000, 5)] int birthYear
        )
    {
        // Arrange
        Book book = new(_bookStub.Books.First());
        IEnumerable<Author> authors = new List<Author>
        {
            book.Authors.First()
        };
        
        book.PublishDate = new(publishYear, 1, 1);
        authors.First().BirthDate = new(birthYear, 1, 1);
        
        book.Authors = authors;

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo($"Publish date {book.PublishDate} cannot be earlier than author's birthdate {authors.First().BirthDate} unless you travel back in time."));
        } 
    }
    
    [Test]
    [Category("AmountPages")]
    public void Test_Exception_AddBook_When_AmountPages_IsMore_Than50000()
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.AmountPages = 50001;
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo($"Amount of pages must be less or equal to 50000 Page. Not {book.AmountPages} Page."));
        }
    }
    
    [Test]
    [Category("AmountPages")]
    public void Test_Exception_AddBook_When_AmountPages_IsLess_Than1()
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.AmountPages = 0;
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo($"Amount of pages must be more than or equal to 1 Page. Not {book.AmountPages} Page."));
        }
    }
    
    [Test]
    [Category("Authors")]
    public void Test_Exception_AddBook_When_Author_IsAlreadyAdded()
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Authors = new List<AuthorDTO>
        {
            book.Authors.First(),
            book.Authors.First()
        };
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo("Author is already added."));
        }
    }
    
    [Test]
    [Category("Genre")]
    public void Test_Exception_AddBook_When_Genre_IsAlreadyAdded()
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Genres = new List<GenreDTO>
        {
            book.Genres.First(),
            book.Genres.First()
        };
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo("Genre is already added."));
        }
    }
    
    [Test]
    [Category("Setting")]
    public void Test_Exception_AddBook_When_Setting_IsAlreadyAdded()
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Settings = new List<SettingDTO>
        {
            book.Settings.First(),
            book.Settings.First()
        };
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo("Setting is already added."));
        }
    }
    
    [Test]
    [Category("Theme")]
    public void Test_Exception_AddBook_When_Theme_IsAlreadyAdded()
    {
        // Arrange
        BookDTO book = _bookStub.Books[0];
        
        book.Themes = new List<ThemeDTO>
        {
            book.Themes.First(),
            book.Themes.First()
        };
        
        // Act
        AggregateException exception = Assert.Throws<AggregateException>(() => _bookContainer.Add(new(book)))!;
        
        // Assert
        foreach (Exception innerException in exception.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.That(innerException.Message, Is.EqualTo("Theme is already added."));
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        _bookStub = null!;
    }
}