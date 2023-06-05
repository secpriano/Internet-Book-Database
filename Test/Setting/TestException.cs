using Business.Container;
using Test.STUB;

namespace Test.Setting;
using Business.Entity;

[TestFixture]
public class TestException
{
    private SettingSTUB _settingStub = null!;
    private SettingContainer _settingContainer;

    [SetUp]
    public void Setup()
    {
        _settingStub = new();
        _settingContainer = new(_settingStub);
    }
    
    [Test]
    [Category("Description")]
    public void Test_Exception_AddSetting_When_Description_IsLonger_Than25Characters()
    {
        // Arrange
        Setting expectedSetting = new(
            (byte?)(_settingStub.Settings.Count + 1),
            "Best setting in the world!!!"
        );

        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _settingContainer.Add(expectedSetting));    
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Description must be less or equal to 25 Character. Not {expectedSetting.Description.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Description")]
    public void Test_Exception_AddSetting_When_Description_IsLess_Than2Characters()
    {
        // Arrange
        Setting expectedSetting = new(
            (byte?)(_settingStub.Settings.Count + 1),
            "B"
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _settingContainer.Add(expectedSetting));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Description must be more than or equal to 2 Character. Not {expectedSetting.Description.Length} Character.", innerException.Message);
        }
    }
    
    // invalid characters
    [Test]
    [Category("Description")]
    public void Test_Exception_AddSetting_When_Description_Contains_InvalidCharacters()
    {
        // Arrange
        Setting expectedSetting = new(
            (byte?)(_settingStub.Settings.Count + 1),
            "Metro@$*#^%"
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _settingContainer.Add(expectedSetting));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Description can only contain letters, numbers, spaces, commas, and ampersands.", innerException.Message);
        }
    }
}