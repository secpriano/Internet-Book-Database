using Interface.DTO;

namespace Business.Entity;

public record Setting(byte? Id, string Description)
{
    public Setting(SettingDTO settingDto) : this(settingDto.Id, settingDto.Description) { }
    public SettingDTO GetDto() => new SettingDTO(Id, Description);
    
    public bool ThisEquals(Setting otherSetting)
    {
        return Id == otherSetting.Id && Description == otherSetting.Description;
    }
}