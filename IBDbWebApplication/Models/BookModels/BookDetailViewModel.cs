using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using IBDbWebApplication.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace IBDbWebApplication.Models.AdminModels.BookModels;

public class BookDetailViewModel
{
    [BindProperty]
    public ReviewModel ReviewModel { get; set; }
    
    [BindProperty]
    public CommentModel CommentModel { get; set; }
    
    public BookModel BookModel { get; set; }
    public IEnumerable<ReviewViewModel> ReviewViewModels { get; set; } = new List<ReviewViewModel>();
    public bool Shelved { get; set; }

    public BookDetailViewModel(BookModel bookModel, IEnumerable<ReviewViewModel> reviewViewModels, bool shelved) => 
        (BookModel, ReviewViewModels, ReviewModel, CommentModel, Shelved) = (bookModel, reviewViewModels, new() { BookId = (long) bookModel.Id }, new() { BookId = (long) bookModel.Id }, shelved);

    public BookDetailViewModel() { }
}