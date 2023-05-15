using Interface.DTO;

namespace Business.Entity;

public class Author : IEqualityComparer<Author>
{
    public long? Id { get; set; } 
    public string Name { get; set; } 
    public string Description { get; set; } 
    public DateTime BirthDate { get; set; } 
    public DateTime? DeathDate { get; set; } 

    public Author(long? id, string name, string description, DateTime birthDate, DateTime? deathDate) => 
        (Id, Name, Description, BirthDate, DeathDate) = (id, name, description, birthDate, deathDate);

    public Author(AuthorDTO authorDto) : this(
        authorDto.Id, authorDto.Name, authorDto.Description, authorDto.BirthDate, authorDto.DeathDate
        ) {}

    public Author(long? id)
    {
        Id = id;
    }
    
    public AuthorDTO ToDto() => new(Id, Name, Description, BirthDate, DeathDate);

    public bool Equals(Author x, Author y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id && x.Name == y.Name && x.Description == y.Description && x.BirthDate.Equals(y.BirthDate) && Nullable.Equals(x.DeathDate, y.DeathDate);
    }

    public int GetHashCode(Author obj)
    {
        return HashCode.Combine(obj.Id, obj.Name, obj.Description, obj.BirthDate, obj.DeathDate);
    }
}
