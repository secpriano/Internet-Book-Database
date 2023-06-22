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
        [Values("0", "031", "0302", "23478723456238452", "1763951678356181993847")]
        string isbn
    )
    {
        // Arrange
        Book book = new(_bookStub.Books[bookIndex])
        {
            Isbn = isbn
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;

        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo("ISBN must be exactly 13 Character."));
        }
    }

    [Test, Combinatorial]
    [Category("ISBN")]
    public void Test_Exception_AddBook_When_Isbn_Contains_NonNumericCharacters(
        [Values(0, 1)] int bookIndex,
        [Values("G1234/7#$012a", "aG12F!K#$012a", "dke#*aQ1!hjY#")]
        string isbn
    )
    {
        // Arrange
        Book book = new(_bookStub.Books[bookIndex])
        {
            Isbn = isbn
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;

        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo("ISBN must contain only numeric characters."));
        }
    }

    [Test]
    [Category("ISBN")]
    public void Test_Exception_AddBook_When_Isbn_IsAlreadyInUse(
        [Values("2037162530194", "9780312866272")] string isbn
        )
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = isbn
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;

        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo($"ISBN {book.Isbn} is already in use."));
        }
    }

    [Test]
    [Category("Title")]
    public void Test_Exception_AddBook_When_Title_IsLonger_Than100Characters(
        [Values(101, 150, 500)] int length
        )
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            Title = new('a', length)
        };
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo($"Title must be less or equal to 100 Character. Not {book.Title.Length} Character."));
        }
    }
    
    [Test]
    [Category("Title")]
    public void Test_Exception_AddBook_When_Title_IsEmpty()
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            Title = string.Empty
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
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
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            Title = title
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
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
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            Synopsis = new('a', length)
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
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
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            Title = new('a', titleLength),
            Synopsis = new('a', synopsisLength)
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo($"Synopsis must be more than or equal to the length of title: {book.Title.Length} Character. Not {book.Synopsis.Length} Character."));
        }
    }
    
    [Test]
    [Category("Synopsis")]
    public void Test_Exception_AddBook_When_Synopsis_Contains_NonAlphabeticCharactersExcludingPunctuation(
        [Values("This is a test. 123", "This is a test. #$@", "This is a test. {}?<>{?><")] string synopsis
    )
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            Synopsis = synopsis
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
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
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            PublishDate = new(publishYear, 1, 1)
        };
        Author author = book.Authors.First();
        author.BirthDate = new(birthYear, 1, 1); 
        book.Authors = new List<Author>{author};

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo($"Publish date {book.PublishDate} cannot be earlier than author's birthdate {author.BirthDate} unless you travel back in time."));
        } 
    }
    
    [Test]
    [Category("AmountPages")]
    public void Test_Exception_AddBook_When_AmountPages_IsMore_Than50000()
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            AmountPages = 50001
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo($"Amount pages must be less or equal to 50000 Page. Not {book.AmountPages} Page."));
        }
    }
    
    [Test]
    [Category("AmountPages")]
    public void Test_Exception_AddBook_When_AmountPages_IsLess_Than1()
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123",
            AmountPages = 0
        };

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo($"Amount pages must be more than or equal to 1 Page. Not {book.AmountPages} Page."));
        }
    }
    
    [Test]
    [Category("Authors")]
    public void Test_Exception_AddBook_When_Author_IsAlreadyAdded()
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123"
        };
        book.Authors = new List<Author>
        {
            book.Authors.First(),
            book.Authors.First()
        };
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo("Author is already added."));
        }
    }
    
    [Test]
    [Category("Genre")]
    public void Test_Exception_AddBook_When_Genre_IsAlreadyAdded()
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123"
        };
        book.Genres = new List<Genre>
        {
            book.Genres.First(),
            book.Genres.First()
        };
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo("Genre is already added."));
        }
    }
    
    [Test]
    [Category("Setting")]
    public void Test_Exception_AddBook_When_Setting_IsAlreadyAdded()
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123"
        };
        book.Settings = new List<Setting>
        {
            book.Settings.First(),
            book.Settings.First()
        };
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo("Setting is already added."));
        }
    }
    
    [Test]
    [Category("Theme")]
    public void Test_Exception_AddBook_When_Theme_IsAlreadyAdded()
    {
        // Arrange
        Book book = new(_bookStub.Books[0])
        {
            Isbn = "1234567890123"
        };
        book.Themes = new List<Theme>
        {
            book.Themes.First(),
            book.Themes.First()
        };
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _bookContainer.Add(book))!;
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.That(innerException.Message, Is.EqualTo("Theme is already added."));
        }
    }

    [TearDown]
    public void TearDown()
    {
        _bookStub = null!;
    }
}