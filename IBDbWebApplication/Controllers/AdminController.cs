using Business.Container;
using Data;
using IBDbWebApplication.Models.AdminModels;
using IBDbWebApplication.Models.AdminModels.AuthorModels;
using IBDbWebApplication.Models.AdminModels.BookModels;
using IBDbWebApplication.Models.AdminModels.PublisherModels;
using IBDbWebApplication.Models.AdminModels.SettingModels;
using IBDbWebApplication.Models.AdminModels.ThemeModels;
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
            GetAuthorModels(),
            GetPublisherModels(),
            GetGenreModels(),
            GetThemeModels(),
            GetSettingModels()
        );
        return View("Book/Index", bookViewModel);
    }
    
    public IActionResult Author()
    {
        AuthorViewModel authorViewModel = new(
            GetAuthorModels(),
            GetGenreModels()
        );
        
        return View("Author/Index", authorViewModel);
    }
    
    public IActionResult Publisher()
    {
        PublisherViewModel publisherViewModel = new(
            GetPublisherModels()
        );
        
        return View("Publisher/Index", publisherViewModel);
    }
    
    public IActionResult Genre()
    {
        GenreViewModel genreViewModel = new(
            GetGenreModels()
        );
        
        return View("Genre/Index", genreViewModel);
    }
    
    public IActionResult Setting()
    {
        SettingViewModel settingViewModel = new(
            GetSettingModels()
        );
        
        return View("Setting/Index", settingViewModel);
    }
    
    public IActionResult Theme()
    {
        ThemeViewModel themeViewModel = new(
            GetThemeModels()
        );
        
        return View("Theme/Index", themeViewModel);
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
                        author.DeathDate,
                        author.Genres.Select(genre =>
                            new GenreModel(
                                genre.Id,
                                genre.Name
                            )
                        )
                    )
                ),
                new(book.Publisher.Id, book.Publisher.Name, book.Publisher.FoundingDate, book.Publisher.Description),
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

    private IEnumerable<AuthorModel> GetAuthorModels()
    {
        return _authorContainer.GetAll().Select(author => new AuthorModel(author.Id, author.Name, author.Description, author.BirthDate, author.DeathDate, author.Genres.Select(genre => new GenreModel(genre.Id, genre.Name))));
    }
    
    private IEnumerable<PublisherModel> GetPublisherModels()
    {
        return _publisherContainer.GetAll().Select(publisher => new PublisherModel(publisher.Id, publisher.Name, publisher.FoundingDate, publisher.Description));
    }
    
    private IEnumerable<GenreModel> GetGenreModels()
    {
        return _genreContainer.GetAll().Select(genre => new GenreModel(genre.Id, genre.Name));
    }
    
    private IEnumerable<ThemeModel> GetThemeModels()
    {
        return _themeContainer.GetAll().Select(theme => new ThemeModel(theme.Id, theme.Description));
    }
    
    private IEnumerable<SettingModel> GetSettingModels()
    {
        return _settingContainer.GetAll().Select(setting => new SettingModel(setting.Id, setting.Description));
    }
}