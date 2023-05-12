using Interface.DTO;

namespace Business.Entity;

public record Publisher(long? Id, string Name, DateTime FoundingDate, string Description)
{
    public Publisher(PublisherDTO publisherDto) : this(publisherDto.Id, publisherDto.Name, publisherDto.FoundingDate, publisherDto.Description) { }
    public PublisherDTO GetDto() => new PublisherDTO(Id, Name, FoundingDate, Description);
}