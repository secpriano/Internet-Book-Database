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
            bookViewModel.AuthorIds.Select(authorId => new Author(authorId)),
            new(bookViewModel.PublisherId), 
            bookViewModel.GenreIds.Select(genreId => new Genre(genreId)), 
            bookViewModel.ThemeIds.Select(themeId => new Theme(themeId)),
            bookViewModel.SettingIds.Select(settingId => new Setting(settingId))
        ));

        return RedirectToAction("Book", "Admin");
    }
}