using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.PublisherModels;

public class PublisherViewModel
{
    [Key]
    public long? Id { get; set; }
    public string Name { get; set; }
    public DateTime FoundingDate { get; set; }
    public string Description { get; set; }

    public IEnumerable<PublisherModel> PublisherModels { get; set; } = new List<PublisherModel>();

    public PublisherViewModel(IEnumerable<PublisherModel> publisherModels) => PublisherModels = publisherModels;

    public PublisherViewModel() { }
}