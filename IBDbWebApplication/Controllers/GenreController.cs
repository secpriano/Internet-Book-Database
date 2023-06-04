using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class GenreController : Controller
{
    private readonly GenreContainer _genreContainer = new(new GenreData());
    
    [HttpGet]
    public IActionResult Index()
    {
        GenreViewModel genreViewModel = new(
            GetGenreModels()
        );
        
        return View(genreViewModel);
    }
    
    [HttpPost]
    public IActionResult AddGenre(GenreViewModel genreViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), new GenreViewModel(GetGenreModels()));
        }
        
        _genreContainer.Add(new((byte?)genreViewModel.Id, genreViewModel.Name));
        
        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<GenreModel> GetGenreModels()
    {
        return _genreContainer.GetAll().Select(genre => new GenreModel(genre.Id, genre.Name));
    }
}