using Interface.DTO;

namespace Business.Entity;

public record Theme(long? Id, string Description)
{
    public Theme (ThemeDTO themeDto) : this(themeDto.Id, themeDto.Description) { }
    public ThemeDTO GetDto() => new ThemeDTO(Id, Description);
    
    public bool ThisEquals(Theme otherTheme)
    {
        return Id == otherTheme.Id && Description == otherTheme.Description;
    }
}