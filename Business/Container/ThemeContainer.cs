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
        
        return _themeData.Add(theme.ToDto());
    }
    
    private void ValidateTheme(Theme theme)
    {
        try
        {
            Task[] tasks = {
                Task.Run(() => ValidateDescription(theme.Description))
            };

            Task.WaitAll(tasks);
        }
        catch (AggregateException ex)
        {
            throw new AggregateException(ex.InnerExceptions);
        }
    }
    
    private void ValidateDescription(string description)
    {
        Validate.OutOfRange((ulong)description.Length, 2, 25, "Description", Validate.Unit.Character);
        
        Validate.Regex(description, @"^[a-zA-Z ,&+-]+$", "Description can only contain letters, spaces, commas, and ampersands.");
    }
}