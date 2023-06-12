using Interface.DTO;
using Interface.Interfaces;

namespace Test.STUB;

public class SettingSTUB : ISettingData
{
    public List<SettingDTO> Settings = new()
    {
        new(1, "Medieval"),
        new(2, "Modern"),
        new(3, "Future")
    };

    public bool Add(SettingDTO entity)
    {
        Settings.Add(entity);
        return Settings.Exists(entity.Equals);
    }

    public SettingDTO GetById(long id)
    {
        throw new NotImplementedException();
    }

    public SettingDTO Update(SettingDTO entity)
    {
        throw new NotImplementedException();
    }

    public bool Delete(long id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<SettingDTO> GetAll()
    {
        throw new NotImplementedException();
    }

    public bool Exist(string uid)
    {
        return Settings.Exists(setting => setting.Description == uid);
    }
}