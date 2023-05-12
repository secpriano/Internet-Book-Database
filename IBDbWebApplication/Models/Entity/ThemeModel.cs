namespace IBDbWebApplication.Models.Entity;

public class ThemeModel
{
    public byte? Id { get; set; }
    public string Description { get; set; }
    
    public ThemeModel(byte? id, string description) => 
        (Id, Description) = (id, description);
}