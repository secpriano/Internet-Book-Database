using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.AuthorModels;

public class AuthorViewModel
{
    [Key]
    public long? Id { get; set; } 
    
    [DisplayName("Name")]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(1000, ErrorMessage = "Name must be between 1 and 1000 characters", MinimumLength = 1 )]
    [RegularExpression(@"^[a-zA-Z .]+$", ErrorMessage = "Description can only contain letters, spaces, and periods.")]
    public string Name { get; set; } 
    
    [DisplayName("Description")]
    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description must be between 10 and 1000 characters", MinimumLength = 10 )]
    public string Description { get; set; } 
    
    [DisplayName("Birth date")]
    [Required(ErrorMessage = "Birth date is required")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; } 
    
    [DisplayName("Death date")]
    [DataType(DataType.Date)]
    public DateTime? DeathDate { get; set; }
    
    [Required(ErrorMessage = "Genre(s) is required")]
    public IEnumerable<byte> GenreIds { get; set; }
    
    public IEnumerable<AuthorModel> AuthorModels { get; set; } = new List<AuthorModel>();
    public IEnumerable<GenreModel> GenreModels { get; set; } = new List<GenreModel>();
    
    public AuthorViewModel(IEnumerable<AuthorModel> authorModels, IEnumerable<GenreModel> genreModels) => 
        (AuthorModels, GenreModels) = (authorModels, genreModels);

    public AuthorViewModel()
    {
        
    }
}