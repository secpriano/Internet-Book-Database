using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels.SettingModels;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class SettingController : Controller
{
    private readonly SettingContainer _settingContainer = new(new SettingData());
    
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Setting), "Admin");
    }
    
    [HttpPost]
    public IActionResult AddSetting(SettingViewModel settingViewModel)
    {
        _settingContainer.Add(new(settingViewModel.Id, settingViewModel.Description));
        
        return RedirectToAction(nameof(Setting), "Admin");
    }
}