using Business.Container;
using Interface.DTO;
using Test.STUB;

namespace Test.Comment;
using Business.Entity;

[TestFixture]
public class TestException
{
    static AccountSTUB _AccountSTUB = new();
    
    private CommentSTUB _commentStub = null!;
    private Review _commentContainer;

    [SetUp]
    public void Setup()
    {
        _commentStub = new();
        _commentContainer = new(_commentStub);
    }
    
    [Test]
    [Category("Content")]
    public void Test_Exception_AddComment_When_Content_IsLonger_Than4000Characters()
    {
        // Arrange
        Comment expectedComment = new(
            _commentStub.Comments.Count + 1,
            new('a', 4001),
            new(_AccountSTUB.Accounts[0]),
            1
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _commentContainer.AddComment(expectedComment));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Content must be less or equal to 4000 Character. Not {expectedComment.Content.Length} Character."));
        }
    }
    
    [Test]
    [Category("Content")]
    public void Test_Exception_AddComment_When_Content_IsEmpty()
    {
        // Arrange
        Comment expectedComment = new(
            _commentStub.Comments.Count + 1,
            String.Empty,
            new(_AccountSTUB.Accounts[0]),
            1
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _commentContainer.AddComment(expectedComment));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Content must be more than or equal to 2 Character. Not {expectedComment.Content.Length} Character."));
        }
    }
    
    [Test]
    [Category("Content")]
    public void Test_Exception_AddComment_When_Content_IsLessThan2Characters()
    {
        // Arrange
        Comment expectedComment = new(
            _commentStub.Comments.Count + 1,
            "a",
            new(_AccountSTUB.Accounts[0]),
            1
        );
        
        // Act
        AggregateException aggregateException = Assert.Throws<AggregateException>(() => _commentContainer.AddComment(expectedComment));
        
        // Assert
        foreach (Exception innerException in aggregateException.InnerExceptions)
        {
            Assert.That(innerException, Is.TypeOf<KeyValueException>()); 
            Assert.That(innerException.Message, Is.EqualTo($"Content must be more than or equal to 2 Character. Not {expectedComment.Content.Length} Character."));
        }
    }
}