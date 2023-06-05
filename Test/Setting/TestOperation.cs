using Business.Container;
using Test.STUB;

namespace Test.Setting;
using Business.Entity;

[TestFixture]
public class TestOperation
{
    private SettingSTUB _settingStub = null!;
    private SettingContainer _settingContainer;

    [SetUp]
    public void Setup()
    {
        _settingStub = new();
        _settingContainer = new(_settingStub);
    }
    
    [Test, Combinatorial]
    [Category("Create")]
    public void TestAdd(
        [Values("Mars", "90s", "Swamp")] string name
    )
    {
        // Arrange
        Setting expectedSetting = new(
            (byte?)(_settingStub.Settings.Count + 1),
            name
        );

        // Act
        bool actual = _settingContainer.Add(expectedSetting);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_settingStub.Settings, Is.Not.Null);
            Assert.That(_settingStub.Settings, Is.Not.Empty);
            Assert.That(actual);
            Assert.That(_settingStub.Settings.Exists(setting =>
                setting.Id == expectedSetting.Id &&
                setting.Description == expectedSetting.Description
                )
            );
        });
    }
}