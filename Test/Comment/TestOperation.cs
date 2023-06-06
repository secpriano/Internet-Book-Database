using Test.STUB;

namespace Test.Comment;
using Business.Entity;

[TestFixture]
public class TestOperation
{
    private CommentSTUB _commentStub = null!;
    private Review _commentContainer;

    [SetUp]
    public void Setup()
    {
        _commentStub = new();
        _commentContainer = new(_commentStub);
    }
    
    [Test]
    [Category("Create")]
    public void AddComment()
    {
        // Arrange
        Comment expectedComment = new(
            _commentStub.Comments.Count + 1,
            1,
            "This is a comment",
            1
        );
        
        // Act
        bool actual = _commentContainer.AddComment(expectedComment);
        
        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_commentStub.Comments, Is.Not.Null);
            Assert.That(_commentStub.Comments, Is.Not.Empty);
            Assert.That(actual);
            Assert.That(_commentStub.Comments.Exists(comment =>
                comment.Id == expectedComment.Id &&
                comment.ReviewId == expectedComment.ReviewId &&
                comment.Content == expectedComment.Content &&
                comment.UserId == expectedComment.UserId
                )
            );
        });
    }
}