using IBDbWebApplication.Models.Entity;

namespace IBDbWebApplication.Models.AdminModels.SettingModels;

public class SettingViewModel
{
    public byte? Id { get; }
    public string Description { get; }
    
    public IEnumerable<SettingModel> SettingModels { get; set; }
    
    public SettingViewModel(IEnumerable<SettingModel> settingModels) => SettingModels = settingModels;

    public SettingViewModel() { }
}