using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels.SettingModels;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class SettingController : Controller
{
    private readonly SettingContainer _settingContainer = new(new SettingData());
    
    [HttpGet]
    public IActionResult Index()
    {
        return View(GetSettingViewModel());
    }
    
    private SettingViewModel GetSettingViewModel()
    {
        return new(
            GetSettingModels()
        );
    }
    
    [HttpPost]
    public IActionResult AddSetting(SettingViewModel settingViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetSettingViewModel());
        }   
        
        _settingContainer.Add(new(settingViewModel.Id, settingViewModel.Description));
        
        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<SettingModel> GetSettingModels()
    {
        return _settingContainer.GetAll().Select(setting => new SettingModel(setting.Id, setting.Description));
    }
}