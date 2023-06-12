using System.Text.RegularExpressions;
using Business.Container;
using Data.MsSQL;
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
        if (HttpContext.Session.GetInt32("IsAdmin") == 0)
            return RedirectToAction("Login", "Account");
        
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
        if (HttpContext.Session.GetInt32("IsAdmin") == 0)
            return RedirectToAction("Login", "Account");
        
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetSettingViewModel());
        }

        try
        {
            _settingContainer.Add(new(settingViewModel.Id, settingViewModel.Description));
        }
        catch (AggregateException aggregateException)
        {
            foreach (Exception innerException in aggregateException.InnerExceptions)
            {
                ModelState.AddModelError($"{Regex.Replace(innerException.GetType().GetProperty("Type").GetValue(innerException) as string, @"\s", "")}", innerException.Message);
            }
            return View(nameof(Index), GetSettingViewModel());
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<SettingModel> GetSettingModels()
    {
        return _settingContainer.GetAll().Select(setting => new SettingModel(setting.Id, setting.Description));
    }
}