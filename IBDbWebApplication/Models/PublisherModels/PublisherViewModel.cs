using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.PublisherModels;

public class PublisherViewModel
{
    [Key]
    public long? Id { get; set; }
    
    [DisplayName("Name")]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name must be between 1 and 50 characters", MinimumLength = 1 )]
    [RegularExpression(@"^[a-zA-Z &]+$", ErrorMessage = "Name must only contain letters, spaces, and ampersand.")]
    public string Name { get; set; }
    
    [DisplayName("Founding date")]
    [Required(ErrorMessage = "Founding date is required")]
    [DataType(DataType.Date)]
    public DateTime? FoundingDate { get; set; }
    
    [DisplayName("Description")]
    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description must be between 10 and 1000 characters", MinimumLength = 10 )]
    public string Description { get; set; }

    public IEnumerable<PublisherModel> PublisherModels { get; set; } = new List<PublisherModel>();

    public PublisherViewModel(IEnumerable<PublisherModel> publisherModels) => PublisherModels = publisherModels;

    public PublisherViewModel() { }
}