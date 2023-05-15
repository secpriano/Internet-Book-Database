using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels.AuthorModels;
using IBDbWebApplication.Models.AdminModels.BookModels;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AuthorController : Controller
{
    private readonly AuthorContainer _authorContainer = new(new AuthorData());
    // GET
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Author), "Admin");
    }
    
    public IActionResult AddAuthor(AuthorViewModel authorViewModel)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }
        
        _authorContainer.Add(new(
            authorViewModel.Id,
            authorViewModel.Name,
            authorViewModel.Description,
            authorViewModel.BirthDate,
            authorViewModel.DeathDate,
            authorViewModel.GenreIds.Select(genreId => new Genre(genreId.Id))
        ));

        return RedirectToAction(nameof(Author), "Admin");
    }
}