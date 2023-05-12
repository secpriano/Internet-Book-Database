namespace IBDbWebApplication.Models.Entity;

public class GenreModel
{
    public byte? Id { get; set; }
    public string Name { get; set; }

    public GenreModel(byte? id, string name) =>
        (Id, Name) = (id, name);
    
    public GenreModel()
    {
    }
}