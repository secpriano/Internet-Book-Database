using Business.Container;
using Interface.DTO;
using Test.STUB;

namespace Test.Author;

[TestFixture]
public class TestException
{
    private AuthorSTUB _authorStub = null!;
    private AuthorContainer _authorContainer;

    [SetUp]
    public void Setup()
    {
        _authorStub = new();
        _authorContainer = new(_authorStub);
    }
    
    [Test]
    public void Test_Exception_AddAuthor_When_Name_IsLonger_Than1000Characters()
    {
        // Arrange
        AuthorDTO author = _authorStub.Authors[0];
        
        author.Name = new('a', 1001);
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _authorContainer.Add(new (author)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Name must be less or equal to 1000 Character. Not {author.Name.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddAuthor_When_Name_IsEmpty()
    {
        // Arrange
        AuthorDTO author = _authorStub.Authors[0];
        
        author.Name = String.Empty;
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _authorContainer.Add(new (author)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Name must be more than or equal to 1 Character. Not {author.Name.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddAuthor_When_Name_Contains_InvalidCharacters()
    {
        // Arrange
        AuthorDTO author = _authorStub.Authors[0];
        
        author.Name = "a1";
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _authorContainer.Add(new (author)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo("Name can only contain letters, spaces, and periods."));
        }
    }
    
    [Test]
    public void Test_Exception_AddAuthor_When_Description_IsLonger_Than1000Characters()
    {
        // Arrange
        AuthorDTO author = _authorStub.Authors[0];
        
        author.Description = new('a', 1001);
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _authorContainer.Add(new (author)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Description must be less or equal to 1000 Character. Not {author.Description.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddAuthor_When_Description_IsEmpty()
    {
        // Arrange
        AuthorDTO author = _authorStub.Authors[0];
        
        author.Description = String.Empty;
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _authorContainer.Add(new (author)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Description must be more than or equal to 10 Character. Not {author.Description.Length} Character."));
        }
    }
    
    [Test]
    public void Test_Exception_AddAuthor_When_BirthDate_IsBefore_150Years()
    {
        // Arrange
        AuthorDTO author = _authorStub.Authors[0];
        
        author.BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-151));
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _authorContainer.Add(new (author)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Birth date must be more than or equal to {DateTime.Now.AddYears(-150).Year} Year. Not {author.BirthDate.Year} Year."));
        }
    }
    
    [Test]
    public void Test_Exception_AddAuthor_When_BirthDate_IsAfter_6Years()
    {
        // Arrange
        AuthorDTO author = _authorStub.Authors[0];
        
        author.BirthDate = DateOnly.FromDateTime(DateTime.Now.AddYears(-5));
        author.DeathDate = null;
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _authorContainer.Add(new (author)));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Birth date must be less or equal to {DateTime.Now.AddYears(-6).Year} Year. Not {author.BirthDate.Year} Year."));
        }
    }
}