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
        if (Validate.Exceptions.InnerExceptions.Count > 0)
        {
            throw Validate.Exceptions;
        }
    }
}