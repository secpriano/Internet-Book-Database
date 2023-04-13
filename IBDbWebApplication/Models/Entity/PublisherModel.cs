namespace IBDbWebApplication.Models;

public class PublisherModel
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    
    public PublisherModel(long id, string name, string description) => 
        (Id, Name, Description) = (id, name, description);
    
    public PublisherModel() { }
}