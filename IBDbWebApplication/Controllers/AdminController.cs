using Business.Container;
using Data;
using IBDbWebApplication.Models.AdminModels.BookModels;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class AdminController : Controller
{
    private readonly BookContainer _bookContainer = new(new BookData());
    private readonly AuthorContainer _authorContainer = new(new AuthorData());
    private readonly PublisherContainer _publisherContainer = new(new PublisherData());
    private readonly GenreContainer _genreContainer = new(new GenreData());
    private readonly ThemeContainer _themeContainer = new(new ThemeData());
    private readonly SettingContainer _settingContainer = new(new SettingData());
    
    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult Book()
    {
        BookViewModel bookViewModel = new(
            GetBookModels(),
            _authorContainer.GetAll().Select(author => new AuthorModel(author.Id, author.Name, author.Description, author.BirthDate, author.DeathDate)),
            _publisherContainer.GetAll().Select(publisher => new PublisherModel(publisher.Id, publisher.Name, publisher.Description)),
            _genreContainer.GetAll().Select(genre => new GenreModel(genre.Id, genre.Name)),
            _themeContainer.GetAll().Select(theme => new ThemeModel(theme.Id, theme.Description)),
            _settingContainer.GetAll().Select(setting => new SettingModel(setting.Id, setting.Description))
        );
        return View("Book/Index", bookViewModel);
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

    private IEnumerable<BookModel> GetBookModels()
    {
        return _bookContainer.GetAll().Select(book =>
            new BookModel(
                book.Id,
                book.Isbn,
                book.Title,
                book.Synopsis,
                book.PublishDate,
                book.AmountPages,
                book.Authors.Select(author =>
                    new AuthorModel(
                        author.Id,
                        author.Name,
                        author.Description,
                        author.BirthDate,
                        author.DeathDate
                    )
                ),
                new(book.Publisher.Id, book.Publisher.Name, book.Publisher.Description),
                book.Genres.Select(genre =>
                    new GenreModel(
                        genre.Id,
                        genre.Name
                    )
                ),
                book.Themes.Select(theme =>
                    new ThemeModel(
                        theme.Id,
                        theme.Description
                    )
                ),
                book.Settings.Select(setting =>
                    new SettingModel(
                        setting.Id,
                        setting.Description
                    )
                )
            )
        );
    }
}