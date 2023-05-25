using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class PublisherContainer
{
    private readonly IPublisherData _publisherData;
    
    public PublisherContainer(IPublisherData publisherData)
    {
        _publisherData = publisherData;
    }   
    
    public IEnumerable<Publisher> GetAll()
    {
        return _publisherData.GetAll().Select(publisher => new Publisher(publisher.Id, publisher.Name, publisher.FoundingDate, publisher.Description));
    }
    
    public bool Add(Publisher publisher)
    {
        ValidatePublisher(publisher);
        
        return _publisherData.Add(publisher.GetDto());
    }
    
    private void ValidatePublisher(Publisher publisher)
    {
        ValidateName(publisher.Name);
        ValidateDescription(publisher.Description);
        ValidateFoundingDate(publisher.FoundingDate);
        
        if (Validate.Exceptions.InnerExceptions.Count > 0)
        {
            throw Validate.Exceptions;
        }
    }
    
    private void ValidateName(string name)
    {
        Validate.OutOfRange((ulong)name.Length, 1, 50, "Name", Validate.Unit.Character);
        Validate.Regex(name, "^[a-zA-Z &]+$", "Name must only contain letters, spaces, and ampersand.");
    }
    
    private void ValidateDescription(string description)
    {
        Validate.OutOfRange((ulong)description.Length, 10, 1000, "Description", Validate.Unit.Character);
    }
    
    private void ValidateFoundingDate(DateOnly foundingDate)
    {
        Validate.OutOfRange((ulong)foundingDate.Year, (ulong)DateTime.MinValue.Year, (ulong)DateTime.Now.Year, "Founding date", Validate.Unit.Year);
    }
}