using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class GenreController : Controller
{
    private readonly GenreContainer _genreContainer = new(new GenreData());
    
    [HttpGet]
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Genre), "Admin");
    }
    
    [HttpPost]
    public IActionResult AddGenre(GenreViewModel genreViewModel)
    {
        _genreContainer.Add(new((byte?)genreViewModel.Id, genreViewModel.Name));
        
        return RedirectToAction(nameof(Genre), "Admin");
    }
}