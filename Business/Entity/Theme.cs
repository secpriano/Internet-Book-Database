using Interface.DTO;

namespace Business.Entity;

public class Theme
{
    public byte? Id { get; set; }
    public string Description { get; set; }

    public Theme(byte? id, string description) => 
        (Id, Description) = (id, description);

    public Theme(ThemeDTO themeDto) : this(themeDto.Id, themeDto.Description) { }

   public Theme(byte? id)
   {
       Id = id;
   }
    
    public ThemeDTO GetDto() => new ThemeDTO(Id, Description);
}