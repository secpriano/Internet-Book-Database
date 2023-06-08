using Business.Container;
using Business.Entity;
using Data.MsSQL;
using IBDbWebApplication.Models.AdminModels.AuthorModels;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AuthorController : Controller
{
    private readonly AuthorContainer _authorContainer = new(new AuthorData());
    private readonly GenreContainer _genreContainer = new(new GenreData());

    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
        return View(GetAuthorViewModel());
    }
    
    private AuthorViewModel GetAuthorViewModel()
    {
        return new(
            GetAuthorModels(),
            GetGenreModels()
        );
    }
    
    public IActionResult AddAuthor(AuthorViewModel authorViewModel)
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetAuthorViewModel());
        }
        
        _authorContainer.Add(new(
            authorViewModel.Id,
            authorViewModel.Name,
            authorViewModel.Description,
            DateOnly.FromDateTime(authorViewModel.BirthDate.Value),
            authorViewModel.DeathDate != null ? DateOnly.FromDateTime(authorViewModel.DeathDate.Value) : null,
            authorViewModel.GenreIds.Select(genreId => new Genre(genreId))
        ));

        return RedirectToAction(nameof(Index));
    }
    
    private IEnumerable<AuthorModel> GetAuthorModels()
    {
        return _authorContainer.GetAll().Select(author => new AuthorModel(author.Id, author.Name, author.Description, author.BirthDate, author.DeathDate, author.Genres.Select(genre => new GenreModel(genre.Id, genre.Name))));
    }
    
    private IEnumerable<GenreModel> GetGenreModels()
    {
        return _genreContainer.GetAll().Select(genre => new GenreModel(genre.Id, genre.Name));
    }
}