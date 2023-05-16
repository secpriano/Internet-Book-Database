namespace IBDbWebApplication.Models.Entity;

public class PublisherModel
{
    public long? Id { get; set; }
    public string Name { get; set; }
    public DateOnly FoundingDate { get; set; }
    public string Description { get; set; }
    
    public PublisherModel(long? id, string name, DateOnly foundingDate, string description) => 
        (Id, Name, FoundingDate, Description) = (id, name, foundingDate, description);
    
    public PublisherModel() { }
}