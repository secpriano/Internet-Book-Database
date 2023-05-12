namespace Interface.DTO;

public record struct AuthorDTO(long? Id, string Name, string Description, DateTime BirthDate, DateTime? DeathDate);