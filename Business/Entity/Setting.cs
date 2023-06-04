using Interface.DTO;

namespace Business.Entity;

public class Setting : IEquatable<Setting>
{
    public byte? Id { get; }
    public string Description { get; }

    public Setting(byte? id, string description) => 
        (Id, Description) = (id, description);

    public Setting(SettingDTO settingDto) : this(settingDto.Id, settingDto.Description) { }

    public Setting(byte? id)
    {
        Id = id;
    }
    
    public SettingDTO ToDto() => new SettingDTO(Id, Description);

    public bool Equals(Setting? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Id == other.Id && Description == other.Description;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj.GetType() == GetType() && Equals((Setting) obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, Description);
    }
}