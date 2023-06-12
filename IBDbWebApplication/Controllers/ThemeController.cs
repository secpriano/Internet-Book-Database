using System.Text.RegularExpressions;
using Business.Container;
using Data.MsSQL;
using IBDbWebApplication.Models.AdminModels.ThemeModels;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class ThemeController : Controller
{
    private readonly ThemeContainer _themeContainer = new(new ThemeData());
    
    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == 0)
            return RedirectToAction("Login", "Account");
        
        return View(GetThemeViewModel());
    }
    
    private ThemeViewModel GetThemeViewModel()
    {
        return new(
            GetThemeModels()
        );
    }

    [HttpPost]
    public IActionResult AddTheme(ThemeViewModel themeViewModel)
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == 0)
            return RedirectToAction("Login", "Account");
        
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetThemeViewModel());
        }

        try
        {
            _themeContainer.Add(new(themeViewModel.Id, themeViewModel.Description));
        }
        catch (AggregateException aggregateException)
        {
            foreach (Exception innerException in aggregateException.InnerExceptions)
            {
                ModelState.AddModelError($"{Regex.Replace(innerException.GetType().GetProperty("Type").GetValue(innerException) as string, @"\s", "")}", innerException.Message);
            }
            return View(nameof(Index), GetThemeViewModel());
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<ThemeModel> GetThemeModels()
    {
        return _themeContainer.GetAll().Select(theme => new ThemeModel(theme.Id, theme.Description));
    }
}