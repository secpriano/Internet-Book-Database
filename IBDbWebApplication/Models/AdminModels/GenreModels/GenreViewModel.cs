using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels;

public class GenreViewModel
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }

    public IEnumerable<GenreModel> GenreModels { get; set; }

    public GenreViewModel(IEnumerable<GenreModel> genreModels) => GenreModels = genreModels;

    public GenreViewModel() { }
}