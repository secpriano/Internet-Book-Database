using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels.ThemeModels;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class ThemeController : Controller
{
    private readonly ThemeContainer _themeContainer = new(new ThemeData());
    
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Theme), "Admin");
    }

    [HttpPost]
    public IActionResult AddTheme(ThemeViewModel themeViewModel)
    {
        _themeContainer.Add(new(themeViewModel.Id, themeViewModel.Description));
        
        return RedirectToAction(nameof(Theme), "Admin");
    }
}