using Interface.DTO;

namespace Business.Entity;

public class Genre
{
    public byte? Id { get; set; } 
    public string Name { get; set; }

    public Genre(byte? id, string name) => 
        (Id, Name) = (id, name);

    public Genre(GenreDTO genreDto) : this(genreDto.Id, genreDto.Name) { }
    
    public Genre(byte? id)
    {
        Id = id;
    }
    
    public GenreDTO ToDto() => new(Id, Name);
}
