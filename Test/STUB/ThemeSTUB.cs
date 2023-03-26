using Interface.DTO;

namespace Test.STUB;

public class ThemeSTUB
{
    public List<ThemeDTO> Themes = new()
    {
        new ThemeDTO(1, "Dark"),
        new ThemeDTO(2, "Light")
    };
}