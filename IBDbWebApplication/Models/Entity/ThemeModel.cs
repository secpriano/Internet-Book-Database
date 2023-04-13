namespace IBDbWebApplication.Models;

public class ThemeModel
{
    public long Id { get; set; }
    public string Description { get; set; }
    
    public ThemeModel(long id, string description) => 
        (Id, Description) = (id, description);
}