using Interface.DTO;

namespace Test.STUB;

public class SettingSTUB
{
    public List<SettingDTO> Settings = new()
    {
        new(1, "Medieval"),
        new(2, "Modern"),
        new(3, "Future")
    };

}