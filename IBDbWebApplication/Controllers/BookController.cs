using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class BookController : Controller
{
    // GET
    public IActionResult Index()
    {
        return RedirectToAction("Book", "Admin");
    }
}