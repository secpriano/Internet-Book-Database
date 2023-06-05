using Business.Container;
using Test.STUB;

namespace Test.Theme;
using Business.Entity;

[TestFixture]
public class TestException
{
    private ThemeSTUB _themeStub = null!;
    private ThemeContainer _themeContainer;

    [SetUp]
    public void Setup()
    {
        _themeStub = new();
        _themeContainer = new(_themeStub);
    }
    
    [Test]
    [Category("Description")]
    public void Test_Exception_AddTheme_When_Description_IsLonger_Than25Characters()
    {
        // Arrange
        Theme expectedTheme = new(
            (byte?)(_themeStub.Themes.Count + 1),
            "Best theme in the world!!!"
        );

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _themeContainer.Add(expectedTheme));    
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Description must be less or equal to 25 Character. Not {expectedTheme.Description.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Description")]
    public void Test_Exception_AddTheme_When_Description_IsLess_Than2Characters()
    {
        // Arrange
        Theme expectedTheme = new(
            (byte?)(_themeStub.Themes.Count + 1),
            "B"
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _themeContainer.Add(expectedTheme));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Description must be more than or equal to 2 Character. Not {expectedTheme.Description.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Description")]
    public void Test_Exception_AddTheme_When_Description_Contains_InvalidCharacters()
    {
        // Arrange
        Theme expectedTheme = new(
            (byte?)(_themeStub.Themes.Count + 1),
            "Lov#$%#"
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _themeContainer.Add(expectedTheme));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Description can only contain letters, spaces, commas, and ampersands.", innerException.Message);
        }
    }
}