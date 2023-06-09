﻿using Interface.DTO;

namespace Business.Entity;

public class Author : IEqualityComparer<Author>, IEquatable<Author>
{
    public long? Id { get; } 
    public string Name { get; } 
    public string Description { get; } 
    public DateOnly BirthDate { get; set; } 
    public DateOnly? DeathDate { get; }
    public IEnumerable<Genre> Genres { get; }

    public Author(long? id, string name, string description, DateOnly birthDate, DateOnly? deathDate, IEnumerable<Genre> genres) => 
        (Id, Name, Description, BirthDate, DeathDate, Genres) = (id, name, description, birthDate, deathDate, genres);

    public Author(AuthorDTO authorDto) : this(
        authorDto.Id, authorDto.Name, authorDto.Description, authorDto.BirthDate, authorDto.DeathDate, authorDto.Genres.Select(genre => new Genre(genre)).ToList()
        ) {}

    public Author(long? id)
    {
        Id = id;
    }
    
    public AuthorDTO ToDto() => new(Id, Name, Description, BirthDate, DeathDate, Genres?.Select(genre => genre.ToDto()).ToList());

    public bool Equals(Author x, Author y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (ReferenceEquals(x, null)) return false;
        if (ReferenceEquals(y, null)) return false;
        if (x.GetType() != y.GetType()) return false;
        return x.Id == y.Id && x.Name == y.Name && x.Description == y.Description && x.BirthDate.Equals(y.BirthDate) && Nullable.Equals(x.DeathDate, y.DeathDate) && x.Genres.Equals(y.Genres);
    }

    public int GetHashCode(Author obj)
    {
        return HashCode.Combine(obj.Id, obj.Name, obj.Description, obj.BirthDate, obj.DeathDate, obj.Genres);
    }

    public bool Equals(Author? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;

        for (int i = 0; i < Genres.Count(); i++)
        {
            return Genres.ToArray()[i].Equals(other.Genres.ToArray()[i]);
        }
        return Id == other.Id && Name == other.Name && Description == other.Description && BirthDate.Equals(other.BirthDate) && Nullable.Equals(DeathDate, other.DeathDate);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Author) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Name, Description, BirthDate, DeathDate, Genres);
    }
}
