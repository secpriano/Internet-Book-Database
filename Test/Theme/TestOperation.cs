using Business.Container;
using Test.STUB;

namespace Test.Theme;
using Business.Entity;

[TestFixture]
public class TestOperation
{
    private ThemeSTUB _themeStub = null!;
    private ThemeContainer _themeContainer;

    [SetUp]
    public void Setup()
    {
        _themeStub = new();
        _themeContainer = new(_themeStub);
    }
    
    [Test, Combinatorial]
    [Category("Create")]
    public void TestAdd(
        [Values("LGBTQIA+", "Redemption", "Love")] string name
    )
    {
        // Arrange
        Theme expectedTheme = new(
            (byte?)(_themeStub.Themes.Count + 1),
            name
        );

        // Act
        bool actual = _themeContainer.Add(expectedTheme);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_themeStub.Themes, Is.Not.Null);
            Assert.That(_themeStub.Themes, Is.Not.Empty);
            Assert.That(actual);
            Assert.That(_themeStub.Themes.Exists(theme =>
                theme.Id == expectedTheme.Id &&
                theme.Description == expectedTheme.Description
                )
            );
        });
    }
}