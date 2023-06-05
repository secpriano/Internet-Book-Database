using Business.Entity;
using Interface.Interfaces;

namespace Business.Container;

public class SettingContainer
{
    private readonly ISettingData _settingData;
    
    public SettingContainer(ISettingData settingData)
    {
        _settingData = settingData;
    }   
    
    public IEnumerable<Setting> GetAll()
    {
        return _settingData.GetAll().Select(setting => new Setting(setting.Id, setting.Description));
    }
    
    public bool Add(Setting setting)
    {
        ValidateSetting(setting);
        
        return _settingData.Add(setting.ToDto());
    }
    
    private void ValidateSetting(Setting setting)
    {
        try
        {
            Task[] tasks = {
                Task.Run(() => ValidateDescription(setting.Description))
            };  

            Task.WaitAll(tasks);
        }
        catch (AggregateException ex)
        {
            throw new AggregateException(ex.InnerExceptions);
        }
    }
    
    private void ValidateDescription(string description)
    {
        Validate.OutOfRange((ulong)description.Length, 2, 25, "Description", Validate.Unit.Character);
        
        Validate.Regex(description, @"^[a-zA-Z0-9 ,&]+$", "Description can only contain letters, numbers, spaces, commas, and ampersands.");
    }
}