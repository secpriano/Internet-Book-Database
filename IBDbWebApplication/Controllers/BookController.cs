using Business.Container;
using Business.Entity;
using Data;
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
        BookDetailModel bookDetailModel = new(GetBookModelById(id), GetBookReviewModelsByBookId(id));
        
        return View(bookDetailModel);
    }

    [HttpPost]
    public IActionResult Favorite(long id)
    {
        if (_bookContainer.IsFavorite(id, 1))
        {
            _bookContainer.Unfavorite(id, 1);
        }
        else
        {
            _bookContainer.Favorite(id, 1);
        }
        
        return RedirectToAction(nameof(Detail), new {id});
    }
    
    public IActionResult CreateReview(ReviewModel reviewModel)
    {
        BookDetailModel bookDetailModel;

        if (!ModelState.IsValid)
        {
            bookDetailModel = new(GetBookModelById(reviewModel.BookId), GetBookReviewModelsByBookId(reviewModel.BookId));

            return View(nameof(Detail), bookDetailModel);

        }
        
        _reviewContainer.Add(new(
            null,
            reviewModel.Title,
            reviewModel.Content,
            1,
            reviewModel.BookId,
            null
        ));

        bookDetailModel = new(GetBookModelById(reviewModel.BookId), GetBookReviewModelsByBookId(reviewModel.BookId));

        return View(nameof(Detail), bookDetailModel);
    }

    public IActionResult CreateComment(CommentModel commentModel)
    {
        BookDetailModel bookDetailModel;
        
        if (!ModelState.IsValid)
        {
            bookDetailModel = new(GetBookModelById(commentModel.BookId), GetBookReviewModelsByBookId(commentModel.BookId));

            return View(nameof(Detail), bookDetailModel);
        }

        Review review = new(new CommentData());
        review.AddComment(new(
            commentModel.Id,
            commentModel.ReviewId,
            commentModel.Content,
            1
        ));
        
        bookDetailModel = new(GetBookModelById(commentModel.BookId), GetBookReviewModelsByBookId(commentModel.BookId));

        return View(nameof(Detail), bookDetailModel);
    }

    public IActionResult Delete(long? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        _bookContainer.Delete(id.Value);

        return RedirectToAction(nameof(Index));
    }

    public IActionResult EditBook(BookViewModel bookViewModel)
    {
        if (!ModelState.IsValid)
        {
            return RedirectToAction(nameof(Index));
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
    
    private IEnumerable<ReviewModel> GetBookReviewModelsByBookId(long id)
    {
        return _reviewContainer.GetAllByBookId(id).Select(review => new ReviewModel(review.Id, review.Title, review.Content, review.BookId, review.UserId, review.Comments.Select(comment => new CommentModel(comment.Id, comment.Content, comment.UserId))));
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