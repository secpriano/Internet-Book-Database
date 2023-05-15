using Interface.DTO;

namespace Business.Entity;

public class Publisher
{
    public long? Id { get; set; }
    public string Name { get; set; }
    public DateTime FoundingDate { get; set; }
    public string Description { get; set; }

    public Publisher(long? id, string name, DateTime foundingDate, string description) => 
        (Id, Name, FoundingDate, Description) = (id, name, foundingDate, description);

    public Publisher(PublisherDTO publisherDto) : this(publisherDto.Id, publisherDto.Name, publisherDto.FoundingDate, publisherDto.Description) { }
    
    public Publisher(long? id)
    {
        Id = id;
    }
    
    public PublisherDTO GetDto() => new PublisherDTO(Id, Name, FoundingDate, Description);
}