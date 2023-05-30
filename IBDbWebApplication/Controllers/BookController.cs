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
    
    [HttpGet]
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
            bookViewModel.SettingIds.Select(settingId => new Setting(settingId)),
            0
        ));

        return RedirectToAction("Book", "Admin");
    }

    [HttpGet]
    public IActionResult Detail(long id)
    {
        BookDetailModel bookDetailModel = new(GetBookModelById(id), GetBookReviewModelsByBookId(id));
        
        return View("~/Views/Admin/Book/Detail.cshtml", bookDetailModel);
    }

    private IEnumerable<ReviewModel> GetBookReviewModelsByBookId(long id)
    {
        return _reviewContainer.GetAllByBookId(id).Select(review => 
            new ReviewModel(
                review.Id, 
                review.Title, 
                review.Content, 
                review.BookId, 
                review.UserId, 
                review.Comments.Select(comment => 
                    new CommentModel(
                        comment.Id, 
                        comment.Content, 
                        comment.UserId
                    )
                )
            )
        );
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
}