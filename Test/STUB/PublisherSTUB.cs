using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class PublisherSTUB : IPublisherData
{
    public List<PublisherDTO> Publishers = new()
    {
        new(1, "Houghton Mifflin Harcourt",
            new(624, 8, 9),
            "Houghton Mifflin Harcourt is an American publishing company. It is the largest trade book publisher in the United States, and the second largest in the world, after Penguin Random House."),
        new(2, "Penguin Random House",
            new(1484, 7, 1),
            "Penguin Random House is an American multinational publishing company. It is the world's largest trade book publisher, and the largest English-language general trade book publisher in the world."),
    };

    public bool Add(PublisherDTO entity)
    {
        Publishers.Add(entity);
        return Publishers.Exists(entity.Equals);
    }

    public PublisherDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public PublisherDTO Update(PublisherDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<PublisherDTO> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Exist(string uid)
    {
        return Publishers.Exists(publisher => publisher.Name == uid);
    }
}