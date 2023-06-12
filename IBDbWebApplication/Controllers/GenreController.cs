using System.Text.RegularExpressions;
using Business.Container;
using Data.MsSQL;
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
        if (HttpContext.Session.GetInt32("IsAdmin") == 0)
            return RedirectToAction("Login", "Account");

        return View(GetGenreViewModel());
    }
    
    private GenreViewModel GetGenreViewModel()
    {
        return new(
            GetGenreModels()
        );
    }
    
    [HttpPost]
    public IActionResult AddGenre(GenreViewModel genreViewModel)
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == 0)
            return RedirectToAction("Login", "Account");
        
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetGenreViewModel());
        }

        try
        {
            _genreContainer.Add(new((byte?)genreViewModel.Id, genreViewModel.Name));
        }
        catch (AggregateException aggregateException)
        {
            foreach (Exception innerException in aggregateException.InnerExceptions)
            {
                ModelState.AddModelError($"{Regex.Replace(innerException.GetType().GetProperty("Type").GetValue(innerException) as string, @"\s", "")}", innerException.Message);
            }
            return View(nameof(Index), GetGenreViewModel());
        }

        
        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<GenreModel> GetGenreModels()
    {
        return _genreContainer.GetAll().Select(genre => new GenreModel(genre.Id, genre.Name));
    }
}