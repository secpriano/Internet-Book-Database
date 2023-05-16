using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.ThemeModels;

public class ThemeViewModel
{
    public byte? Id { get; set; }
    public string Description { get; set; }
    
    public IEnumerable<ThemeModel> ThemeModels { get; set; }
    
    public ThemeViewModel(IEnumerable<ThemeModel> themeModels) => ThemeModels = themeModels;
    
    public ThemeViewModel() { }
}