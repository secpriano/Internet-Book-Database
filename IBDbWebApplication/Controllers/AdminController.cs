using Business.Container;
using Data;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AdminController : Controller
{
    BookContainer bookContainer = new(new BookData());
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Book()
    {
        return View("Book/Index");
    }
    
    public IActionResult Author()
    {
        return View("Author/Index");
    }
    
    public IActionResult Publisher()
    {
        return View("Publisher/Index");
    }
    
    public IActionResult Genre()
    {
        return View("Genre/Index");
    }
    
    public IActionResult Setting()
    {
        return View("Setting/Index");
    }
    
    public IActionResult Theme()
    {
        return View("Theme/Index");
    }
}