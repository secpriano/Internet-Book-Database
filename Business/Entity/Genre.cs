using Interface.DTO;

namespace Business.Entity;

public class Genre : IEquatable<Genre>
{
    public byte? Id { get; } 
    public string Name { get; }

    public Genre(byte? id, string name) => 
        (Id, Name) = (id, name);

    public Genre(GenreDTO genreDto) : this(genreDto.Id, genreDto.Name) { }
    
    public Genre(byte? id)
    {
        Id = id;
    }
    
    public GenreDTO ToDto() => new(Id, Name);

    public bool Equals(Genre? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Genre) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name);
    }
}
