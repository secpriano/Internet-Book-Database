using Business.Container;
using Interface.DTO;
using Test.STUB;

namespace Test.Publisher;
using Business.Entity;

[TestFixture]
public class TestOperation
{
    private PublisherSTUB _publisherStub = null!;
    private PublisherContainer _publisherContainer;

    [SetUp]
    public void Setup()
    {
        _publisherStub = new();
        _publisherContainer = new(_publisherStub);
    }

    //add publisher
    [Test, Combinatorial]
    [Category("Create")]
    public void TestAdd(
        [Values("HarperCollins", "Faber & Faber", "Penguin Random House")] string name,
        [Values("2nd of Big Three", "Indie publisher", "1st of Big Three")] string description
    )
    {
        // Arrange
        Publisher expectedPublisher = new(
            _publisherStub.Publishers.Count + 1,
            name,
            new(1550, 1, 1),
            description
        );

        // Act
        bool actual = _publisherContainer.Add(expectedPublisher);

        // Assert
        PublisherDTO expectedPublisherDto = expectedPublisher.ToDto();

        Assert.Multiple(() =>
        {
            Assert.That(_publisherStub.Publishers, Is.Not.Null);
            Assert.That(_publisherStub.Publishers, Is.Not.Empty);
            Assert.That(actual);
            Assert.That(_publisherStub.Publishers.Exists(publisher =>
                publisher.Id == expectedPublisherDto.Id &&
                publisher.Name == expectedPublisherDto.Name &&
                publisher.Description == expectedPublisherDto.Description
                )
            );
        });
    }
}