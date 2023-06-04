using Business.Container;
using Interface.DTO;
using Test.STUB;

namespace Test.Publisher;

[TestFixture]
public class TestException
{
    private PublisherSTUB _publisherStub = null!;
    private PublisherContainer _publisherContainer;

    [SetUp]
    public void Setup()
    {
        _publisherStub = new();
        _publisherContainer = new(_publisherStub);
    }
    
    [Test]
    public void Test_Exception_AddPublisher_When_Name_IsLonger_Than50Characters()
    {
        // Arrange
        PublisherDTO publisher = _publisherStub.Publishers[0];
        
        publisher.Name = new('a', 51);
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _publisherContainer.Add(new (publisher)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Name must be less or equal to 50 Character. Not {publisher.Name.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddPublisher_When_Name_IsEmpty()
    {
        // Arrange
        PublisherDTO publisher = _publisherStub.Publishers[0];
        
        publisher.Name = String.Empty;
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _publisherContainer.Add(new (publisher)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Name must be more than or equal to 1 Character. Not {publisher.Name.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddPublisher_When_Name_Contains_InvalidCharacters()
    {
        // Arrange
        PublisherDTO publisher = _publisherStub.Publishers[0];
        
        publisher.Name = "a1";
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _publisherContainer.Add(new (publisher)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo("Name must only contain letters, spaces, and ampersand."));
        }
    }
    
    [Test]
    public void Test_Exception_AddPublisher_When_Description_IsShorter_Than10Characters()
    {
        // Arrange
        PublisherDTO publisher = _publisherStub.Publishers[0];
        
        publisher.Description = new('a', 9);
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _publisherContainer.Add(new (publisher)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Description must be more than or equal to 10 Character. Not {publisher.Description.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddPublisher_When_Description_IsLonger_Than1000Characters()
    {
        // Arrange
        PublisherDTO publisher = _publisherStub.Publishers[0];
        
        publisher.Description = new('a', 1001);
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _publisherContainer.Add(new (publisher)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Description must be less or equal to 1000 Character. Not {publisher.Description.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddPublisher_When_Description_IsEmpty()
    {
        // Arrange
        PublisherDTO publisher = _publisherStub.Publishers[0];
        
        publisher.Description = String.Empty;
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _publisherContainer.Add(new (publisher)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Description must be more than or equal to 10 Character. Not {publisher.Description.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddPublisher_When_FoundingDate_IsNotWithin_DateOnlyMinValue_To_DateOnlyMaxValue(
        [Values(0, 10000)] int year
        )
    {
        // Arrange
        PublisherDTO publisher = _publisherStub.Publishers[0];

        // Act
        ArgumentOutOfRangeException argumentOutOfRangeException = Assert.Throws<ArgumentOutOfRangeException>(() => publisher.FoundingDate = new(year, 1, 1));
        
        // Assert
        Assert.That(argumentOutOfRangeException, Is.TypeOf<ArgumentOutOfRangeException>()); 
        Assert.That(argumentOutOfRangeException.Message, Is.EqualTo("Year, Month, and Day parameters describe an un-representable DateTime."));
    }
}