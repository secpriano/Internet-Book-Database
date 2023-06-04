using Business.Container;
using Interface.DTO;
using Test.STUB;

namespace Test.Author;
using Business.Entity;

[TestFixture]
public class TestOperation
{
    private AuthorSTUB _authorStub = null!;
    private AuthorContainer _authorContainer;

    [SetUp]
    public void Setup()
    {
        _authorStub = new();
        _authorContainer = new(_authorStub);
    }
    
    [Test, Combinatorial]
    [Category("Create")]
    public void TestAdd(
        [Values("J. R. R. Tolkien", "J. K. Rowling", "George Lucas")] string name,
        [Values("J. R. R. Tolkien beautiful description!", "J. K. Rowling? beautiful description.", "George Lucas beautiful description")] string description
    )
    {
        // Arrange
        GenreSTUB genreStub = new();
        
        Author expectedAuthor = new(
            _authorStub.Authors.Count + 1,
            name,
            description,
            new(1892, 1, 3),
            new(1973,9, 2),
            genreStub.Genres.FindAll(genre => genre is { Id: 1 } or { Id: 2 })
                .Select(genre => new Genre(genre.Id, genre.Name))
        );

        // Act
        bool actual = _authorContainer.Add(expectedAuthor); 
        
        // Assert
        AuthorDTO expectedAuthorDto = expectedAuthor.ToDto();

        Assert.Multiple (() =>
        {
            Assert.That(_authorStub.Authors, Is.Not.Null);
            Assert.That(_authorStub.Authors, Is.Not.Empty);
            Assert.That(actual); 
            Assert.That(_authorStub.Authors.Exists(author =>
                author.Id == expectedAuthorDto.Id &&
                author.Name == expectedAuthorDto.Name &&
                author.Description == expectedAuthorDto.Description &&
                author.BirthDate == expectedAuthorDto.BirthDate &&
                author.DeathDate == expectedAuthorDto.DeathDate &&
                author.Genres.SequenceEqual(expectedAuthorDto.Genres, new GenreComparer())
                )
            );
        });
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
}