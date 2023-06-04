using Business.Container;
using Business.Entity;
using Data;
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
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetThemeViewModel());
        }
        
        _themeContainer.Add(new(themeViewModel.Id, themeViewModel.Description));
        
        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<ThemeModel> GetThemeModels()
    {
        return _themeContainer.GetAll().Select(theme => new ThemeModel(theme.Id, theme.Description));
    }
}