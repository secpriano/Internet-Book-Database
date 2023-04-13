using Interface.DTO;

namespace Business.Entity;

public record Genre(long? Id, string Name)
{
    public Genre(GenreDTO genreDto) : this(genreDto.Id, genreDto.Name) { }
    public GenreDTO GetDto() => new(Id, Name);
    
    public bool ThisEquals(Genre otherGenre)
    {
        return Id == otherGenre.Id && Name == otherGenre.Name;
    }
}
