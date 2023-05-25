namespace Interface.DTO;

public record struct AuthorDTO(long? Id, string Name, string Description, DateOnly BirthDate, DateOnly? DeathDate, List<GenreDTO> Genres);