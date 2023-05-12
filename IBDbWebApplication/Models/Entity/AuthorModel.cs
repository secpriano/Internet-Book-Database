namespace IBDbWebApplication.Models.Entity;

public class AuthorModel
{
    public long? Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; } 
    public DateTime BirthDate { get; set; } 
    public DateTime? DeathDate { get; set; }
    
    public AuthorModel(long? id, string name, string description, DateTime birthDate, DateTime? deathDate) => 
        (Id, Name, Description, BirthDate, DeathDate) = (id, name, description, birthDate, deathDate);

    public AuthorModel() { }
}