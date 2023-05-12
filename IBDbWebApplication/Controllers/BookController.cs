using System.Linq;
using Business.Container;
using Business.Entity;
using Data;
using IBDbWebApplication.Models.AdminModels.BookModels;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class BookController : Controller
{
    private BookContainer _bookContainer = new(new BookData());
    // GET
    public IActionResult Index()
    {
        return RedirectToAction(nameof(Book), "Admin");
    }

    public IActionResult AddBook(BookViewModel bookViewModel)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
        }
        
        _bookContainer.Add(new(
            bookViewModel.Id,
            bookViewModel.Isbn,
            bookViewModel.Title,
            bookViewModel.Synopsis,
            bookViewModel.PublishDate,
            bookViewModel.AmountPages,
            bookViewModel.Authors.Select(author => 
                new Author(
                    author.Id,
                    author.Name,
                    author.Description,
                    author.BirthDate,
                    author.DeathDate
                )
            ),
            new(bookViewModel.Publisher.Id, bookViewModel.Publisher.Name, bookViewModel.Publisher.FoundingDate, bookViewModel.Publisher.Description), 
            bookViewModel.Genres.Select(genre => 
                new Genre(
                    genre.Id,
                    genre.Name
                )
            ), 
            bookViewModel.Themes.Select(theme => 
                new Theme(
                    theme.Id,
                    theme.Description
                )
            ),
            bookViewModel.Settings.Select(setting => 
                new Setting(
                    setting.Id,
                    setting.Description
                )
            )
        ));

        return RedirectToAction("Book", "Admin");
    }
}