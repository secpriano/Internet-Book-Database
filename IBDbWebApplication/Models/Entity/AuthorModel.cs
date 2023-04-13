namespace IBDbWebApplication.Models;

public class AuthorModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } 
    public DateTime BirthDate { get; set; } 
    public DateTime? DeathDate { get; set; } 
    public IEnumerable<GenreModel> Genres { get; set; }

    public AuthorModel(long id, string name, string description, DateTime birthDate, DateTime? deathDate, IEnumerable<GenreModel> genres) => 
        (Id, Name, Description, BirthDate, DeathDate, Genres) = (id, name, description, birthDate, deathDate, genres);

    public AuthorModel() { }
}