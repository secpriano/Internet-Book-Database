using Interface.DTO;

namespace Business.Entity;

public class Theme : IEquatable<Theme>
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
    
    public ThemeDTO ToDto() => new ThemeDTO(Id, Description);

    public bool Equals(Theme? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Description == other.Description;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Theme) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Description);
    }
}