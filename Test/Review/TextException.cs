using Business.Container;
using NUnit.Framework;
using Test.STUB;

namespace Test.Review;
using Business.Entity;

[TestFixture]
public class TextException
{
    private ReviewSTUB _reviewStub = null!;
    private ReviewContainer _reviewContainer;

    [SetUp]
    public void Setup()
    {
        _reviewStub = new();
        _reviewContainer = new(_reviewStub);
    }
    
    [Test]
    [Category("Title")]
    public void Test_Exception_AddReview_When_Title_IsLonger_Than100Characters()
    {
        // Arrange
        Review expectedReview = new(
            _reviewStub.Reviews.Count + 1,
            new('a', 101),
            "This is the best review in the world",
            5,
            1,
            null
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _reviewContainer.Add(expectedReview));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Title must be less or equal to 100 Character. Not {expectedReview.Title.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Title")]
    public void Test_Exception_AddReview_When_Title_IsEmpty()
    {
        // Arrange
        Review expectedReview = new(
            _reviewStub.Reviews.Count + 1,
            String.Empty,
            "This is the best review in the world",
            5,
            1,
            null
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _reviewContainer.Add(expectedReview));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Title must be more than or equal to 1 Character. Not {expectedReview.Title.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Title")]
    public void Test_Exception_AddReview_When_Title_IsLess_Than1Characters()
    {
        // Arrange
        Review expectedReview = new(
            _reviewStub.Reviews.Count + 1,
            "",
            "This is the best review in the world",
            5,
            1,
            null
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _reviewContainer.Add(expectedReview));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Title must be more than or equal to 1 Character. Not {expectedReview.Title.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Title")]
    public void Test_Exception_AddReview_When_Title_Contains_InvalidCharacters()
    {
        // Arrange
        Review expectedReview = new(
            _reviewStub.Reviews.Count + 1,
            "This is the best review in the world!!",
            "This is the best review in the world",
            5,
            1,
            null
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _reviewContainer.Add(expectedReview));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual("Title must only contain letters, numbers, spaces, and ampersand.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Content")]
    public void Test_Exception_AddReview_When_Content_IsLEss_Than2Characters()
    {
        // Arrange
        Review expectedReview = new(
            _reviewStub.Reviews.Count + 1,
            "This is the best review in the world",
            new('a', 1),
            5,
            1,
            null
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _reviewContainer.Add(expectedReview));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Content must be more than or equal to 2 Character. Not {expectedReview.Content.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Content")]
    public void Test_Exception_AddReview_When_Content_IsLonger_Than2000Characters()
    {
        // Arrange
        Review expectedReview = new(
            _reviewStub.Reviews.Count + 1,
            "This is the best review in the world",
            new('a', 2001),
            5,
            1,
            null
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _reviewContainer.Add(expectedReview));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Content must be less or equal to 2000 Character. Not {expectedReview.Content.Length} Character.", innerException.Message);
        }
    }
    
    [Test]
    [Category("Content")]
    public void Test_Exception_AddReview_When_Content_IsEmpty()
    {
        // Arrange
        Review expectedReview = new(
            _reviewStub.Reviews.Count + 1,
            "This is the best review in the world",
            String.Empty,
            5,
            1,
            null
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _reviewContainer.Add(expectedReview));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<Exception>());
            Assert.AreEqual($"Content must be more than or equal to 2 Character. Not {expectedReview.Content.Length} Character.", innerException.Message);
        }
    }
}