using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels;

public class GenreViewModel
{
    [Key]
    public int Id { get; set; }
    
    [DisplayName("Name")]
    [Required(ErrorMessage = "A name is required")]
    [StringLength(25, ErrorMessage = "Name must be between 2 and 25 characters", MinimumLength = 2 )]
    [RegularExpression(@"^[a-zA-Z ,&]+$", ErrorMessage = "Name can only contain letters, spaces, commas, and ampersands.")]
    public string Name { get; set; }

    public IEnumerable<GenreModel> GenreModels { get; set; }

    public GenreViewModel(IEnumerable<GenreModel> genreModels) => GenreModels = genreModels;

    public GenreViewModel() { }
}