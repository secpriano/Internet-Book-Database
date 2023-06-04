using Business.Container;
using Interface.DTO;
using Test.STUB;

namespace Test.Book;
using Business.Entity;

[TestFixture]
public class TestOperations
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
    [Category("Create")]
    public void TestAdd(
        [Values("9780312866272", "3957461037136", "0038400470000")] string isbn,
        [Values("Lord ofthe Rings", "Harry Potter", "Star Wars")] string title,
        [Values("Lord of the Rings beautiful sypnosis!", "Harry Potter? beautiful sypnosis.", "StarWarsbeautifulsypnosis")] string synopsis
        )
    {
        // Arrange
        AuthorSTUB authorStub = new();
        PublisherSTUB publisherStub = new();
        GenreSTUB genreStub = new();
        ThemeSTUB themeStub = new();
        SettingSTUB settingStub = new();

        PublisherDTO publisher = publisherStub.Publishers[1];

        Book expectedBook = new(
            _bookStub.Books.Count + 1,
            isbn,
            title,
            synopsis,
            new(1990, 1, 1),
            800,
            authorStub.Authors.FindAll(author => author is { Id: 1 })
                .Select(author => new Author(author.Id, author.Name, author.Description, author.BirthDate, 
                    author.DeathDate, new List<Genre>())),
            new(publisher.Id, publisher.Name, publisher.FoundingDate, publisher.Description),
            genreStub.Genres.FindAll(genre => genre is { Id: 1 } or { Id: 3 })
                .Select(genre => new Genre(genre.Id, genre.Name)),
            themeStub.Themes.FindAll(theme => theme is { Id: 1 } or { Id: 2 })
                .Select(theme => new Theme(theme.Id, theme.Description)),
            settingStub.Settings.FindAll(setting => setting is { Id: 1 } or { Id: 2 })
                .Select(setting => new Setting(setting.Id, setting.Description)),
                0
        );

        // Act
        bool actual = _bookContainer.Add(expectedBook); 
        
        // Assert
        BookDTO expectedBookDto = expectedBook.ToDto();
        
        Assert.Multiple(() =>
        {
            Assert.That(_bookStub.Books, Is.Not.Null);
            Assert.That(_bookStub.Books, Is.Not.Empty);
            Assert.That(actual);   
            Assert.That(_bookStub.Books.Exists(book => 
                book.Id == expectedBookDto.Id &&
                book.Isbn == expectedBookDto.Isbn &&
                book.Title == expectedBookDto.Title &&
                book.Synopsis == expectedBookDto.Synopsis &&
                book.PublishDate == expectedBookDto.PublishDate &&
                book.AmountPages == expectedBookDto.AmountPages &&
                book.Authors.SequenceEqual(expectedBookDto.Authors, new AuthorComparer()) &&
                book.Publisher == expectedBookDto.Publisher &&
                book.Genres.SequenceEqual(expectedBookDto.Genres, new GenreComparer()) &&
                book.Themes.SequenceEqual(expectedBookDto.Themes, new ThemeComparer()) &&
                book.Settings.SequenceEqual(expectedBookDto.Settings, new SettingComparer())
                )
            );
        });
    }

    [Test]
    [Category("Update")]
    public void TestUpdate(
        [Values(1, 2)] long expectedId,
        [Random(1, 50000, 5)] ushort expectedAmountPages
        )
    {
        // Arrange
        const string expectedSynopsis = "This synopsis has been updated";
        
        Book expectedBook = new(_bookStub.Books.Find(book => book.Id == expectedId))
        {
            AmountPages = expectedAmountPages,
            Synopsis = expectedSynopsis
        };

        // Act
        BookDTO actual = _bookContainer.Update(expectedBook).ToDto();
        
        // Assert
        BookDTO expectedBookDto = expectedBook.ToDto();
        
        Assert.Multiple(() =>
        {
            Assert.That(_bookStub.Books.Any(book =>
                    book.Id == expectedBookDto.Id &&
                    book.Isbn == expectedBookDto.Isbn &&
                    book.Title == expectedBookDto.Title &&
                    book.Synopsis == expectedBookDto.Synopsis &&
                    book.PublishDate == expectedBookDto.PublishDate &&
                    book.AmountPages == expectedBookDto.AmountPages &&
                    book.Authors.SequenceEqual(expectedBookDto.Authors, new AuthorComparer()) &&
                    book.Publisher == expectedBookDto.Publisher &&
                    book.Genres.SequenceEqual(expectedBookDto.Genres, new GenreComparer()) &&
                    book.Themes.SequenceEqual(expectedBookDto.Themes, new ThemeComparer()) &&
                    book.Settings.SequenceEqual(expectedBookDto.Settings, new SettingComparer())
                ), Is.True
            );
            Assert.That(
                actual.Id == expectedBookDto.Id &&
                actual.Isbn == expectedBookDto.Isbn &&
                actual.Title == expectedBookDto.Title &&
                actual.Synopsis == expectedBookDto.Synopsis &&
                actual.PublishDate == expectedBookDto.PublishDate &&
                actual.AmountPages == expectedBookDto.AmountPages &&
                actual.Authors.SequenceEqual(expectedBookDto.Authors, new AuthorComparer()) &&
                actual.Publisher == expectedBookDto.Publisher &&
                actual.Genres.SequenceEqual(expectedBookDto.Genres, new GenreComparer()) &&
                actual.Themes.SequenceEqual(expectedBookDto.Themes, new ThemeComparer()) &&
                actual.Settings.SequenceEqual(expectedBookDto.Settings, new SettingComparer()),
                Is.True
            );
        });
    }

    [Test]
    [Category("Delete")]
    public void TestDelete(
        [Values(1, 2)] long expectedId
        )
    {
        // Act
        bool actual = _bookContainer.Delete(expectedId);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(actual, Is.True);
            Assert.That(_bookStub.Books.Any(book => book.Id == expectedId), Is.False);
        });
    }
    
    [TearDown]
    public void TearDown()
    {
        _bookStub = null!;
    }
    
    private struct AuthorComparer : IEqualityComparer<AuthorDTO>
    {
        public bool Equals(AuthorDTO x, AuthorDTO y)
        {
            return x.Id == y.Id && x.Name == y.Name && x.Description == y.Description && x.BirthDate.Equals(y.BirthDate) && Nullable.Equals(x.DeathDate, y.DeathDate);
        }

        public int GetHashCode(AuthorDTO obj)
        {
            return HashCode.Combine(obj.Id, obj.Name, obj.Description, obj.BirthDate, obj.DeathDate);
        }
    }
    
    private struct GenreComparer : IEqualityComparer<GenreDTO>
    {
        public bool Equals(GenreDTO x, GenreDTO y)
        {
            return x.Id == y.Id && x.Name == y.Name;
        }

        public int GetHashCode(GenreDTO obj)
        {
            return HashCode.Combine(obj.Id, obj.Name);
        }
    }
    
    private struct ThemeComparer : IEqualityComparer<ThemeDTO>
    {
        public bool Equals(ThemeDTO x, ThemeDTO y)
        {
            return x.Id == y.Id && x.Description == y.Description;
        }

        public int GetHashCode(ThemeDTO obj)
        {
            return HashCode.Combine(obj.Id, obj.Description);
        }
    }
    
    private struct SettingComparer : IEqualityComparer<SettingDTO>
    {
        public bool Equals(SettingDTO x, SettingDTO y)
        {
            return x.Id == y.Id && x.Description == y.Description;
        }

        public int GetHashCode(SettingDTO obj)
        {
            return HashCode.Combine(obj.Id, obj.Description);
        }
    }
}