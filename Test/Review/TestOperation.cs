using Business.Container;
using Test.STUB;

namespace Test.Review;
using Business.Entity;

[TestFixture]
public class TestOperation
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
    [Category("Create")]
    public void TestAdd()
    {
        // Arrange
        Review expectedReview = new(
            (byte?)(_reviewStub.Reviews.Count + 1),
            "This is a review title",
            "This is a review",
            1,
            5,
            null
        );

        // Act
        bool actual = _reviewContainer.Add(expectedReview);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(_reviewStub.Reviews, Is.Not.Null);
            Assert.That(_reviewStub.Reviews, Is.Not.Empty);
            Assert.That(actual);
            Assert.That(_reviewStub.Reviews.Exists(review =>
                review.Id == expectedReview.Id &&
                review.Title == expectedReview.Title &&
                review.Content == expectedReview.Content &&
                review.BookId == expectedReview.BookId &&
                review.Comments == expectedReview.Comments
                )
            );
        });
    }
}