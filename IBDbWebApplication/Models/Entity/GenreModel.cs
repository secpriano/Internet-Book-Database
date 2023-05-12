namespace IBDbWebApplication.Models.Entity;

public class GenreModel
{
    public long? Id { get; set; }
    public string Name { get; set; }

    public GenreModel(long? id, string name) =>
        (Id, Name) = (id, name);
    
    public GenreModel()
    {
    }
}