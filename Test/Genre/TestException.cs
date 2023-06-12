using Business.Container;
using Test.STUB;

namespace Test.Genre;
using Business.Entity;

[TestFixture]
public class TestException
{
    private GenreSTUB _genreStub = null!;
    private GenreContainer _genreContainer;

    [SetUp]
    public void Setup()
    {
        _genreStub = new();
        _genreContainer = new(_genreStub);
    }
    
    [Test]
    [Category("Description")]
    public void Test_Exception_AddGenre_When_Title_IsLonger_Than25Characters()
    {
        // Arrange
        Genre expectedGenre = new(
            (byte?)(_genreStub.Genres.Count + 1),
            "Best genre in the world!!!"
        );

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _genreContainer.Add(expectedGenre));    
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.AreEqual($"Genre must be less or equal to 25 Character. Not {expectedGenre.Name.Length} Character.", innerException.Message);
        }
    }

    [Test]
    [Category("Description")]
    public void Test_Exception_AddGenre_When_Title_IsLess_Than2Characters()
    {
        // Arrange
        Genre expectedGenre = new(
            (byte?)(_genreStub.Genres.Count + 1),
            "B"
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _genreContainer.Add(expectedGenre));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.AreEqual($"Genre must be more than or equal to 2 Character. Not {expectedGenre.Name.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Description")]
    public void Test_Exception_AddGenre_When_Title_Contains_InvalidCharacters()
    {
        // Arrange
        Genre expectedGenre = new(
            (byte?)(_genreStub.Genres.Count + 1),
            "Best#$@%&("
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _genreContainer.Add(expectedGenre));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>());
            Assert.AreEqual("Name can only contain letters, spaces, commas, and ampersands.", innerException.Message);
        }
    }
}