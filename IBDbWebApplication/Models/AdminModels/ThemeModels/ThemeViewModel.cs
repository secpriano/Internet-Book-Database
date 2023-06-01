using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.ThemeModels;

public class ThemeViewModel
{
    public byte? Id { get; set; }
    
    [DisplayName("Description")]
    [Required(ErrorMessage = "Description is required")]
    [StringLength(25, ErrorMessage = "Description must be between 2 and 25 characters", MinimumLength = 2 )]
    [RegularExpression(@"^[a-zA-Z ,&]+$", ErrorMessage = "Description can only contain letters, spaces, commas, and ampersands.")]
    public string Description { get; set; }
    
    public IEnumerable<ThemeModel> ThemeModels { get; set; }
    
    public ThemeViewModel(IEnumerable<ThemeModel> themeModels) => ThemeModels = themeModels;
    
    public ThemeViewModel() { }
}