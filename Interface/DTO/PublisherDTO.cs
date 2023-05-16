namespace Interface.DTO;

public record struct PublisherDTO(long? Id, string Name, DateOnly FoundingDate, string Description);