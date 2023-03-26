using Interface.DTO;

namespace Business.Entity;

public class Author
{
    public long? Id { get; set; } 
    public string Name { get; set; } 
    public string Description { get; set; } 
    public DateTime BirthDate { get; set; } 
    public DateTime? DeathDate { get; set; } 
    public IEnumerable<Genre> Genres { get; set; }

    public Author(long? id, string name, string description, DateTime birthDate, DateTime? deathDate, IEnumerable<Genre> genres) => 
        (Id, Name, Description, BirthDate, DeathDate, Genres) = (id, name, description, birthDate, deathDate, genres);
    public Author(AuthorDTO authorDto) : this(
        authorDto.Id, authorDto.Name, authorDto.Description, authorDto.BirthDate,
        authorDto.DeathDate, authorDto.Genres.Select(genre => new Genre(genre))
        ) { }
    public AuthorDTO GetDto() => new(Id, Name, Description, BirthDate, DeathDate, Genres.Select(genre => genre.GetDto()));

    public bool ThisEquals(Author otherAuthor)
    {
        for (int i = 0; i < Genres.Count(); i++)
        {
            if (!Genres.ElementAt(i).ThisEquals(otherAuthor.Genres.ElementAt(i)))
            {
                return false;
            }
        }

        return Id == otherAuthor.Id &&
               Name == otherAuthor.Name &&
               Description == otherAuthor.Description &&
               BirthDate == otherAuthor.BirthDate &&
               DeathDate == otherAuthor.DeathDate;
    }
}
