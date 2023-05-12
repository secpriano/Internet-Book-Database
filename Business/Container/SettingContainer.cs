﻿using Business.Entity;
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
        
        return _settingData.Add(setting.GetDto());
    }
    
    private void ValidateSetting(Setting setting)
    {
        if (Validate.Exceptions.InnerExceptions.Count > 0)
        {
            throw Validate.Exceptions;
        }
    }
}