using Business.Container;
using Business.Entity;
using Data.MsSQL;
using IBDbWebApplication.Models.AdminModels.BookModels;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Controllers;

public class BookController : Controller
{
    private readonly BookContainer _bookContainer = new(new BookData());
    private readonly ReviewContainer _reviewContainer = new(new ReviewData());
    private readonly AuthorContainer _authorContainer = new(new AuthorData());
    private readonly PublisherContainer _publisherContainer = new(new PublisherData());
    private readonly GenreContainer _genreContainer = new(new GenreData());
    private readonly ThemeContainer _themeContainer = new(new ThemeData());
    private readonly SettingContainer _settingContainer = new(new SettingData());
    
    [HttpGet]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
       return View(GetBookViewModel());
    }
    
    private BookViewModel GetBookViewModel()
    {
        return new(
            GetBookModels(),
            GetAuthorModels(),
            GetPublisherModels(),
            GetGenreModels(),
            GetThemeModels(),
            GetSettingModels()
        );
    }

    public IActionResult AddBook(BookViewModel bookViewModel)
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetBookViewModel());
        }
        
        _bookContainer.Add(new(
            bookViewModel.Id,
            bookViewModel.Isbn,
            bookViewModel.Title,
            bookViewModel.Synopsis,
            DateOnly.FromDateTime(bookViewModel.PublishDate.Value),
            bookViewModel.AmountPages,
            bookViewModel.AuthorIds.Select(authorId => new Author(authorId)),
            new(bookViewModel.PublisherId), 
            bookViewModel.GenreIds.Select(genreId => new Genre(genreId)), 
            bookViewModel.ThemeIds.Select(themeId => new Theme(themeId)),
            bookViewModel.SettingIds.Select(settingId => new Setting(settingId)),
            0
        ));

        return RedirectToAction(nameof(Index), bookViewModel);
    }

    [HttpGet]
    public IActionResult Detail(long id)
    {
        BookDetailModel bookDetailModel = new(GetBookModelById(id), GetBookReviewViewModelsByBookId(id));
        
        return View(bookDetailModel);
    }

    [HttpPost]
    public IActionResult Favorite(long id)
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
        long accountId = HttpContext.Session.GetInt32("Account").Value;
        if (_bookContainer.IsFavorite(id, accountId))
        {
            _bookContainer.Unfavorite(id, accountId);
        }
        else
        {
            _bookContainer.Favorite(id, accountId);
        }
        
        return RedirectToAction(nameof(Detail), new {id});
    }
    
    [HttpPost]
    public IActionResult CreateReview(ReviewModel reviewModel)
    {
        if (HttpContext.Session.GetInt32("Account") == null)
            return RedirectToAction("Login", "Account");
        
        BookDetailModel bookDetailModel;

        if (!ModelState.IsValid)
        {
            bookDetailModel = new(GetBookModelById(reviewModel.BookId), GetBookReviewViewModelsByBookId(reviewModel.BookId));

            return View(nameof(Detail), bookDetailModel);

        }
        
        _reviewContainer.Add(new(
            null,
            reviewModel.Title,
            reviewModel.Content,
            new((long)HttpContext.Session.GetInt32("Account")),
            reviewModel.BookId,
            null
        ));

        bookDetailModel = new(GetBookModelById(reviewModel.BookId), GetBookReviewViewModelsByBookId(reviewModel.BookId));

        return View(nameof(Detail), bookDetailModel);
    }

    [HttpPost]
    public IActionResult CreateComment(CommentModel commentModel)
    {
        if (HttpContext.Session.GetInt32("Account") == null)
            return RedirectToAction("Login", "Account");
        
        BookDetailModel bookDetailModel;
        
        if (!ModelState.IsValid)
        {
            bookDetailModel = new(GetBookModelById(commentModel.BookId), GetBookReviewViewModelsByBookId(commentModel.BookId));

            return View(nameof(Detail), bookDetailModel);
        }

        Review review = new(new CommentData());
        review.AddComment(new(
            null,
            commentModel.Content,
            new((long)HttpContext.Session.GetInt32("Account")),
            commentModel.ReviewId
        ));
        
        bookDetailModel = new(GetBookModelById(commentModel.BookId), GetBookReviewViewModelsByBookId(commentModel.BookId));

        return View(nameof(Detail), bookDetailModel);
    }

    public IActionResult Delete(long? id)
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
        if (id == null)
        {
            return NotFound();
        }

        _bookContainer.Delete(id.Value);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult EditBook(BookViewModel bookViewModel)
    {
        if (HttpContext.Session.GetInt32("IsAdmin") == null)
            return RedirectToAction("Login", "Account");
        
        if (!ModelState.IsValid)
        {
            return View(nameof(Index), GetBookViewModel());
        }
        
        _bookContainer.Update(new(
            bookViewModel.Id,
            bookViewModel.Isbn,
            bookViewModel.Title,
            bookViewModel.Synopsis,
            DateOnly.FromDateTime(bookViewModel.PublishDate.Value),
            bookViewModel.AmountPages,
            bookViewModel.AuthorIds.Select(authorId => new Author(authorId)),
            new(bookViewModel.PublisherId), 
            bookViewModel.GenreIds.Select(genreId => new Genre(genreId)), 
            bookViewModel.ThemeIds.Select(themeId => new Theme(themeId)),
            bookViewModel.SettingIds.Select(settingId => new Setting(settingId)),
            0
        ));

        return RedirectToAction(nameof(Index));
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
                ),
                book.Favorites
            )
        );
    }
    
    private BookModel GetBookModelById(long id)
    {
        Book book = _bookContainer.GetById(id);
        return new(
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
            ),
            book.Favorites
        );
    }
    
    private IEnumerable<ReviewViewModel> GetBookReviewViewModelsByBookId(long id)
    {
        return _reviewContainer.GetAllByBookId(id).Select(review => 
            new ReviewViewModel(
                review.Id, 
                review.Title, 
                review.Content, 
                ToAccountModel(review.Account), 
                review.Comments.Select(comment => 
                    new CommentViewModel(
                        comment.Content, 
                        ToAccountModel(comment.Account)
                    )
                )
            )
        );
    }

    private AccountModel ToAccountModel(Account account)
    {
        return new(account.Id, account.Username, account.Email, account.IsAdmin);
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