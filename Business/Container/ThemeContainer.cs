using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class ThemeContainer
{
    private readonly IThemeData _themeData;
    
    public ThemeContainer(IThemeData themeData)
    {
        _themeData = themeData;
    }   
    
    public IEnumerable<Theme> GetAll()
    {
        return _themeData.GetAll().Select(theme => new Theme(theme.Id, theme.Description));
    }
    
    public bool Add(Theme theme)
    {
        ValidateTheme(theme);
        
        return _themeData.Add(theme.GetDto());
    }
    
    private void ValidateTheme(Theme theme)
    {
        if (Validate.Exceptions.InnerExceptions.Count > 0)
        {
            throw Validate.Exceptions;
        }
    }
}