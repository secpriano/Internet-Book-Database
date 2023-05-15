using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.AuthorModels;

public class AuthorViewModel
{
    [Key]
    public long? Id { get; set; } 
    public string Name { get; set; } 
    public string Description { get; set; } 
    public DateOnly BirthDate { get; set; } 
    public DateOnly? DeathDate { get; set; }
    public IEnumerable<GenreModel> GenreIds { get; set; }
    
    public IEnumerable<AuthorModel> AuthorModels { get; set; } = new List<AuthorModel>();
    public IEnumerable<GenreModel> GenreModels { get; set; } = new List<GenreModel>();
    
    public AuthorViewModel(IEnumerable<AuthorModel> authorModels, IEnumerable<GenreModel> genreModels) => 
        (AuthorModels, GenreModels) = (authorModels, genreModels);

    public AuthorViewModel()
    {
        
    }
}