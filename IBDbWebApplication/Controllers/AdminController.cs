using Business.Container;
using Data;
using IBDbWebApplication.Models.AdminModels;
using IBDbWebApplication.Models.AdminModels.AuthorModels;
using IBDbWebApplication.Models.AdminModels.BookModels;
using IBDbWebApplication.Models.AdminModels.PublisherModels;
using IBDbWebApplication.Models.AdminModels.SettingModels;
using IBDbWebApplication.Models.AdminModels.ThemeModels;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == 0)
            return RedirectToAction("Login", "Account");

        return View();
    }
}