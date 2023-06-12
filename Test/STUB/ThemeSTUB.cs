using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class ThemeSTUB : IThemeData
{
    public List<ThemeDTO> Themes = new()
    {
        new ThemeDTO(1, "Dark"),
        new ThemeDTO(2, "Light")
    };

    public bool Add(ThemeDTO entity)
    {
        Themes.Add(entity);
        return Themes.Exists(entity.Equals);
    }

    public ThemeDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public ThemeDTO Update(ThemeDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ThemeDTO> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Exist(string uid)
    {
        return Themes.Exists(theme => theme.Description == uid);
    }
}