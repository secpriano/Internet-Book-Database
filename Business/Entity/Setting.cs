using Interface.DTO;

namespace Business.Entity;

public class Setting 
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
    
    public SettingDTO GetDto() => new SettingDTO(Id, Description);
}