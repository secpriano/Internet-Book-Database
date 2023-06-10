using Test.STUB;

namespace Test.Comment;
using Business.Entity;

[TestFixture]
public class TestOperation
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
    [Category("Create")]
    public void AddComment()
    {
        // Arrange
        Comment expectedComment = new(
            _commentStub.Comments.Count + 1,
            "This is a comment",
            new(_AccountSTUB.Accounts[0]),
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
                comment.Account.Id == expectedComment.Account.Id &&
                comment.Account.Username == expectedComment.Account.Username &&
                comment.Account.Email == expectedComment.Account.Email &&
                comment.Account.IsAdmin == expectedComment.Account.IsAdmin
                )
            );
        });
    }
}