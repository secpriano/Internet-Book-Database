using Business.Container;
using Test.STUB;

namespace Test.Genre;
using Business.Entity;

[TestFixture]
public class TestOperation
{
    private GenreSTUB _genreStub = null!;
    private GenreContainer _genreContainer;

    [SetUp]
    public void Setup()
    {
        _genreStub = new();
        _genreContainer = new(_genreStub);
    }
    
    [Test, Combinatorial]
    [Category("Create")]
    public void TestAdd(
        [Values("Space Opera", "Self Help", "Daylight Horror")] string name
    )
    {
        // Arrange
        Genre expectedGenre = new(
            (byte?)(_genreStub.Genres.Count + 1),
            name
        );

        // Act
        bool actual = _genreContainer.Add(expectedGenre);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_genreStub.Genres, Is.Not.Null);
            Assert.That(_genreStub.Genres, Is.Not.Empty);
            Assert.That(actual);
            Assert.That(_genreStub.Genres.Exists(genre =>
                genre.Id == expectedGenre.Id &&
                genre.Name == expectedGenre.Name
                )
            );
        });
    }
}