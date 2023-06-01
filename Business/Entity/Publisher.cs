using Interface.DTO;

namespace Business.Entity;

public class Publisher
{
    public long? Id { get; }
    public string Name { get; }
    public DateOnly FoundingDate { get; }
    public string Description { get; }

    public Publisher(long? id, string name, DateOnly foundingDate, string description) => 
        (Id, Name, FoundingDate, Description) = (id, name, foundingDate, description);

    public Publisher(PublisherDTO publisherDto) : this(publisherDto.Id, publisherDto.Name, publisherDto.FoundingDate, publisherDto.Description) { }
    
    public Publisher(long? id)
    {
        Id = id;
    }
    
    public PublisherDTO ToDto() => new(Id, Name, FoundingDate, Description);
}